using System.Diagnostics;
using TMPro;

public class PaperPanelUpdater : BasePanelUpdater<InteractEvent>
{
    public TextMeshProUGUI KeyText;

    public override void UpdatePanel(InteractEvent eventType)
    {
        if (eventType.InteractItemBaseSO == null)
            return;
            
        var paper = eventType.InteractItemBaseSO as PaperSO;

        switch (eventType.InteractEventType)
        {
            case InteractEventType.Interact:
                KeyText.text = "";
                foreach (var key in paper.CorrectCombination)
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
