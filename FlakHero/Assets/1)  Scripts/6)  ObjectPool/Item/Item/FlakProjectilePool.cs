using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlakProjectilePool : MonoBehaviour
{
    public static FlakProjectilePool Instance;
    public GameObject FlakProjectilePrefab;
    public int initActivationCount;

    private Queue<FlakProjectile> Q = new Queue<FlakProjectile>();

    private void Awake()
    {
        Instance = this;
        Initilize(initActivationCount);
    }

    private FlakProjectile CreateNewObject() 
    {
        var newObj = Instantiate(FlakProjectilePrefab, transform).GetComponent<FlakProjectile>();
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

    public static FlakProjectile GetObject()
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

    public static void ReturnObject(FlakProjectile obj)
    {
        obj.gameObject.SetActive(false);
        Instance.Q.Enqueue(obj);
    }
}
