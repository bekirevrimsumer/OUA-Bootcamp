using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBMovement : MonoBehaviour
{
	float playerHeight = 2f;

	[SerializeField] Transform orientation;

    [Header("Movement")]
    public float moveSpeed = 6f;
	float movementMultiplier = 10f;
	[SerializeField] float airMultiplier = 0.4f;

	[Header("Sprinting")]
	[SerializeField] float walkSpeed = 8f;
	[SerializeField] float sprintSpeed = 10f;
	[SerializeField] float acceleration = 16f;

	[Header("Jumping")]
	public float jumpForce = 5f;

	[Header("Drag")]
	[SerializeField] float groundDrag = 6f;
	[SerializeField] float airDrag = 0.5f;

    float horizontalMovement;
    float verticalMovement;

	[Header("Climbing")]
	[SerializeField] private ClimbCheck climbCheck;
	[SerializeField] private float climbSpeed = 2f;
	private bool isClimbing = false;

	[Header("Ground Detection")]
	[SerializeField] private GroundCheck groundCheck;
	private bool isGrounded;

	[Header("Movement State Machine")]
	private IState currentMovementState;

	[Header("Animator")]
	public Animator animator;

    Vector3 moveDirection;
	Vector3 slopeMoveDirection;

    public Rigidbody rb;

	RaycastHit slopeHit;

	private bool OnSlope()
	{
		if(Physics.Raycast(transform.position,Vector3.down,out slopeHit,playerHeight / 2 + 0.5f))
		{
			if(slopeHit.normal != Vector3.up)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
	}

	private void Start()
	{
		currentMovementState = new IdleState();
		currentMovementState.EnterState(this);

        rb = GetComponent<Rigidbody>();
		//rb.freezeRotation = true;
		groundCheck.OnGroundedChange += GroundCheck_OnGroundedChange;
		climbCheck.OnClimbChange += ClimbCheck_OnClimbChange;
	}

	private void ClimbCheck_OnClimbChange(bool value)
	{
		rb.useGravity = !value;
		isClimbing = value;
	}

	private void GroundCheck_OnGroundedChange(bool value)
	{
		isGrounded = value;
	}

	private void Update()
	{
		MyInput();
		ControlDrag();
		ControlSpeed();

		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			Jump();
		}

		slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
	}

	void MyInput()
	{
		horizontalMovement = Input.GetAxisRaw("Horizontal");
		verticalMovement = Input.GetAxisRaw("Vertical");

		if (isClimbing)
		{
			//Tirmaniyor ise hareket yonunu degistiriyor
			moveDirection = orientation.right * horizontalMovement + orientation.up * verticalMovement;
		}
		else
		{
			moveDirection = orientation.right * horizontalMovement + orientation.forward * verticalMovement;
		}

	}

	void Jump()
	{
		if (isGrounded && !isClimbing)
		{
			rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
			rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
		}
	}

	void ControlSpeed()
	{
		if(Input.GetKey(KeyCode.LeftShift) && isGrounded)
		{
			//Hızlı Kosma
			moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
		}
		else if (isClimbing)
		{
			//Tirmanma
			moveSpeed = Mathf.Lerp(moveSpeed, climbSpeed, acceleration * Time.deltaTime);
		}
		else
		{
			//Normal kosma
			moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
		}
	}

	void ControlDrag()
	{
		if (isGrounded)
		{
			rb.drag = groundDrag;
		}
		else
		{
			rb.drag = airDrag;
		}
	}

	private void FixedUpdate()
	{
		//MovePlayer();
		currentMovementState.UpdateState(this);
	}

	public void ChangeState(IState newState)
	{
		currentMovementState.ExitState(this);
		currentMovementState = newState;
		newState.EnterState(this);
	}

	public void MovePlayer()
	{
		if (isGrounded && !OnSlope())
		{
			rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
		}
		else if(OnSlope() && isGrounded)
		{
			rb.AddForce(slopeMoveDirection.normalized * moveSpeed * movementMultiplier, ForceMode.Acceleration);
		}
		else if (!isGrounded)
		{
			rb.AddForce(moveDirection.normalized * moveSpeed * movementMultiplier * airMultiplier, ForceMode.Acceleration);
		}
	}
}
