using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnBoxPool : MonoBehaviour
{
    public static EnemySpawnBoxPool Instance;
    public GameObject EnemySpawnBoxPrefab;
    public int initActivationCount;

    private Queue<EnemySpawnBox> Q = new Queue<EnemySpawnBox>();

    private void Awake()
    {
        Instance = this;
        Initilize(initActivationCount);
    }

    private EnemySpawnBox CreateNewObject()
    {
        var newObj = Instantiate(EnemySpawnBoxPrefab, transform).GetComponent<EnemySpawnBox>();
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

    public static EnemySpawnBox GetObject()
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

    public static void ReturnObject(EnemySpawnBox obj)
    {
        obj.gameObject.SetActive(false);
        Instance.Q.Enqueue(obj);
    }
}
