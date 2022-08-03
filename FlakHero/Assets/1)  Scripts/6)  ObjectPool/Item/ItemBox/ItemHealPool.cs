using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealPool : MonoBehaviour
{
    public static ItemHealPool Instance;
    public GameObject ItemHealPrefab;
    public int initActivationCount;

    private Queue<ItemHeal> Q = new Queue<ItemHeal>();

    private void Awake()
    {
        Instance = this;
        Initilize(initActivationCount);
    }

    private ItemHeal CreateNewObject()
    {
        var newObj = Instantiate(ItemHealPrefab, transform).GetComponent<ItemHeal>();
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

    public static ItemHeal GetObject()
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

    public static void ReturnObject(ItemHeal obj)
    {
        obj.gameObject.SetActive(false);
        Instance.Q.Enqueue(obj);
    }
}
