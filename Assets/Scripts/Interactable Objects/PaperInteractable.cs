public class PaperInteractable : Interactable
{
    public PaperSO PaperSO;

    public override void Interact()
    {
        if(!IsInteracting)
        {
            InteractEvent.Trigger(InteractEventType.Interact, "DoorLockKeyWindow", false, true, true, PaperSO);
            SoundEvent.Trigger(SoundType.SFX, "PaperOpen", 0, false);
            IsInteracting = true;
        }
        else
        {
            InteractEvent.Trigger(InteractEventType.InteractEnd, "DoorLockKeyWindow", false, false, true, PaperSO);
            SoundEvent.Trigger(SoundType.SFX, "PaperClose", 0, false);
            IsInteracting = false;
        }
    }
}
