using Photon.Pun;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class BaseCondition : MonoBehaviourPunCallbacks
{
    public string sectionName;

    private async void Start() 
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection(); 

        CustomEvent sectionCompletedEvent = new CustomEvent("sectionCompleted")
        {
            {"sectionName", "Section1" }
        };

        AnalyticsService.Instance.RecordEvent(sectionCompletedEvent);
    }

    public virtual bool IsCompleted() 
    {
        CustomEvent sectionCompletedEvent = new CustomEvent("sectionCompleted")
        {
            {"sectionName", sectionName }
        };

        AnalyticsService.Instance.RecordEvent(sectionCompletedEvent);
        return false; 
    }
}
