using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCondition : BaseCondition
{
    private HiddenButtonInteractable Button;

    private void Start()
    {
        Button = GetComponent<HiddenButtonInteractable>();
    }

    public override bool IsCompleted()
    {
        return Button.IsPressed;
    }
}
