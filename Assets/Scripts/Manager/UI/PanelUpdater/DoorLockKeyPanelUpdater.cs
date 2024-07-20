using System.Diagnostics;
using TMPro;

public class DoorLockKeyPanelUpdater : BasePanelUpdater<InteractEvent>
{
    public TextMeshProUGUI KeyText;

    public override void UpdatePanel(InteractEvent eventType)
    {
        if (eventType.InteractItemBaseSO == null)
            return;
            
        var doorLockKeySO = eventType.InteractItemBaseSO as DoorLockKeySO;

        switch (eventType.InteractEventType)
        {
            case InteractEventType.Interact:
                KeyText.text = "";
                foreach (var key in doorLockKeySO.CorrectCombination)
                {
                    KeyText.text += key.ToString() + " ";
                }
                break;
            case InteractEventType.InteractEnd:
                KeyText.text = "";
                break;
        }
    }
}
