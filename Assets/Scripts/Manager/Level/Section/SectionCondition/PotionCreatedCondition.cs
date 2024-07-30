using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCreatedCondition : BaseCondition
{
    public List<PotionItemInteractable> Potions = new List<PotionItemInteractable>();
    private bool _isCompleted = false;

    public override bool IsCompleted()
    {
        base.IsCompleted();
        foreach (PotionItemInteractable potion in Potions)
        {
            if (potion.IsInteracting)
            {
                _isCompleted = true;
            }
            else
            {
                _isCompleted = false;
            }
        }

        return _isCompleted;
    }
}
