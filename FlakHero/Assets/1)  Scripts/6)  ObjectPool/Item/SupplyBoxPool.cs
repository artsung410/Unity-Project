using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyBoxPool : MonoBehaviour
{
    public static SupplyBoxPool Instance;
    public GameObject SupplyBoxPrefab;
    public int initActivationCount;

    private Queue<SupplyBox> Q = new Queue<SupplyBox>();

    private void Awake()
    {
        Instance = this; 
        Initilize(initActivationCount);
    }

    private SupplyBox CreateNewObject() 
    {
        var newObj = Instantiate(SupplyBoxPrefab, transform).GetComponent<SupplyBox>();
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

    public static SupplyBox GetObject()
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

    public static void ReturnObject(SupplyBox obj)
    {
        obj.gameObject.SetActive(false);
        Instance.Q.Enqueue(obj);
    }
}
