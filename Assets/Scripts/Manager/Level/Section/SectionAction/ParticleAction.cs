using System;
using UnityEngine;

[Serializable]
public class ParticleAction : BaseSectionAction
{
    private ParticleSystem _particle;

    private void Start() 
    {
        _particle = transform.GetComponent<ParticleSystem>();    
    }

    public override void Execute()
    {
        _particle.Play();
    }
}
