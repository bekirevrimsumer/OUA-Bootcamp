public class LockControlCondition : BaseCondition
{
    public DoorLockKeySO DoorLockKeySO;
    private int[] _result;
    private bool _isOpened;

    private void Start()
    {
        _result = new int[]{0,0,0,0};
        _isOpened = false;
        Rotate.Rotated += CheckResults;
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

        if (_result[0] == DoorLockKeySO.CorrectCombination[0] && _result[1] == DoorLockKeySO.CorrectCombination[1]
            && _result[2] == DoorLockKeySO.CorrectCombination[2] && _result[3] == DoorLockKeySO.CorrectCombination[3] && !_isOpened)
        {
            _isOpened = true;
            SectionEvent.Trigger(SectionEventType.SectionCompleted);
        }
    }

    public override bool IsCompleted()
    {
        return _isOpened;
    }

    private void OnDestroy()
    {
        Rotate.Rotated -= CheckResults;
    }
}
