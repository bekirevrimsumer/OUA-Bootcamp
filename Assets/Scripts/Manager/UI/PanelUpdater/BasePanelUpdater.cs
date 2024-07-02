using UnityEngine;

public abstract class BasePanelUpdater<TEventType> : MonoBehaviour
{
    public abstract void UpdatePanel(TEventType eventType);
}
