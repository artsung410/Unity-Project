using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactPool : MonoBehaviour
{
    public static ImpactPool Instance;

    public GameObject ImpactPrefab;

    private Queue<Impact> Q = new Queue<Impact>();

    private void Awake()
    {
        Instance = this; // ��ü �ڱ��ڽ��� ����Ŵ
        Initilize(10);
    }

    private Impact CreateNewObject() // ���ο� �Ѿ��� ���巷���� ����
    {
        var newObj = Instantiate(ImpactPrefab, transform).GetComponent<Impact>();
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

    public static Impact GetObject()
    {
        // ������ ������Ʈ�� ������ 
        if (Instance.Q.Count > 0)
        {
            var obj = Instance.Q.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }

        // ������
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    public static void ReturnObject(Impact obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.Q.Enqueue(obj);
    }

    public void SpawnImpact(RaycastHit hit)
    {
        OnSpawnImpact(hit.point, Quaternion.LookRotation(hit.normal));
    }

    public void OnSpawnImpact(Vector3 position, Quaternion rotation)
    {
        Impact impact = ImpactPool.GetObject();
        impact.transform.position = position;
        impact.transform.rotation = rotation;
        impact.GetComponent<Impact>().Setup(Instance);

        ParticleSystem.MainModule main = impact.GetComponent<ParticleSystem>().main;

    }
}
