using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{    
	private float waitTime;
    private float timer;

    public IdleState(float waitTime = 5.0f)
    {
        this.waitTime = waitTime;
    }

	public void EnterState(NPCController npcController)
	{
		npcController.Animator.SetBool("IsWalking", false);
        npcController.Agent.isStopped = true;
        timer = 0f;
	}

	public void ExitState(NPCController npcController)
	{
		npcController.Agent.isStopped = false;
	}

	public void UpdateState(NPCController npcController)
	{
		timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            npcController.ChangeState(npcController._walkState);
        }
	}
}
