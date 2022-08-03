using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
	public float deSpawnTime;
    
    private void Start()
    {
        Destroy(gameObject, 1f);
    }
}

