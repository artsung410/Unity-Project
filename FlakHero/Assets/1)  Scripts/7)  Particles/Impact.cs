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

    // Ÿ������Ʈ�� �������� �ʰ� �޸�Ǯ�� ������ �Ѵ�.
    public void Setup(ImpactPool pool)
    {
        Pool = pool;
    }

    private void Update()
    {
        // ��ƼŬ�� ������� �ƴϸ� ����
        if ( particle.isPlaying == false)
        {
            ImpactPool.ReturnObject(this);
        }
    }
}
