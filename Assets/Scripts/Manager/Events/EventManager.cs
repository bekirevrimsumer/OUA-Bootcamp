using System;
using System.Collections.Generic;
using UnityEngine;

public struct GameEvent
{
    public string EventName;

    public GameEvent(string eventName)
    {
        EventName = eventName;
    }

    static GameEvent e;

    public static void Trigger(string eventName)
    {
        e.EventName = eventName;
        EventManager.TriggerEvent(e);
    }
}

public interface IEventListenerBase { }

public interface IEventListener<T> : IEventListenerBase
{
    void OnEvent(T eventType);
}

public static class EventManager
{
    private static Dictionary<Type, List<IEventListenerBase>> _subscribers;

    static void Initialize()
    {
        if (_subscribers == null)
            _subscribers = new Dictionary<Type, List<IEventListenerBase>>();
    }

    static EventManager()
    {
        Initialize();
    }


    public static void AddListener<T>(IEventListener<T> listener) where T : struct
    {
        Type type = typeof(T);

        if (!_subscribers.ContainsKey(type))
            _subscribers[type] = new List<IEventListenerBase>();

        if (!IsSubscribed(type, listener))
            _subscribers[type].Add(listener);
    }

    public static void RemoveListener<T>(IEventListener<T> listener) where T : struct
    {
        Type type = typeof(T);

        if (!_subscribers.ContainsKey(type))
        {
            Debug.LogError("Attempting to remove listener with no existing subscribers");
            return;
        }

        List<IEventListenerBase> receivers = _subscribers[type];

        for (int i = receivers.Count - 1; i >= 0; i--)
        {
            if (receivers[i] == listener)
            {
                receivers.Remove(receivers[i]);

                if (receivers.Count == 0)
                    _subscribers.Remove(type);

                return;
            }
        }
    }

    public static void TriggerEvent<T>(T eventType) where T : struct
    {
        List<IEventListenerBase> list;

        if (!_subscribers.TryGetValue(typeof(T), out list))
        {
            Debug.LogError("Attempting to trigger event with no subscribers");
            return;
        }

        for (int i = 0; i < list.Count; i++)
        {
            (list[i] as IEventListener<T>).OnEvent(eventType);
        }
    }

    private static bool IsSubscribed(Type type, IEventListenerBase listener)
    {
        List<IEventListenerBase> receivers;

        if (!_subscribers.TryGetValue(type, out receivers))
            return false;

        bool isExists = false;

        for (int i = 0; i < receivers.Count; i++)
        {
            if (receivers[i] == listener)
            {
                isExists = true;
                break;
            }
        }

        return isExists;
    }
}

public static class EventRegister
{
    public static void StartListeningEvent<T>(this IEventListener<T> listener) where T : struct
    {
        EventManager.AddListener(listener);
    }

    public static void StopListeningEvent<T>(this IEventListener<T> listener) where T : struct
    {
        EventManager.RemoveListener(listener);
    }
}

