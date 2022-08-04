using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosionPool : MonoBehaviour
{
    public static ProjectileExplosionPool Instance;
    public GameObject ProjectileExplosionPrefab;
    public int initActivationCount;

    private Queue<ProjectileExplosion> Q = new Queue<ProjectileExplosion>();

    private void Awake()
    {
        Instance = this;
        Initilize(initActivationCount);
    }

    private ProjectileExplosion CreateNewObject()
    {
        var newObj = Instantiate(ProjectileExplosionPrefab, transform).GetComponent<ProjectileExplosion>();
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

    public static ProjectileExplosion GetObject()
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

    public static void ReturnObject(ProjectileExplosion obj)
    {
        obj.gameObject.SetActive(false);
        Instance.Q.Enqueue(obj);
    }
}
