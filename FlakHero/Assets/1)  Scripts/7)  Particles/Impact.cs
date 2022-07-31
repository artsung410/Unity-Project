using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    private ParticleSystem  particle;
    private ImpactPool      Pool;

    //private void Start()
    //{
    //    Destroy(gameObject, 3f);
    //}

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // 타격이펙트는 삭제하지 않고 메모리풀로 관리를 한다.
    public void Setup(ImpactPool pool)
    {
        Pool = pool;
    }

    private void Update()
    {
        // 파티클이 재생중이 아니면 삭제
        if ( particle.isPlaying == false)
        {
            ImpactPool.ReturnObject(this);
        }
    }
}
