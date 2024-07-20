using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCondition : BaseCondition
{
    private DoorButton Button;

    private void Start()
    {
        Button = GetComponent<DoorButton>();
    }

    public override bool IsCompleted()
    {
        return Button.IsPressed;
    }
}
