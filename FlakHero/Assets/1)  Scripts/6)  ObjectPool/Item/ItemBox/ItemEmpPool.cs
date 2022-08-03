using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEmpPool : MonoBehaviour
{
    public static ItemEmpPool Instance;
    public GameObject ItemEmpPrefab;
    public int initActivationCount;

    private Queue<ItemEmp> Q = new Queue<ItemEmp>();

    private void Awake()
    {
        Instance = this;
        Initilize(initActivationCount);
    }

    private ItemEmp CreateNewObject()
    {
        var newObj = Instantiate(ItemEmpPrefab, transform).GetComponent<ItemEmp>();
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

    public static ItemEmp GetObject()
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

    public static void ReturnObject(ItemEmp obj)
    {
        obj.gameObject.SetActive(false);
        Instance.Q.Enqueue(obj);
    }
}
