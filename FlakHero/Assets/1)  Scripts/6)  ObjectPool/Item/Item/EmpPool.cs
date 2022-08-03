using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpPool : MonoBehaviour
{
    public static EmpPool Instance;
    public GameObject EmpPrefab;
    public int initActivationCount;

    private Queue<Emp> Q = new Queue<Emp>();

    private void Awake()
    {
        Instance = this;
        Initilize(initActivationCount);
    }

    private Emp CreateNewObject()
    {
        var newObj = Instantiate(EmpPrefab, transform).GetComponent<Emp>();
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

    public static Emp GetObject()
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

    public static void ReturnObject(Emp obj)
    {
        obj.gameObject.SetActive(false);
        Instance.Q.Enqueue(obj);
    }
}
