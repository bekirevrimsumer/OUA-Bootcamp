using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IState
{
	public void EnterState(RBMovement playerMovement)
	{
		playerMovement.animator.SetBool("IsWalking", true);
	}

	public void ExitState(RBMovement playerMovement)
	{
		
	}

	public void UpdateState(RBMovement playerMovement)
	{
		playerMovement.MovePlayer();

		if(playerMovement.rb.velocity.magnitude <= 0.1f)
		{
			//Duruyor - Idle
			playerMovement.ChangeState(new IdleState());
		}
	}
}
