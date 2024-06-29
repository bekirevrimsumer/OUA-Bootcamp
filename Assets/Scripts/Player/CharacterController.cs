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
    private Quaternion networkRotation;
    private int _currentMirrorViewID;

    [Header("Climbing")]
    public bool IsClimbing = false;
    private bool _inPosition = false;
    private float _posT;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private Quaternion _startRot;
    private Quaternion _endRot;
    private float positionOffset = 0.5f;
    private float offsetFromWall = 0.1f;
    private float speedMultiplier = 1.5f;



    [Header("References")]
    public Animator Animator;
    public LayerMask GroundLayer;
    private Vector3 _lastPosition;
    private float _desiredSpeed;

    private void Start()
    {
        Init();
        Application.targetFrameRate = 120;
    }

    private void Init()
    {
        Animator = GetComponent<Animator>();
        _lastPosition = transform.position;
        if (!photonView.IsMine)
        {
            networkRotation = Quaternion.identity;
        }
    }

    private void Update()
    {
        if(photonView.IsMine)
        {
            TopDownWASDMovement();
            RotateMirror();
        }
        // else 
        // {
        //     if (_currentMirror == null && _currentMirrorViewID != 0)
        //     {
        //         PhotonView mirrorView = PhotonView.Find(_currentMirrorViewID);
        //         if (mirrorView != null)
        //         {
        //             _currentMirror = mirrorView.transform;
        //         }
        //     }
        // }

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

        if (IsClimbing)
        {
            Climb();
        }
        else
        {
            CheckForClimb();
        }
    }

    #region TopDownWASDMovement
    private void TopDownWASDMovement()
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
        LightReflectionEvent.Trigger(LightReflectionEventType.MirrorCarry);
        _isCarry = true;
        Animator.SetBool("IsCarry", true);
		_parentId = transform.GetComponent<PhotonView>().ViewID;
		_currentMirror.GetComponent<PhotonView>().RPC("CarryMirrorRPC", RpcTarget.All, _parentId);
    }

    void DropMirror()
    {
        LightReflectionEvent.Trigger(LightReflectionEventType.MirrorDrop);
        _isCarry = false;
        Animator.SetBool("IsCarry", false);
		_currentMirror.GetComponent<PhotonView>().RPC("DropMirrorRPC", RpcTarget.All);
        _currentMirror = null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Animator.GetBool("IsCarry"));
            stream.SendNext(Animator.GetFloat("Speed"));
            // if (_currentMirror != null)
            // {
            //     // stream.SendNext(_currentMirror.GetComponent<PhotonView>().ViewID);
            //     stream.SendNext(_currentMirror.rotation);
            // }
        }
        else
        {
            Animator.SetBool("IsCarry", (bool)stream.ReceiveNext());
            Animator.SetFloat("Speed", (float)stream.ReceiveNext());

            // if(_currentMirror != null)
            // {
            //     // _currentMirrorViewID = (int)stream.ReceiveNext();
            //     _currentMirror.rotation = (Quaternion)stream.ReceiveNext();
            // }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractMirror") && photonView.IsMine)
        {
            LightReflectionEvent.Trigger(LightReflectionEventType.MirrorEnter);
            _currentMirror = other.transform.parent;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractMirror") && photonView.IsMine)
        {
            LightReflectionEvent.Trigger(LightReflectionEventType.MirrorExit);
            _currentMirror = null;
        }
    }

    #endregion

    #region Climbing

    private void CheckForClimb()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            Debug.DrawRay(transform.position, transform.forward, Color.red);
            if (hit.collider.CompareTag("Climb") && Input.GetKeyDown(KeyCode.Space))
            {
                _startPos = transform.position;
                _endPos = hit.transform.position + hit.transform.forward * positionOffset;
                _startRot = transform.rotation;
                _endRot = hit.transform.rotation;
                _inPosition = true;
                IsClimbing = true;
            }
        }
    }

    private void Climb()
    {
        if (_inPosition)
        {
            Animator.SetTrigger("Climb");
            _posT += Time.deltaTime * speedMultiplier;
            transform.position = Vector3.Lerp(_startPos, _endPos, _posT);
            transform.rotation = Quaternion.Lerp(_startRot, _endRot, _posT);
            if (_posT >= 1)
            {
                _inPosition = false;
                _posT = 0;
            }
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
                _currentMirror.Rotate(Vector3.up, -90 * Time.deltaTime, Space.Self);
            }
            if (Input.GetKey(KeyCode.E))
            {
                _currentMirror.Rotate(Vector3.up, 90 * Time.deltaTime, Space.Self);
            }   
            if(Input.GetKey(KeyCode.X))
            {
                _currentMirror.Rotate(Vector3.right, -90 * Time.deltaTime, Space.Self);
            }
            if(Input.GetKey(KeyCode.C))
            {
                _currentMirror.Rotate(Vector3.right, 90 * Time.deltaTime, Space.Self);
            }
        }
    }

    #endregion

    #region Multiplayer
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
