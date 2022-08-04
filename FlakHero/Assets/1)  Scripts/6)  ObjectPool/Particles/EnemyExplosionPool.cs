using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionPool : MonoBehaviour
{
    public static EnemyExplosionPool Instance;
    public GameObject EnemyExplosionPrefab;
    public int initActivationCount;

    private Queue<EnemyExplosion> Q = new Queue<EnemyExplosion>();

    private void Awake()
    {
        Instance = this;
        Initilize(initActivationCount);
    }

    private EnemyExplosion CreateNewObject()
    {
        var newObj = Instantiate(EnemyExplosionPrefab, transform).GetComponent<EnemyExplosion>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    private void Initilize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Q.Enqueue(CreateNewObject());
        }
    }

    public static EnemyExplosion GetObject()
    {
        if (Instance.Q.Count > 0)
        {
            var obj = Instance.Q.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }

        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    public static void ReturnObject(EnemyExplosion obj)
    {
        obj.gameObject.SetActive(false);
        Instance.Q.Enqueue(obj);
    }
}
