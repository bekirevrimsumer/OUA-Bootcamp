using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightReflectionEventType { HitTarget }

public struct LightReflectionEvent : IEventType
{
    public LightReflectionEventType LightReflectionEventType;

    public LightReflectionEvent(LightReflectionEventType lightReflectionEventType)
    {
        LightReflectionEventType = lightReflectionEventType;
    }

    static LightReflectionEvent e;

    public static void Trigger(LightReflectionEventType lightReflectionEventType)
    {
        e.LightReflectionEventType = lightReflectionEventType;
        EventManager.TriggerEvent(e);
    }
}
