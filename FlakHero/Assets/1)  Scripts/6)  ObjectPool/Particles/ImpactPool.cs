using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactPool : MonoBehaviour
{
    public static ImpactPool Instance;
    public GameObject ImpactPrefab;
    public int initActivationCount;

    private Queue<Impact> Q = new Queue<Impact>();

    private void Awake()
    {
        Instance = this; 
        Initilize(initActivationCount);
    }

    private Impact CreateNewObject()
    {
        var newObj = Instantiate(ImpactPrefab, transform).GetComponent<Impact>();
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

    public static Impact GetObject()
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

    public static void ReturnObject(Impact obj)
    {
        obj.gameObject.SetActive(false);
        Instance.Q.Enqueue(obj);
    }

    public static void SpawnImpact(RaycastHit hit)
    {
        OnSpawnImpact(hit.point, Quaternion.LookRotation(hit.normal));
    }

    public static void OnSpawnImpact(Vector3 position, Quaternion rotation)
    {
        Impact impact = ImpactPool.GetObject();
        impact.transform.position = position;
        impact.transform.rotation = rotation;
        impact.GetComponent<Impact>().Setup(Instance);
    }
}

