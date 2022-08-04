using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileAircraftPool : MonoBehaviour
{
    public static MissileAircraftPool Instance;
    public GameObject MissileAircraftPrefab;
    public int initActivationCount;

    private Queue<MissileAircraft> Q = new Queue<MissileAircraft>();

    private void Awake()
    {
        Instance = this;
        Initilize(10);
    }

    private MissileAircraft CreateNewObject()
    {
        var newObj = Instantiate(MissileAircraftPrefab, transform).GetComponent<MissileAircraft>();
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

    public static MissileAircraft GetObject()
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

    public static void ReturnObject(MissileAircraft obj)
    {
        obj.gameObject.SetActive(false);
        Instance.Q.Enqueue(obj);
    }
}
