public class LightBeamReachedTargetCondition : BaseCondition
{
    public LightBeamTarget target;
    public Section Section;

    public override bool IsCompleted()
    {
        base.IsCompleted();
        return target.IsReached;
    }
}
