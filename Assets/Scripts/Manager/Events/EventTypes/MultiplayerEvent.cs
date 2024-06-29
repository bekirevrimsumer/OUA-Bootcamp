using Photon.Pun;
using UnityEngine;

public enum MultiplayerEventType { JoinGame }

public struct MultiplayerEvent : IEventType
{
    public MultiplayerEventType MultiplayerEventType;
    public GameObject Character;

    public MultiplayerEvent(MultiplayerEventType multiplayerEventEventType, GameObject character)
    {
        MultiplayerEventType = multiplayerEventEventType;
        Character = character;
    }

    static MultiplayerEvent e;

    public static void Trigger(MultiplayerEventType multiplayerEventType, GameObject character)
    {
        e.MultiplayerEventType = multiplayerEventType;
        e.Character = character;
        EventManager.TriggerEvent(e);
    }
}
