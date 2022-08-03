using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlakPool : MonoBehaviour
{
    public static FlakPool Instance;
    public GameObject FlakPrefab;
    public int initActivationCount;

    private Queue<Flak> Q = new Queue<Flak>();

    private void Awake()
    {
        Instance = this;
        Initilize(initActivationCount);
    }

    private Flak CreateNewObject()
    {
        var newObj = Instantiate(FlakPrefab, transform).GetComponent<Flak>();
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

    public static Flak GetObject()
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

    public static void ReturnObject(Flak obj)
    {
        obj.gameObject.SetActive(false);
        Instance.Q.Enqueue(obj);
    }
}
