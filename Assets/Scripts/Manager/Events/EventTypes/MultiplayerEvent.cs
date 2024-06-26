using UnityEngine;

public enum MultiplayerEventType { JoinGame }

public struct MultiplayerEvent : IEventType
{
    public MultiplayerEventType MultiplayerEventEventType;
    public GameObject Character;

    public MultiplayerEvent(MultiplayerEventType multiplayerEventEventType, GameObject character)
    {
        MultiplayerEventEventType = multiplayerEventEventType;
        Character = character;
    }

    static MultiplayerEvent e;

    public static void Trigger(MultiplayerEventType multiplayerEventType, GameObject character)
    {
        e.MultiplayerEventEventType = multiplayerEventType;
        e.Character = character;
        EventManager.TriggerEvent(e);
    }
}
