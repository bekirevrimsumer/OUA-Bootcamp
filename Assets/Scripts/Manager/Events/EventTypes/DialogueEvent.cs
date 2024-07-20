using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueEventType
{
    StartDialogue,
    EndDialogue
}

public struct DialogueEvent : IEventType
{
    public DialogueEventType DialogueEventType;
    public DialogueSO DialogueSO;
    
    public DialogueEvent(DialogueEventType dialogueEventType, DialogueSO dialogueSO)
    {
        DialogueEventType = dialogueEventType;
        DialogueSO = dialogueSO;
    }

    static DialogueEvent e;

    public static void Trigger(DialogueEventType dialogueEventType, DialogueSO dialogueSO)
    {
        e.DialogueEventType = dialogueEventType;
        e.DialogueSO = dialogueSO;
        EventManager.TriggerEvent(e);
    }
}
