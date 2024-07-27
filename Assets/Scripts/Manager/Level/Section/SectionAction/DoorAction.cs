using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Analytics;
using Unity.Services.Analytics;
using Unity.Services.Core;

[Serializable]
public class DoorAction : BaseSectionAction
{
    public float Rotation;
    public string sectionName;
    private GameObject _door;

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        _door = transform.gameObject;

        CustomEvent sectionCompletedEvent = new CustomEvent("sectionCompleted")
        {
            {"sectionName", "Section1" }
        };

        AnalyticsService.Instance.RecordEvent(sectionCompletedEvent);
    }

    public override void Execute()
    {
        base.Execute();
        _door.transform.DOLocalRotate(new Vector3(0, Rotation, 0), 4f).SetEase(Ease.InOutQuad);
        //AnalyticsResult analyticsResult = Analytics.CustomEvent("sectionCompleted");
        //Debug.Log("Result: " + analyticsResult);

        CustomEvent sectionCompletedEvent = new CustomEvent("sectionCompleted")
        {
            {"sectionName", sectionName }
        };

        AnalyticsService.Instance.RecordEvent(sectionCompletedEvent);
    }
}
