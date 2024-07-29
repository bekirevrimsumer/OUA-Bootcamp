public class MirrorAction : BaseSectionAction
{
    private Mirror _mirror;

    private void Start() 
    {
        _mirror = transform.GetComponent<Mirror>();    
    }

    public override void Execute()
    {
        base.Execute();
        _mirror.IsInteractable = false;
        transform.parent = null;
    }
}
