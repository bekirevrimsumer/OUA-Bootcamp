using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SpikeAction : BaseSectionAction
{
    [HideInInspector]
    public Spike Spike;

    private void Start() 
    {
        Spike = GetComponent<Spike>();    
    }

    public override void Execute()
    {
        base.Execute();
        Spike.StopSpike();
    }
}
