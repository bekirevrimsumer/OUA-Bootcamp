using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IEventListener<LightReflectionEvent>
{
    [System.Serializable]
    public class KeyValuePair
    {
        public string key;
        public GameObject value;
    }

    public List<KeyValuePair> windowList = new List<KeyValuePair>();

    #region Window Operation
    public void OpenWindow(string windowKey)
    {
        windowList.Find(x => x.key == windowKey).value.SetActive(true);
    }

    public void CloseWindow(string windowKey)
    {
        windowList.Find(x => x.key == windowKey).value.SetActive(false);
    }
    
    public void ToggleWindow(string windowKey)
    {
        windowList.Find(x => x.key == windowKey).value.SetActive(!windowList.Find(x => x.key == windowKey).value.activeSelf);
    }

    public void CloseAllWindows()
    {
        foreach (var window in windowList)
        {
            window.value.SetActive(false);
        }
    }
    #endregion

    #region Events

    public void OnEvent(LightReflectionEvent eventType)
    {
        switch (eventType.LightReflectionEventType)
        {
            case LightReflectionEventType.MirrorEnter:
                OpenWindow("InteractMirrorWindow");
                break;
            case LightReflectionEventType.MirrorExit:
                CloseWindow("InteractMirrorWindow");
                break;
            case LightReflectionEventType.MirrorCarry:
                CloseWindow("InteractMirrorWindow");
                OpenWindow("CarryMirrorWindow");
                break;
            case LightReflectionEventType.MirrorDrop:
                CloseWindow("CarryMirrorWindow");
                OpenWindow("InteractMirrorWindow");
                break;
        }
    }

    protected virtual void OnEnable()
    {
        this.StartListeningEvent<LightReflectionEvent>();
    }

    protected virtual void OnDisable()
    {
        this.StopListeningEvent<LightReflectionEvent>();
    }

    #endregion
}
