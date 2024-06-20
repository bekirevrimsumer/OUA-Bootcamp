using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
	public void EnterState(RBMovement playerMovement)
	{
		playerMovement.animator.SetBool("IsWalking", false);
	}

	public void ExitState(RBMovement playerMovement)
	{
		
	}

	public void UpdateState(RBMovement playerMovement)
	{
		playerMovement.MovePlayer();

		if (playerMovement.rb.velocity.magnitude > 0.1f)
		{
			//Hareketli
			playerMovement.ChangeState(new WalkState());
		}
	}
}
