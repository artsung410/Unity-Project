using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    private ParticleSystem  particle;
    private ImpactPool      Pool;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void Setup(ImpactPool pool)
    {
        Pool = pool;
    }

    private void Update()
    {
        if ( particle.isPlaying == false)
        {
            ImpactPool.ReturnObject(this);
        }
    }
}
