using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractEventType { MirrorEnter, MirrorExit, MirrorCarry, MirrorDrop, ClimbEnter, ClimbExit, DoorLockKeyEnter, DoorLockKeyShow, DoorLockKeyHide, DoorLockKeyExit }

public struct InteractEvent : IEventType
{
    public InteractEventType InteractEventType;
    public InteractItemBaseSO InteractItemBaseSO;

    public InteractEvent(InteractEventType interactEventType, InteractItemBaseSO interactItemBaseSO = null)
    {
        InteractEventType = interactEventType;
        InteractItemBaseSO = interactItemBaseSO;
    }

    static InteractEvent e;

    public static void Trigger(InteractEventType interactEventType, InteractItemBaseSO interactItemBaseSO = null)
    {
        e.InteractEventType = interactEventType;
        e.InteractItemBaseSO = interactItemBaseSO;
        EventManager.TriggerEvent(e);
    }
}
