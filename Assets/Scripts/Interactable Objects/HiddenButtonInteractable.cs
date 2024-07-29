using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HiddenButtonInteractable : Interactable
{
    [HideInInspector]
    public bool IsPressed;
    public GameObject Button;

    public override void Interact()
    {
        if(!IsInteracting && !IsPressed)
        {
            IsInteracting = true;
            IsPressed = true;
            CurrentPlayer.Animator.SetTrigger("Interact");
            Button.transform.DOLocalMoveZ(-0.05f, 0.5f).SetEase(Ease.InOutQuad).OnComplete(() => IsInteracting = false);
            SectionEvent.Trigger(SectionEventType.SectionCompleted);
        }
    }
}
