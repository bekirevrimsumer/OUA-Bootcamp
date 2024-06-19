using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GroundCheck groundCheck;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxVelocityChange = 10f;
    [Space]
    [SerializeField] private float jumpHeight = 5f;
    [Space]
    [SerializeField] private float airControl = 0.5f;

    private bool jumping;
    private bool grounded = false;
      

    Rigidbody rb;
    Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
		groundCheck.OnGroundedChange += GroundCheck_OnGroundedChange;
    }

	private void GroundCheck_OnGroundedChange(bool isGrounded)
	{
        grounded = isGrounded;
	}

	// Update is called once per frame
	void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveInput.Normalize();
        jumping = Input.GetButton("Jump");
    }

	private void FixedUpdate()
	{
		if (grounded)
		{
			if (jumping)
			{
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
			}

            rb.AddForce(MoveDir(speed), ForceMode.VelocityChange);
		}
		else
		{
            rb.AddForce(MoveDir(speed * airControl), ForceMode.VelocityChange);
        }
	}

	Vector3 MoveDir(float _speed)
	{
        Vector3 targetDir = new Vector3(moveInput.x, 0f, moveInput.y);
        targetDir = transform.TransformDirection(targetDir);

        targetDir *= _speed;

        Vector3 velocity = rb.velocity;

        if(moveInput.magnitude > 0.5f)
		{
            Vector3 velocityChange = targetDir - velocity;

            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);

            velocityChange.y = 0;

            return velocityChange;
		}
		else
		{
            return new Vector3();
		}
	}
}
