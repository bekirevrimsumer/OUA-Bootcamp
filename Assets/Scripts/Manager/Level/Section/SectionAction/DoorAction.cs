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
    private GameObject _door;

    private void Start()
    {
        _door = transform.gameObject;
    }

    public override void Execute()
    {
        base.Execute();
        _door.transform.DOLocalRotate(new Vector3(0, Rotation, 0), 4f).SetEase(Ease.InOutQuad);
    }
}
