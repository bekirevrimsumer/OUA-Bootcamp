using System.Diagnostics;

public class DoorLockKey : Interactable
{
    public DoorLockKeySO DoorLockKeySO;
    private bool _isOpen;

    public override void Interact()
    {
        if(!_isOpen)
        {
            InteractEvent.Trigger(InteractEventType.DoorLockKeyShow, DoorLockKeySO);
            _isOpen = true;
        }
        else
        {
            InteractEvent.Trigger(InteractEventType.DoorLockKeyHide);
            _isOpen = false;
        }
    }
}
