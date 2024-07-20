using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractEventType { MirrorEnter, MirrorExit, MirrorCarry, MirrorDrop, ClimbEnter, ClimbExit, InteractableObjectEnter, Interact, InteractEnd, InteractableObjectExit }

public struct InteractEvent : IEventType
{
    public InteractEventType InteractEventType;
    public string PanelName;
    public bool IsAnimatePanel;
    public bool IsOpen;
    public bool IsUpdatePanel;
    public InteractItemBaseSO InteractItemBaseSO;

    public InteractEvent(InteractEventType interactEventType, string panelName, bool isAnimatePanel, bool isOpen, bool isUpdatePanel, InteractItemBaseSO interactItemBaseSO)
    {
        InteractEventType = interactEventType;
        PanelName = panelName;
        IsAnimatePanel = isAnimatePanel;
        IsOpen = isOpen;
        IsUpdatePanel = isUpdatePanel;
        InteractItemBaseSO = interactItemBaseSO;
    }

    static InteractEvent e;

    public static void Trigger(InteractEventType interactEventType, string panelName = null, bool isAnimatePanel = false, bool isOpen = true, bool isUpdatePanel = false, InteractItemBaseSO interactItemBaseSO = null)
    {
        e.InteractEventType = interactEventType;
        e.PanelName = panelName;
        e.IsAnimatePanel = isAnimatePanel;
        e.IsOpen = isOpen;
        e.IsUpdatePanel = isUpdatePanel;
        e.InteractItemBaseSO = interactItemBaseSO;
        EventManager.TriggerEvent(e);
    }
}
