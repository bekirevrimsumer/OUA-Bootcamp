using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractEventType { MirrorEnter, MirrorExit, MirrorCarry, MirrorDrop, ClimbEnter, ClimbExit, InteractableObjectEnter, Interact, InteractEnd, InteractableObjectExit }

public struct InteractEvent : IEventType
{
    public InteractEventType InteractEventType;
    public string PanelName;
    public bool IsAnimatePanel;
    public bool IsUpdatePanel;
    public bool IsOpenDefaultPanel;
    public InteractItemBaseSO InteractItemBaseSO;

    public InteractEvent(InteractEventType interactEventType, string panelName, bool isAnimatePanel, bool isUpdatePanel, bool isOpenDefaultPanel, InteractItemBaseSO interactItemBaseSO)
    {
        InteractEventType = interactEventType;
        PanelName = panelName;
        IsAnimatePanel = isAnimatePanel;
        IsUpdatePanel = isUpdatePanel;
        IsOpenDefaultPanel = isOpenDefaultPanel;
        InteractItemBaseSO = interactItemBaseSO;
    }

    static InteractEvent e;

    public static void Trigger(InteractEventType interactEventType, string panelName = null, bool isAnimatePanel = false, bool isUpdatePanel = false, bool IsOpenDefaultPanel = true, InteractItemBaseSO interactItemBaseSO = null)
    {
        e.InteractEventType = interactEventType;
        e.PanelName = panelName;
        e.IsAnimatePanel = isAnimatePanel;
        e.IsUpdatePanel = isUpdatePanel;
        e.InteractItemBaseSO = interactItemBaseSO;
        e.IsOpenDefaultPanel = IsOpenDefaultPanel;
        EventManager.TriggerEvent(e);
    }
}
