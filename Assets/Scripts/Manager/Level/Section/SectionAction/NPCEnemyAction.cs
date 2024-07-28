using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCEnemyAction : BaseSectionAction
{
    private NPCController _NPCController;

    public void Start()
    {
        _NPCController = transform.GetComponent<NPCController>();
    }

    public override void Execute()
    {
        base.Execute();
        _NPCController.Animator.SetBool("IsDead", true);
    }
}
