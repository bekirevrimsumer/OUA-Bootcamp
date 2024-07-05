using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractEventType { MirrorEnter, MirrorExit, MirrorCarry, MirrorDrop, ClimbEnter, ClimbExit }

public struct InteractEvent : IEventType
{
    public InteractEventType InteractEventType;

    public InteractEvent(InteractEventType interactEventType)
    {
        InteractEventType = interactEventType;
    }

    static InteractEvent e;

    public static void Trigger(InteractEventType interactEventType)
    {
        e.InteractEventType = interactEventType;
        EventManager.TriggerEvent(e);
    }
}
