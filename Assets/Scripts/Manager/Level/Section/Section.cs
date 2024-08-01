using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

[Serializable]
public class Section : BaseSectionAction, IEventListener<SectionEvent>
{
    public List<BaseSectionAction> Actions = new List<BaseSectionAction>();
    public List<BaseCondition> Conditions = new List<BaseCondition>();
    public DialogueSO DialogueSO;
    private bool _isCompleted;

    public override void Execute()
    {
        if (!_isCompleted)
        {
            photonView.RPC("ExecuteActions", RpcTarget.All);
        }
    }

    [PunRPC]
    private void ExecuteActions()
    {
        foreach (var action in Actions)
        {
            action.Execute();
        }
        SoundEvent.Trigger(SoundType.SFX, "Target", 0, false);
        
        if(DialogueSO != null)
            DialogueEvent.Trigger(DialogueEventType.StartDialogue, DialogueSO);
        
        _isCompleted = true;
    }

    public void CheckCompletion()
    {
        if (!_isCompleted && Conditions.All(condition => condition.IsCompleted()))
        {
            Execute();
        }
    }

    public void OnEvent(SectionEvent eventType)
    {
        switch (eventType.SectionEventType)
        {
            case SectionEventType.SectionCompleted:
                CheckCompletion();
                break;
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        this.StartListeningEvent<SectionEvent>();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        this.StopListeningEvent<SectionEvent>();
    }
}
