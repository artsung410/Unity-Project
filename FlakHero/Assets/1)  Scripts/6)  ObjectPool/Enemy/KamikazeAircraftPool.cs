using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeAircraftPool : MonoBehaviour
{
    public static KamikazeAircraftPool Instance;
    public GameObject KamikazeAircraftPrefab;

    private Queue<KamikazeAircraft> Q = new Queue<KamikazeAircraft>();

    private void Awake()
    {
        Instance = this; // ��ü �ڱ��ڽ��� ����Ŵ
        Initilize(10);
    }

    private KamikazeAircraft CreateNewObject() // ���ο� �Ѿ��� ���巷���� ����
    {
        var newObj = Instantiate(KamikazeAircraftPrefab, transform).GetComponent<KamikazeAircraft>();
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

    public static KamikazeAircraft GetObject()
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

    public static void ReturnObject(KamikazeAircraft obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.Q.Enqueue(obj);
    }
}
