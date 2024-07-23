public class PaperInteractable : Interactable
{
    public PaperSO PaperSO;

    public override void Interact()
    {
        if(!IsInteracting)
        {
            InteractEvent.Trigger(InteractEventType.Interact, "DoorLockKeyWindow", false, true, true, PaperSO);
            IsInteracting = true;
        }
        else
        {
            InteractEvent.Trigger(InteractEventType.InteractEnd, "DoorLockKeyWindow", false, false, true, PaperSO);
            IsInteracting = false;
        }
    }
}
