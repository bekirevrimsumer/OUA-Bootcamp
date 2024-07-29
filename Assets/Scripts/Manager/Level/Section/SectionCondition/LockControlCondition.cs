public class LockControlCondition : BaseCondition
{
    public PaperSO PaperSO;
    private int[] _result;
    private bool _isOpened;

    private void Start()
    {
        _result = new int[]{0,0,0,0};
        _isOpened = false;
        PadlockRotate.Rotated += CheckResults;
    }

    private void CheckResults(string wheelName, int number)
    {
        switch (wheelName)
        {
            case "WheelOne":
                _result[0] = number;
                break;

            case "WheelTwo":
                _result[1] = number;
                break;

            case "WheelThree":
                _result[2] = number;
                break;

            case "WheelFour":
                _result[3] = number;
                break;
        }

        if (_result[0] == PaperSO.CorrectCombination[0] && _result[1] == PaperSO.CorrectCombination[1]
            && _result[2] == PaperSO.CorrectCombination[2] && _result[3] == PaperSO.CorrectCombination[3] && !_isOpened)
        {
            _isOpened = true;
            SectionEvent.Trigger(SectionEventType.SectionCompleted);
        }
    }

    public override bool IsCompleted()
    {
        base.IsCompleted();
        return _isOpened;
    }

    private void OnDestroy()
    {
        PadlockRotate.Rotated -= CheckResults;
    }
}
