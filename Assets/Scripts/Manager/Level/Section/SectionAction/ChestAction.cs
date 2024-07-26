using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChestAction : BaseSectionAction
{
    public int Rotation;
    public GameObject ObjectInChest;

    public override void Execute()
    {
        base.Execute();
        
        ObjectInChest.SetActive(true);
        transform.DOLocalRotate(new Vector3(Rotation, 0, 0), 1f).SetEase(Ease.InOutQuad);
        SoundEvent.Trigger(SoundType.SFX, "OpenChest", 0.5f, 0, false);
    }
}
