using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyBox : MonoBehaviour
{
    // �ڽ�Ŭ������ ����Ҽ��ֵ��� protected�� ����
    [Header("EnemyAircraft")]
    [SerializeField]
    protected int maxHP = 100; // �ִ�ü��

    [SerializeField]
    protected int currentHP; // ����ü��

    [SerializeField]
    protected GameObject explosionPrefab;

    protected bool isExplode = false;

    private void Awake()
    {
        currentHP = maxHP; 
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0 && isExplode == false && transform.position.y < 0.5)
        {
            StartCoroutine("Explode");
        }
    }

    private IEnumerator Explode()
    {
        // ��ó�� �����Ⱑ ������ �ٽ� ��Ʈ������ �� ��(StackOverflow ����)
        isExplode = true;

        //// ���� ����Ʈ ����
        //Bounds bounds = GetComponent<Collider>().bounds;
        //Instantiate(explosionPrefab, new Vector3(bounds.center.x, bounds.min.y, bounds.center.z), transform.rotation);
        gameObject.SetActive(false);

        yield return null;
    }
}
