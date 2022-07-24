using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    private ParticleSystem  particle;
    private MemoryPool      memoryPool;

    //private void Start()
    //{
    //    Destroy(gameObject, 3f);
    //}

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Ÿ������Ʈ�� �������� �ʰ� �޸�Ǯ�� ������ �Ѵ�.
    public void Setup(MemoryPool pool)
    {
        memoryPool = pool;
    }

    private void Update()
    {
        // ��ƼŬ�� ������� �ƴϸ� ����
        if ( particle.isPlaying == false)
        {
            memoryPool.DeactivatePoolItem(gameObject);
        }
    }
}
