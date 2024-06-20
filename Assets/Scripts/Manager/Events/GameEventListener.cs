using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour, IEventListener<GameEvent>
{
    public UnityEvent OnGameEvent;
    public string EventName;

    public void OnEvent(GameEvent eventType)
    {
        if (eventType.EventName == EventName)
            OnGameEvent?.Invoke();
    }

    protected virtual void OnEnable()
    {
        this.StartListeningEvent();
    }

    protected virtual void OnDisable()
    {
        this.StopListeningEvent();
    }
}
