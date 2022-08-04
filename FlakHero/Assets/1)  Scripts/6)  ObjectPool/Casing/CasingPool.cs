using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasingPool : MonoBehaviour
{
    public static CasingPool Instance;
    public GameObject CasingPrefab;
    public int initActivationCount;

    private Queue<Casing> Q = new Queue<Casing>();

    private void Awake()
    {
        Instance = this; 
        Initilize(initActivationCount);
    }

    private Casing CreateNewObject()
    {
        var newObj = Instantiate(CasingPrefab, transform).GetComponent<Casing>();
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

    public static Casing GetObject()
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

    public static void ReturnObject(Casing obj)
    {
        obj.gameObject.SetActive(false);
        Instance.Q.Enqueue(obj);
    }

    public static void SpawnCasing(Vector3 position, Vector3 direction)
    {
        var obj = CasingPool.GetObject();
        obj.transform.position = position;
        obj.transform.rotation = Random.rotation;
        obj.GetComponent<Casing>().Setup(Instance, direction);
    }
}