public class LightBeamReachedTargetCondition : BaseCondition
{
    public LightBeamTarget target;

    public override bool IsCompleted()
    {
        base.IsCompleted();
        return target.IsReached;
    }
}
