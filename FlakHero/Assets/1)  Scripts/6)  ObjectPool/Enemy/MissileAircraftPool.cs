using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileAircraftPool : MonoBehaviour
{
    public static MissileAircraftPool Instance;
    public GameObject MissileAircraftPrefab;

    private Queue<MissileAircraft> Q = new Queue<MissileAircraft>();

    private void Awake()
    {
        Instance = this; // 객체 자기자신을 가리킴
        Initilize(10);
    }

    private MissileAircraft CreateNewObject() // 새로운 총알을 만드렁내는 역할
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

    public static void ReturnObject(MissileAircraft obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.Q.Enqueue(obj);
    }
}
