using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAircraft : MonoBehaviour
{
    [Header("EnemyAircraft")]
    public      float           explosionRadius = 10.0f;
    public      float           explosionForce = 1000.0f;
    public      bool            isRandomItemDrop;
    private     int             RandomDropIndex;

    public abstract void TakeDamage(int damage);

    protected void ExplodeAircraft()
    {
        OnExplosion();
    }

    protected void ItemAirDrop()
    {
        if (isRandomItemDrop == true)
        {
            RandomDropIndex = Random.Range(1, 4);

            if (RandomDropIndex == 1)
            {
                ItemInit();
            }
        }

        else
        {
            ItemInit();
        }
    }

    void ItemInit()
    {
        SupplyBox item = SupplyBoxPool.GetObject();
        item.gameObject.transform.position = transform.position;
        item.gameObject.transform.rotation = Quaternion.identity;
    }

    void OnExplosion()
    {
        var obj = EnemyExplosionPool.GetObject();
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
    }
}
