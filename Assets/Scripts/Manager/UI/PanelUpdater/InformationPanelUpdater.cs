using TMPro;
using UnityEngine;

public class InformationPanelUpdater : BasePanelUpdater<InformationEvent>
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Message;

    public override void UpdatePanel(InformationEvent eventType)
    {
        if (eventType.InfoMessageSO == null)
            return;
            
        Title.text = eventType.InfoMessageSO.Title;
        Message.text = eventType.InfoMessageSO.Message;
    }
}
