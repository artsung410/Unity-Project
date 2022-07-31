using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlakProjectilePool : MonoBehaviour
{
    public static FlakProjectilePool Instance;
    public GameObject FlakProjectilePrefab;

    private Queue<FlakProjectile> Q = new Queue<FlakProjectile>();

    private void Awake()
    {
        Instance = this; // 객체 자기자신을 가리킴
        Initilize(10);
    }

    private FlakProjectile CreateNewObject() // 새로운 총알을 만드렁내는 역할
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
        // 빌려줄 오브젝트가 있을때 
        if (Instance.Q.Count > 0)
        {
            var obj = Instance.Q.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }

        // 없을때
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    public static void ReturnObject(FlakProjectile obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.Q.Enqueue(obj);
    }
}
