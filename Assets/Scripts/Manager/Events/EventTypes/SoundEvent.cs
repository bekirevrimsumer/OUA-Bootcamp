public enum SoundType
{
    Background,
    SFX,
}

public struct SoundEvent : IEventType
{
    public SoundType Type;
    public string Name;
    public float Volume;
    public float Delay;
    public bool Loop;

    public SoundEvent(SoundType type, string name, float volume, float delay, bool loop)
    {
        Type = type;
        Name = name;
        Volume = volume;
        Delay = delay;
        Loop = loop;
    }

    static SoundEvent e;

    public static void Trigger(SoundType type, string name, float volume, float delay, bool loop = false)
    {
        e.Type = type;
        e.Name = name;
        e.Volume = volume;
        e.Delay = delay;
        e.Loop = loop;
        EventManager.TriggerEvent(e);
    }
}
