using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IState
{
	private bool destinationSet = false;

	public void EnterState(NPCController npcController)
	{
		npcController.Animator.SetBool("IsWalking", true);
		destinationSet = false;
	}

	public void ExitState(NPCController npcController)
	{
		
	}

	public void UpdateState(NPCController npcController)
	{
		if (!destinationSet)
        {
            npcController.MovePlayer();
            destinationSet = true;
        }

		if (npcController.Agent.remainingDistance <= npcController.Agent.stoppingDistance && !npcController.Agent.pathPending)
        {
            npcController.ChangeState(npcController._idleState);
        }
	}
}
