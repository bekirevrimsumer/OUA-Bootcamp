public enum SectionEventType 
{ 
    SectionCompleted,
    GameCompleted
}

public struct SectionEvent : IEventType
{
    public SectionEventType SectionEventType;

    public SectionEvent(SectionEventType sectionEventType)
    {
        SectionEventType = sectionEventType;
    }

    static SectionEvent e;

    public static void Trigger(SectionEventType sectionEventType)
    {
        e.SectionEventType = sectionEventType;
        EventManager.TriggerEvent(e);
    }
}
