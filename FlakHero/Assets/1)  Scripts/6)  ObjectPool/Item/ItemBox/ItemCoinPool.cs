using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCoinPool : MonoBehaviour
{
    public static ItemCoinPool Instance;
    public GameObject ItemCoinPrefab;
    public int initActivationCount;

    private Queue<ItemCoin> Q = new Queue<ItemCoin>();

    private void Awake()
    {
        Instance = this; 
        Initilize(initActivationCount);
    }

    private ItemCoin CreateNewObject() 
    {
        var newObj = Instantiate(ItemCoinPrefab, transform).GetComponent<ItemCoin>();
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

    public static ItemCoin GetObject()
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

    public static void ReturnObject(ItemCoin obj)
    {
        obj.gameObject.SetActive(false);
        Instance.Q.Enqueue(obj);
    }
}
