using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IEventListener<InteractEvent>, IEventListener<InformationEvent>, IEventListener<DialogueEvent>
{
    private GameObject _currentWindow;

    public List<KeyValuePair> windowList = new List<KeyValuePair>();

    private void Update() 
    {
        if (SceneManager.GetActiveScene().name == "Game" && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseWindow("SettingsPanel");
            CloseWindow("GameplayPanel");
            ToggleWindow("PausePanel");
        }    
    }

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

    public void ClickEvent()
    {
        SoundEvent.Trigger(SoundType.SFX, "UI_Click", 0);
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

    public void QuitGame()
    {
        Application.Quit();
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
            {
                CloseWindow("InteractMirrorWindow");
                OpenWindow("CarryMirrorWindow");
            }
                break;
            case InteractEventType.MirrorDrop:
                CloseWindow("CarryMirrorWindow");
                OpenWindow("InteractMirrorWindow");
                break;
            case InteractEventType.ClimbEnter:
                AnimatePanel("ClimbWindow", true);
                break;
            case InteractEventType.ClimbExit:
                AnimatePanel("ClimbWindow", false);
                break;
            case InteractEventType.Interact:
                if(eventType.IsOpenDefaultPanel)
                {
                    CloseWindow("InteractPanel");
                    OpenWindow("BackPanel");
                }

                if(!string.IsNullOrEmpty(eventType.PanelName))
                {
                    if(eventType.IsUpdatePanel)
                        UpdatePanel(eventType.PanelName, eventType);
                    if(eventType.IsAnimatePanel)
                        AnimatePanel(eventType.PanelName, true);
                    else
                        OpenWindow(eventType.PanelName);
                }
                break;
            case InteractEventType.InteractEnd:
                if(eventType.IsOpenDefaultPanel)
                {
                    CloseWindow("BackPanel");
                    OpenWindow("InteractPanel");
                }

                if(!string.IsNullOrEmpty(eventType.PanelName))
                {
                    if(eventType.IsAnimatePanel)
                        AnimatePanel(eventType.PanelName, false);
                    else
                        CloseWindow(eventType.PanelName);
                }
                break;
            case InteractEventType.InteractableObjectEnter:
                if(string.IsNullOrEmpty(eventType.PanelName))
                {
                    AnimatePanel("InteractPanel", true);
                }
                else
                {
                    AnimatePanel(eventType.PanelName, true);
                }
                break;
            case InteractEventType.InteractableObjectExit:
                if(string.IsNullOrEmpty(eventType.PanelName))
                {
                    AnimatePanel("InteractPanel", false);
                }
                else
                {
                    AnimatePanel(eventType.PanelName, false);
                }
                break;
        }
    }

    public void OnEvent(InformationEvent eventType)
    {
        switch (eventType.InformationEventType)
        {
            case InformationEventType.Show:
                AnimatePanel("InfoPanel", true);
                UpdatePanel("InfoPanel", eventType);
                break;
            case InformationEventType.Hide:
                AnimatePanel("InfoPanel", false);
                break;
        }
    }

    public void OnEvent(DialogueEvent eventType)
    {
        switch (eventType.DialogueEventType)
        {
            case DialogueEventType.StartDialogue:
                AnimatePanel("DialoguePanel", true);
                UpdatePanel("DialoguePanel", eventType);
                break;
            case DialogueEventType.EndDialogue:
                AnimatePanel("DialoguePanel", false);
                break;
        }
    }

    protected virtual void OnEnable()
    {
        this.StartListeningEvent<InteractEvent>();
        this.StartListeningEvent<InformationEvent>();
        this.StartListeningEvent<DialogueEvent>();
    }

    protected virtual void OnDisable()
    {
        this.StopListeningEvent<InteractEvent>();
        this.StopListeningEvent<InformationEvent>();
        this.StopListeningEvent<DialogueEvent>();
    }

    #endregion
}


[System.Serializable]
public class KeyValuePair
{
    public string key;
    public GameObject value;
}