using DG.Tweening;
using UnityEngine;

public class SwitchAction : BaseSectionAction
{
    public float Rotation;
    
    public override void Execute()
    {
        transform.DOLocalRotate(new Vector3(Rotation, 0, 0), 1f).SetEase(Ease.InOutQuad);
    }
}
