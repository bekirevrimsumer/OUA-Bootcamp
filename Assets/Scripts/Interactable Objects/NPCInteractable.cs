using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Cinemachine.PostFX;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NPCInteractable : Interactable
{
    public DialogueSO dialogueSO;
    public bool IsCameraFocus = true;

    public override void Interact()
    {
        var framingTransposer = CurrentPlayer.Camera.GetCinemachineComponent<CinemachineFramingTransposer>();
        var volumeSettings = CurrentPlayer.Camera.GetComponent<CinemachineVolumeSettings>();

        if (!IsInteracting)
        {
            InteractEvent.Trigger(InteractEventType.Interact, null, false, false, true, null);

            IsInteracting = true;

            DialogueEvent.Trigger(DialogueEventType.StartDialogue, dialogueSO);

            if(IsCameraFocus)
            {
                CurrentPlayer.Camera.Follow = transform;
                CurrentPlayer.Camera.transform.DORotate(new Vector3(10, -110, 0), 1f);
                CurrentPlayer.IsCameraRotatingEnabled = false;
                
                DOTween.To(() => framingTransposer.m_CameraDistance, x => framingTransposer.m_CameraDistance = x, 6f, 1f);

                volumeSettings.m_Profile.TryGet<DepthOfField>(out var depthOfField);
                depthOfField.active = false;
            }
            else
            {
                InteractEvent.Trigger(InteractEventType.InteractEnd, null, false, false, true, null);
                IsInteracting = false;
            }
        }
        else if (IsInteracting)
        {
            InteractEvent.Trigger(InteractEventType.InteractEnd, null, false, false, true, null);
            IsInteracting = false;

            if(IsCameraFocus)
            {
                CurrentPlayer.Camera.Follow = CurrentPlayer.CameraFollowTransform;
                CurrentPlayer.IsCameraRotatingEnabled = true;
                CurrentPlayer.Camera.transform.DORotate(new Vector3(50, 0, 0), 1f);
                
                DOTween.To(() => framingTransposer.m_CameraDistance, x => framingTransposer.m_CameraDistance = x, 13f, 1f);
                DOTween.To(() => framingTransposer.m_TrackedObjectOffset.y, y => framingTransposer.m_TrackedObjectOffset.y = y, 1.2f, 1f);

                volumeSettings.m_Profile.TryGet<DepthOfField>(out var depthOfField);
                depthOfField.active = true;
            }

            DialogueEvent.Trigger(DialogueEventType.EndDialogue, null);
        }
    }
}
