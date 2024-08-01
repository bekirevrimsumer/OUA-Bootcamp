public enum SoundType
{
    Background,
    SFX,
    Footstep
}

public struct SoundEvent : IEventType
{
    public SoundType Type;
    public string Name;
    public float Delay;
    public bool Loop;

    public SoundEvent(SoundType type, string name, float delay, bool loop)
    {
        Type = type;
        Name = name;
        Delay = delay;
        Loop = loop;
    }

    static SoundEvent e;

    public static void Trigger(SoundType type, string name, float delay, bool loop = false)
    {
        e.Type = type;
        e.Name = name;
        e.Delay = delay;
        e.Loop = loop;
        EventManager.TriggerEvent(e);
    }
}
