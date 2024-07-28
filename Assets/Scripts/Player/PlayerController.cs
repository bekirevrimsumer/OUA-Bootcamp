using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Cinemachine.PostFX;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable, IEventListener<MultiplayerEvent>
{
    [Header("Camera")]
    public CinemachineVirtualCamera Camera;
    public Transform CameraFollowTransform;
    public bool IsCameraRotatingEnabled = true;
    public Camera MainCamera;
    private CinemachineFramingTransposer _framingTransposer;

    [Header("Movement")]
    [SerializeField] private float _speed = 5f;


    [Header("Jumping")]
    public float JumpForce = 5f;
    public float GroundDistance = 0.1f;
    public bool IsGrounded = false;

    [Header("Climbing")]
    public float ClimbSpeed = 2f;
    private Transform _climbPoint;
    private bool _canClimb = false;
    private bool _isClimbing = false;
    private Transform _climbWall;
    private bool _canClimbWall = false;
    private bool _isClimbWall = false;


    [Header("Interactable")]
    private Interactable _currentInteractable;
    private bool _canInteract = false;
    private Book _book;
    private List<LineRenderer> _bookSlots = new List<LineRenderer>();

    [Header("References")]
    public Animator Animator;
    public Rigidbody Rb;    
    public LayerMask GroundLayer;
    private SoundManager _soundManager;
    public static event Action BookChanged = delegate { };

    private void Start()
    {
        Init();
        Application.targetFrameRate = 120;
    }

    private void Init()
    {
        Animator = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody>();
        _framingTransposer = Camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _soundManager = FindObjectOfType<SoundManager>();
        _soundManager.footstepSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(photonView.IsMine)
        {
            TopDownMovement();
            HandleClimbing();
            HandleJumping();
            HandleGroundedState();
            HandleClimbWall();
            HandleInteract();
            CameraRotation();

        if(_book != null && _book.isPickedUp && _book.ShelfInteractable.IsInteracting)
        {
            PlaceBook();
        }
        else if(Input.GetMouseButtonDown(0))
        {
            PickBook();
        }

        if(_book != null && _book.isPickedUp && !_book.ShelfInteractable.IsInteracting)
        {
            _book.isPickedUp = false;
            _book.outline.enabled = false;
            DOTween.To(() => transform.position, x => transform.position = x, _book.FirstTransform.position, 0.2f);
        }

        if(_book != null &&     _book.isPickedUp && Input.GetMouseButtonDown(1))
        {
            _book.isPickedUp = false;
            _book.outline.enabled = false;
        }
        }
    }

    private void HandleClimbWall()
    {
        if (_canClimbWall && Input.GetKeyDown(KeyCode.Space))
        {
            _isClimbWall = !_isClimbWall;
            Animator.SetBool("IsClimbWall", _isClimbWall);
            Rb.useGravity = !_isClimbWall;
        }

        if (_isClimbWall)
        {
            ClimbWall();
        }
    }

    private void ClimbWall()
    {
        Animator.SetBool("IsClimbWall", true);
        var movement = GetMovement();

        Animator.SetFloat("Speed", movement.magnitude);

        if (Input.GetKey(KeyCode.W))
        {
            float targetY = transform.position.y + ClimbSpeed * 5 * Time.deltaTime;
            transform.DOMoveY(targetY, 0.3f).SetEase(Ease.Linear);
        }
    }

    private void HandleClimbing()
    {
        if (_canClimb && Input.GetKeyDown(KeyCode.Space) && !_isClimbing && !_isClimbWall)
        {
            transform.LookAt(new Vector3(_climbPoint.position.x, transform.position.y, _climbPoint.position.z));
            _isClimbing = true;
            StartCoroutine(Climb());
        }
    }

    private void HandleJumping()
    {
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, GroundDistance, GroundLayer);

        if (!_canClimb && !_canClimbWall && IsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    private void HandleGroundedState()
    {
        Animator.SetBool("IsGrounded", IsGrounded);
    }

    private void HandleInteract()
    {
        if (Input.GetKeyDown(KeyCode.F) && _currentInteractable != null && _canInteract)
        {
            _currentInteractable.Interact();
        }
        
        if(_currentInteractable != null && _currentInteractable is MirrorInteractable mirrorInteractable)
        {
            if(Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.C))
            {
                mirrorInteractable.Interact();
            }
        }
    }

    #region TopDownWASDMovement
    private void TopDownMovement()
    {
        if(_isClimbWall) return;

        if(!_currentInteractable is MirrorInteractable && _currentInteractable != null && _currentInteractable.IsInteracting) return;

        var movement = GetMovement();
        movement = Vector3.ClampMagnitude(movement, 1);

        Quaternion cameraRotation = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
        movement = cameraRotation * movement;

        Animator.SetFloat("Speed", movement.magnitude);

        Vector3 newPosition = transform.position + movement * Time.deltaTime * _speed;
        transform.position = newPosition;
        Rotate(movement);

        photonView.RPC("UpdatePositionAndRotation", RpcTarget.Others, transform.position, transform.rotation);
    }

    private void Rotate(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private Vector3 GetMovement()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var movement = new Vector3(horizontal, 0, vertical);

        return movement;
    }

    private void CameraRotation()
    {
        if (!IsCameraRotatingEnabled) return;

        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * 2;

            Camera.transform.RotateAround(transform.position, Vector3.up, mouseX);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            _framingTransposer.m_CameraDistance = Mathf.Clamp(_framingTransposer.m_CameraDistance - scroll * 2f, 8f, 13f);
        }
    }

    #endregion

    #region Mirror Interact

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Animator.GetBool("IsCarry"));
            stream.SendNext(Animator.GetFloat("Speed"));
        }
        else
        {
            Animator.SetBool("IsCarry", (bool)stream.ReceiveNext());
            Animator.SetFloat("Speed", (float)stream.ReceiveNext());
        }
    }

    #endregion

    #region Collision

    void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        switch (other.tag)
        {
            case "InformationMessageArea":
                HandleInformationMessageAreaEnter(other);
                break;
            case "Climb":
                HandleClimbEnter(other);
                break;
            case "ClimbWall":
                HandleClimbWallEnter(other);
                break;
            case "WallUp":
                HandleWallUpEnter();
                break;
            case "DialogueArea":
                HandleDialogueAreaEnter(other);
                break;
            case "Interactable":
                HandleInteractableEnter(other);
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine) return;

        switch (other.tag)
        {
            case "InformationMessageArea":
                HandleInformationMessageAreaExit(other);
                break;
            case "Climb":
                HandleClimbExit(other);
                break;
            case "ClimbWall":
                HandleClimbWallExit(other);
                break;
            case "WallUp":
                HandleWallUpExit();
                break;
            case "Interactable":
                HandleInteractableExit(other);
                break;
        }
    }

    void HandleInformationMessageAreaEnter(Collider other)
    {
        var infoMessageArea = other.GetComponent<InformantionMessageArea>();
        if (infoMessageArea != null)
        {
            InformationEvent.Trigger(InformationEventType.Show, infoMessageArea.infoMessageSO);
        }
    }

    void HandleInformationMessageAreaExit(Collider other)
    {
        var infoMessageArea = other.GetComponent<InformantionMessageArea>();
        if (infoMessageArea != null)
        {
            InformationEvent.Trigger(InformationEventType.Hide, infoMessageArea.infoMessageSO);
        }
    }

    void HandleClimbEnter(Collider other)
    {
        var climbBox = other.GetComponent<ClimbBox>();
        if (climbBox != null)
        {
            _canClimb = true;
            _climbPoint = climbBox.climbPoint;
            InteractEvent.Trigger(InteractEventType.ClimbEnter, null, false, false, false);
        }
    }

    void HandleClimbExit(Collider other)
    {
        _canClimb = false;
        InteractEvent.Trigger(InteractEventType.ClimbExit, null, false, false, false);
    }

    void HandleClimbWallEnter(Collider other)
    {
        _canClimbWall = true;
        _climbWall = other.transform;
    }

    void HandleClimbWallExit(Collider other)
    {
        _canClimbWall = false;
    }

    void HandleWallUpEnter()
    {
        if (_isClimbWall)
        {
            Animator.SetTrigger("WallUp");
            transform.DOMoveY(transform.position.y + 1.6f, 0.25f).SetEase(Ease.Linear)
                .onComplete += () => transform.DOMove(transform.position + transform.forward * 1.1f, 1.1f).SetEase(Ease.Linear);
        }
    }

    void HandleWallUpExit()
    {
        _isClimbWall = false;
        Animator.SetBool("IsClimbWall", false);
        Rb.useGravity = true;
    }

    void HandleDialogueAreaEnter(Collider other)
    {
        var dialogueArea = other.GetComponent<DialogueArea>();
        if (dialogueArea != null && !dialogueArea.IsDialogueStartBefore)
        {
            dialogueArea.IsDialogueStartBefore = true;
            DialogueEvent.Trigger(DialogueEventType.StartDialogue, dialogueArea.DialogueSO);
        }
    }

    private void PickBook()
    {
        RaycastHit hit;
        
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Interactable")))
        {
            _bookSlots = hit.transform.parent.GetComponentsInChildren<LineRenderer>().ToList();

            if (_bookSlots.Count != 0)
            {
                foreach (var bookSlot in _bookSlots)
                {
                    bookSlot.enabled = true;
                }
            }

            _book = hit.transform.GetComponent<Book>();
            _book.outline.enabled = true;
            _book.isPickedUp = true;
        }
    }

    private void PlaceBook()
    {
        RaycastHit hit;
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("BookSlot")))
        {
            var transformPoint = hit.transform.GetChild(0);
            DOTween.To(() => _book.FirstTransform.position, x => _book.FirstTransform.position = x, transformPoint.position, 0.2f);
            _book.transform.DOLocalRotate(new Vector3(0, 90, 0), 0.2f);

            if(Input.GetMouseButtonDown(0))
            {
                
                _book.BookSlot = hit.transform.GetComponent<BookSlot>();
                _book.isPickedUp = false;
                _book.outline.enabled = false;

                BookChanged?.Invoke();
                
                if(_bookSlots.Count != 0)
                {
                    foreach (var bookSlot in _bookSlots)
                    {
                        bookSlot.enabled = false;
                    }
                }
            }
        }
    }

    void HandleInteractableEnter(Collider other)
    {
        var interactable = other.GetComponent<Interactable>();
        _currentInteractable = interactable;
        _currentInteractable.CurrentPlayer = this;
        _canInteract = true;

        if(interactable == null) return;

        if(interactable is MirrorInteractable mirrorInteractable)
        {
            if(!mirrorInteractable.Mirror.IsCarry && mirrorInteractable.Mirror.IsInteractable)
            {
                InteractEvent.Trigger(InteractEventType.MirrorEnter); 
            }

           return;
        }

        InteractEvent.Trigger(InteractEventType.InteractableObjectEnter);
    }

    void HandleInteractableExit(Collider other)
    {
        var interactable = other.GetComponent<Interactable>();
        _currentInteractable = null;
        _canInteract = false;

        if(interactable == null) return;

        if(interactable is MirrorInteractable mirrorInteractable)
        {
            if(mirrorInteractable.Mirror != null && !mirrorInteractable.Mirror.IsCarry)
            {
                InteractEvent.Trigger(InteractEventType.MirrorExit); 
            }

           return;
        }

        InteractEvent.Trigger(InteractEventType.InteractableObjectExit);
    }

    #endregion

    #region Climbing

    IEnumerator Climb()
    {
        Animator.SetTrigger("Climb");
        while (Vector3.Distance(transform.position, _climbPoint.position) > 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _climbPoint.position, ClimbSpeed * Time.deltaTime);
            yield return null;
        }

        _isClimbing = false;
    }

    #endregion



    #region Multiplayer Events
    public void IsLocalPlayer()
    {
        Camera.gameObject.SetActive(true);
        Camera.Follow = CameraFollowTransform;
    }

    public void OnEvent(MultiplayerEvent eventType)
    {
        switch (eventType.MultiplayerEventType)
        {
            case MultiplayerEventType.JoinGame:
                if (photonView.IsMine)
                {
                    IsLocalPlayer();
                }
                break;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        this.StartListeningEvent<MultiplayerEvent>();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        this.StopListeningEvent<MultiplayerEvent>();
    }

    #endregion

    #region Animation Sounds

    public void Footstep()
    {
        SoundEvent.Trigger(SoundType.Footstep, "FootStep", 0.15f, 0, false);
    }

    #endregion
}
