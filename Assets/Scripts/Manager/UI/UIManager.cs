using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour, IEventListener<InteractEvent>, IEventListener<InformationEvent>
{
    [System.Serializable]
    public class KeyValuePair
    {
        public string key;
        public GameObject value;
    }

    private GameObject _currentWindow;

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

    public GameObject GetWindow(string windowKey)
    {
        return windowList.Find(x => x.key == windowKey).value;
    }
    #endregion

    public void UpdatePanel<T>(string key, T eventType)
    {
        var panel = windowList.Find(p => p.key == key).value;
        var updater = panel.GetComponent<BasePanelUpdater<T>>();
        updater.UpdatePanel(eventType);
    }

    public void AnimatePanel(string key, bool isShow, TweenCallback onComplete = null)
    {
        _currentWindow = GetWindow(key);
        if (isShow)
        {
            _currentWindow.GetComponent<RectTransform>().DOScaleX(1, 0.2f).SetEase(Ease.OutBack);
            _currentWindow.GetComponent<RectTransform>().DOScaleY(1, 0.2f).SetEase(Ease.OutBack).onComplete += onComplete;
        }
        else
        {
            _currentWindow.GetComponent<RectTransform>().DOScaleX(0, 0.2f).SetEase(Ease.OutBack);
            _currentWindow.GetComponent<RectTransform>().DOScaleY(0, 0.2f).SetEase(Ease.OutBack).onComplete += onComplete;
        }
    }

    #region Events

    public void OnEvent(InteractEvent eventType)
    {
        switch (eventType.InteractEventType)
        {
            case InteractEventType.MirrorEnter:
                AnimatePanel("InteractMirrorWindow", true);
                break;
            case InteractEventType.MirrorExit:
                AnimatePanel("InteractMirrorWindow", false);
                break;
            case InteractEventType.MirrorCarry:
                AnimatePanel("InteractMirrorWindow", false, () => { AnimatePanel("CarryMirrorWindow", true);});
                break;
            case InteractEventType.MirrorDrop:
                AnimatePanel("CarryMirrorWindow", false, () => { AnimatePanel("InteractMirrorWindow", true);});
                break;
            case InteractEventType.ClimbEnter:
                AnimatePanel("ClimbWindow", true);
                break;
            case InteractEventType.ClimbExit:
                AnimatePanel("ClimbWindow", false);
                break;
        }
    }

    public void OnEvent(InformationEvent eventType)
    {
        switch (eventType.InformationEventType)
        {
            case InformationEventType.Show:
                AnimatePanel("InfoPanel", true);
                break;
            case InformationEventType.Hide:
                AnimatePanel("InfoPanel", false);
                break;
        }
    }

    protected virtual void OnEnable()
    {
        this.StartListeningEvent<InteractEvent>();
        this.StartListeningEvent<InformationEvent>();
    }

    protected virtual void OnDisable()
    {
        this.StopListeningEvent<InteractEvent>();
        this.StopListeningEvent<InformationEvent>();
    }

    #endregion
}
