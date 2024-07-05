using System.Collections;
using Cinemachine;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class CharacterController : MonoBehaviourPunCallbacks, IPunObservable, IEventListener<MultiplayerEvent>
{
    [Header("Camera")]
    public CinemachineVirtualCamera Camera;
    public Transform CameraFollowTransform;

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
        }
    }

    private void HandleClimbing()
    {
        if (_canClimb && Input.GetKeyDown(KeyCode.Space) && !_isClimbing)
        {
            _isClimbing = true;
            StartCoroutine(Climb());
        }
    }

    private void HandleJumping()
    {
        IsGrounded = Physics.Raycast(transform.position, Vector3.down, GroundDistance, GroundLayer);

        if (!_canClimb && !_isCarry && IsGrounded && Input.GetKeyDown(KeyCode.Space))
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

    #region TopDownWASDMovement
    private void TopDownMovement()
    {
        var movement = GetMovement();
        movement = Vector3.ClampMagnitude(movement, 1);

        Quaternion cameraRotation = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
        movement = cameraRotation * movement;

        Animator.SetFloat("Speed", movement.magnitude);

        Vector3 newPosition = transform.position + movement * Time.deltaTime * _speed;
        // newPosition.y = Terrain.activeTerrain.SampleHeight(newPosition);
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

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractMirror") && photonView.IsMine && !other.transform.parent.GetComponent<Mirror>().IsCarry && !_isCarry)
        {
            InteractEvent.Trigger(InteractEventType.MirrorEnter);
            _currentMirror = other.transform.parent;
        }

        if (other.CompareTag("InformationMessageArea") && photonView.IsMine)
        {
            var infoMessageArea = other.GetComponent<InformantionMessageArea>();
            InformationEvent.Trigger(InformationEventType.Show, infoMessageArea.infoMessageSO);
        }

        if (other.CompareTag("Climb"))
        {
            InteractEvent.Trigger(InteractEventType.ClimbEnter);
            _climbPoint = other.GetComponent<ClimbBox>().climbPoint;
            _canClimb = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractMirror") && photonView.IsMine && !other.transform.parent.GetComponent<Mirror>().IsCarry && !_isCarry)
        {
            InteractEvent.Trigger(InteractEventType.MirrorExit);
            _currentMirror = null;
        }

        if (other.CompareTag("InformationMessageArea") && photonView.IsMine)
        {
            var infoMessageArea = other.GetComponent<InformantionMessageArea>();
            InformationEvent.Trigger(InformationEventType.Hide, infoMessageArea.infoMessageSO);
        }

        if (other.CompareTag("Climb"))
        {
            
            InteractEvent.Trigger(InteractEventType.ClimbExit);
            _canClimb = false;
        }
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

    #region Mirror Rotation

    private void RotateMirror()
    {
        if (!_isCarry && _currentMirror != null)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                _currentMirror.GetComponent<PhotonView>().RPC("RotateMirrorRPC", RpcTarget.All, Vector3.up, -90 * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.E))
            {
                _currentMirror.GetComponent<PhotonView>().RPC("RotateMirrorRPC", RpcTarget.All, Vector3.up, 90 * Time.deltaTime);                
            }   
            if(Input.GetKey(KeyCode.X))
            {
                _currentMirror.GetComponent<PhotonView>().RPC("RotateMirrorRPC", RpcTarget.All, Vector3.right, -90 * Time.deltaTime);
            }
            if(Input.GetKey(KeyCode.C))
            {
                _currentMirror.GetComponent<PhotonView>().RPC("RotateMirrorRPC", RpcTarget.All, Vector3.right, 90 * Time.deltaTime);
            }
        }
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
