using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosion : MonoBehaviour
{
    public float deSpawnTime;

    private void Start()
    {
        StartCoroutine("DeactiveObject");
    }

    IEnumerator DeactiveObject()
    {
        yield return new WaitForSeconds(deSpawnTime);

        ProjectileExplosionPool.ReturnObject(this);
    }
}

