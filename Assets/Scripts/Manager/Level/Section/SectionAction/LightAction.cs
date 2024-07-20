using System;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class LightAction : BaseSectionAction
{
    public float Intensity;
    private Light _light;

    private void Start() 
    {
        _light = transform.GetComponent<Light>();
    }


    public override void Execute()
    {
        base.Execute();
        DOTween.To(() => _light.intensity, x => _light.intensity = x, Intensity, 4);
    }
}
