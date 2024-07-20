using System.Collections;
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
    private CinemachineFramingTransposer _framingTransposer;
    private CinemachineVolumeSettings _volumeSettings;

    [Header("Movement")]
    [SerializeField] private float _speed = 5f;

    [Header("Interact Objects")]
    public Transform InteractObjectTransform;
    private bool _isCarry = false;
    private Transform _currentMirror;
    private int _parentId;

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

    [Header("Padlock")]
    private GameObject _currentPadlock;
    private bool _canFocusPadlock = false;
    private bool _isPadlockFocused = false;

    [Header("Interactable")]
    private IInteractable _currentInteractable;

    [Header("References")]
    public Animator Animator;
    public Rigidbody Rb;    
    public LayerMask GroundLayer;

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
        _volumeSettings = Camera.GetComponent<CinemachineVolumeSettings>();
    }

    private void Update()
    {
        if(photonView.IsMine)
        {
            TopDownMovement();
            RotateMirror();
            HandleClimbing();
            HandleJumping();
            HandleGroundedState();
            HandleMirrorPickup();
            HandleClimbWall();
            HandleLockFocus();
            HandleInteract();
            CameraRotation();
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
            float targetY = transform.position.y + ClimbSpeed * 10 * Time.deltaTime;
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

        if (!_canClimb && !_canClimbWall && !_isCarry && IsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    private void HandleGroundedState()
    {
        Animator.SetBool("IsGrounded", IsGrounded);
    }

    private void HandleMirrorPickup()
    {
        if (Input.GetKeyDown(KeyCode.F) && _currentMirror != null)
        {
            if (!_isCarry)
            {
                PickupMirror();
            }
            else
            {
                DropMirror();
            }
        }
    }

    private void HandleLockFocus()
    {
        if (Input.GetKeyDown(KeyCode.F) && _canFocusPadlock && !_isPadlockFocused)
        {
            _isPadlockFocused = true;
            Camera.Follow = _currentPadlock.transform;
            Camera.transform.DORotate(new Vector3(45, 110, 0), 1f);
            IsCameraRotatingEnabled = false;
            
            DOTween.To(() => _framingTransposer.m_CameraDistance, x => _framingTransposer.m_CameraDistance = x, 1f, 1f);
            DOTween.To(() => _framingTransposer.m_TrackedObjectOffset.y, y => _framingTransposer.m_TrackedObjectOffset.y = y, 0f, 1f);

            _volumeSettings.m_Profile.TryGet<DepthOfField>(out var depthOfField);
            depthOfField.active = false;
        }
        else if (Input.GetKeyDown(KeyCode.F) && _isPadlockFocused)
        {
            _isPadlockFocused = false;
            Camera.Follow = CameraFollowTransform;
            IsCameraRotatingEnabled = true;
            Camera.transform.DORotate(new Vector3(50, 0, 0), 1f);
            
            DOTween.To(() => _framingTransposer.m_CameraDistance, x => _framingTransposer.m_CameraDistance = x, 13f, 1f);
            DOTween.To(() => _framingTransposer.m_TrackedObjectOffset.y, y => _framingTransposer.m_TrackedObjectOffset.y = y, 1.2f, 1f);

            _volumeSettings.m_Profile.TryGet<DepthOfField>(out var depthOfField);
            depthOfField.active = true;
        }
    }

    private void HandleInteract()
    {
        if (Input.GetKeyDown(KeyCode.F) && _currentInteractable != null)
        {
            _currentInteractable.Interact();
        }
    }

    #region TopDownWASDMovement
    private void TopDownMovement()
    {
        if(_isClimbWall || _isPadlockFocused) return;

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
    void PickupMirror()
    {
        InteractEvent.Trigger(InteractEventType.MirrorCarry);
        _isCarry = true;
        Animator.SetBool("IsCarry", true);
		_parentId = transform.GetComponent<PhotonView>().ViewID;
		_currentMirror.GetComponent<PhotonView>().RPC("CarryMirrorRPC", RpcTarget.All, _parentId);
        Rb.useGravity = false;
        Rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    void DropMirror()
    {
        InteractEvent.Trigger(InteractEventType.MirrorDrop);
        _isCarry = false;
        Animator.SetBool("IsCarry", false);
		_currentMirror.GetComponent<PhotonView>().RPC("DropMirrorRPC", RpcTarget.All);
        _currentMirror = null;
        Rb.useGravity = true;
        Rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

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

    #region Mirror Rotation

    private void RotateMirror()
    {
        if (!_isCarry && _currentMirror != null)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                _currentMirror.GetComponent<PhotonView>().RPC("RotateMirrorRPC", RpcTarget.All, Vector3.up, -45 * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.E))
            {
                _currentMirror.GetComponent<PhotonView>().RPC("RotateMirrorRPC", RpcTarget.All, Vector3.up, 45 * Time.deltaTime);                
            }   
            if(Input.GetKey(KeyCode.X))
            {
                _currentMirror.GetComponent<PhotonView>().RPC("RotateMirrorRPC", RpcTarget.All, Vector3.right, -45 * Time.deltaTime);
            }
            if(Input.GetKey(KeyCode.C))
            {
                _currentMirror.GetComponent<PhotonView>().RPC("RotateMirrorRPC", RpcTarget.All, Vector3.right, 45 * Time.deltaTime);
            }
        }
    }

    #endregion

    #region Collision

    void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        switch (other.tag)
        {
            case "InteractMirror":
                HandleInteractMirrorEnter(other);
                break;
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
            case "Switch":
                HandleSwitchEnter(other);
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
            case "InteractMirror":
                HandleInteractMirrorExit(other);
                break;
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
            case "Switch":
                HandleSwitchExit();
                break;
            case "Interactable":
                HandleInteractableExit(other);
                break;
        }
    }

    void HandleInteractMirrorEnter(Collider other)
    {
        var mirror = other.transform.parent.GetComponent<Mirror>();
        if (mirror != null && !mirror.IsCarry && !_isCarry)
        {
            InteractEvent.Trigger(InteractEventType.MirrorEnter);
            _currentMirror = other.transform.parent;
        }
    }

    void HandleInteractMirrorExit(Collider other)
    {
        var mirror = other.transform.parent.GetComponent<Mirror>();
        if (mirror != null && !mirror.IsCarry && !_isCarry)
        {
            InteractEvent.Trigger(InteractEventType.MirrorExit);
            _currentMirror = null;
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
            InteractEvent.Trigger(InteractEventType.ClimbEnter);
            _climbPoint = climbBox.climbPoint;
            _canClimb = true;
        }
    }

    void HandleClimbExit(Collider other)
    {
        InteractEvent.Trigger(InteractEventType.ClimbExit);
        _canClimb = false;
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

    void HandleSwitchEnter(Collider other)
    {
        _canFocusPadlock = true;
        _currentPadlock = other.gameObject.FindChildObject("padlock");
    }

    void HandleSwitchExit()
    {
        _canFocusPadlock = false;
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

    void HandleInteractableEnter(Collider other)
    {
        var interactable = other.GetComponent<IInteractable>();
        _currentInteractable = interactable;
        InteractEvent.Trigger(InteractEventType.DoorLockKeyEnter);
    }

    void HandleInteractableExit(Collider other)
    {
        _currentInteractable = null;
        InteractEvent.Trigger(InteractEventType.DoorLockKeyExit);
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
}
