public class LightBeamReachedTargetCondition : BaseCondition
{
    public LightBeamTarget target;

    public override bool IsCompleted()
    {
        return target.IsReached;
    }
}
