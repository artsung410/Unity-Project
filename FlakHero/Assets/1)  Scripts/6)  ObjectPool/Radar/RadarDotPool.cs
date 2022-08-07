using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarDotPool : MonoBehaviour
{
    public static RadarDotPool Instance;
    public GameObject RadarDotPrefab;
    public int initActivationCount;

    private Queue<RadarDot> Q = new Queue<RadarDot>();

    private void Awake()
    {
        Instance = this;
        Initilize(10);
    }

    private RadarDot CreateNewObject()
    {
        var newObj = Instantiate(RadarDotPrefab, GameObject.Find("Canvas").transform).GetComponent<RadarDot>();
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

    public static RadarDot GetObject()
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

    public static void ReturnObject(RadarDot obj)
    {
        obj.gameObject.SetActive(false);
        Instance.Q.Enqueue(obj);
    }
}
