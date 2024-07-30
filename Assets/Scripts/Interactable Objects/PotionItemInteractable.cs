using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PotionItemInteractable : Interactable
{
    public override void Interact()
    {
        base.Interact();
        IsInteracting = true;
        CurrentPlayer.Animator.SetTrigger("Interact");
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad);
        SectionEvent.Trigger(SectionEventType.SectionCompleted);
    }
}
