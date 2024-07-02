public enum InformationEventType { Show, Hide }

public struct InformationEvent : IEventType
{
    public InformationEventType InformationEventType;
    public InfoMessageSO InfoMessageSO;

    public InformationEvent(InformationEventType informationEventType, InfoMessageSO infoMessageSO)
    {
        InformationEventType = informationEventType;
        InfoMessageSO = infoMessageSO;
    }

    static InformationEvent e;

    public static void Trigger(InformationEventType informationEventType, InfoMessageSO infoMessageSO)
    {
        e.InformationEventType = informationEventType;
        e.InfoMessageSO = infoMessageSO;
        EventManager.TriggerEvent(e);
    }
}
