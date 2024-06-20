using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class CharacterController : MonoBehaviour
{
    public enum ControllerType { TopDown, ClickMove }
    [SerializeField] private ControllerType _currentControllerType;
    public ControllerType CurrentControllerType
    {
        get => _currentControllerType;
        set
        {
            _currentControllerType = value;
            HandleControllerChange();
        }
    }

    [Header("Movement")]
    [SerializeField] private float _speed = 5f;
    

    [Header("References")]
    public Animator Animator;
    public NavMeshAgent NavMeshAgent;
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
        NavMeshAgent = GetComponent<NavMeshAgent>();
        NavMeshAgent.speed = _speed;
        _lastPosition = transform.position;
    }

    [ExecuteInEditMode]
    private void HandleControllerChange()
    {
        switch (_currentControllerType)
        {
            case ControllerType.TopDown:
                NavMeshAgent.enabled = false;
                break;
            case ControllerType.ClickMove:
                NavMeshAgent.enabled = true;
                break;
        }
    }

    private void Update()
    {
        if (HasMovementRestriction()) return;
        switch (CurrentControllerType)
        {
            case ControllerType.TopDown:
                TopDownWASDMovement();
                break;
            case ControllerType.ClickMove:
                ClickToMoveController();
                break;
        }
    }

    private bool HasMovementRestriction()
    {
        return false;
    }

    #region TopDownWASDMovement
    private void TopDownWASDMovement()
    {
        var movement = GetMovement();
        movement = Vector3.ClampMagnitude(movement, 1);

        Quaternion cameraRotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        movement = cameraRotation * movement;

        Animator.SetFloat("Speed", movement.magnitude);

        Vector3 newPosition = transform.position + movement * Time.deltaTime * _speed;
        newPosition.y = Terrain.activeTerrain.SampleHeight(newPosition);
        transform.position = newPosition;
        Rotate(movement);
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

    #region ClickToMove
    private void ClickToMoveController()
    {
        if (!NavMeshAgent.enabled) return;
        if (NavMeshAgent.hasPath)
        {
            if (NavMeshAgent.path.corners.Length >= 1)
            {
                var intendedRot = Quaternion.LookRotation(NavMeshAgent.path.corners[1] - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, intendedRot, 5 * Time.deltaTime);
            }
            else
            {
                transform.LookAt(NavMeshAgent.destination);
            }
        }
        _desiredSpeed = Mathf.InverseLerp(0, 1, (_lastPosition - transform.position).magnitude / Time.deltaTime);
        var currentSpeed = Animator.GetFloat("Speed");
        currentSpeed = Mathf.Lerp(currentSpeed, _desiredSpeed, 500 * Time.deltaTime);
        Animator.SetFloat("Speed", currentSpeed);

        _lastPosition = transform.position;

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            var r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(EventSystem.current.IsPointerOverGameObject()) return;
            if (Physics.Raycast(r, out hit, 100f, GroundLayer))
            {
                NavMeshAgent.destination = hit.point;
                NavMeshAgent.velocity = (NavMeshAgent.destination - transform.position).normalized * NavMeshAgent.speed;
            }
        }
    }
    #endregion
}
