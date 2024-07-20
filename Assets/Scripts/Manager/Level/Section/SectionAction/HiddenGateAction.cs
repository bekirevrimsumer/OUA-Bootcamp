using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HiddenGateAction : BaseSectionAction
{
    public Vector3 GatePosition;

    public override void Execute()
    {
        base.Execute();
        transform.DOLocalMove(GatePosition, 1f).SetEase(Ease.InOutQuad);
    }
}
