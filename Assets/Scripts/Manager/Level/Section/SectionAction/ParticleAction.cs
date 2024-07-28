using System;
using UnityEngine;

public class ParticleAction : BaseSectionAction
{
    private ParticleSystem _particle;

    private void Start() 
    {
        _particle = transform.GetComponent<ParticleSystem>();    
    }

    public override void Execute()
    {
        base.Execute();
        _particle.Play();
    }
}
