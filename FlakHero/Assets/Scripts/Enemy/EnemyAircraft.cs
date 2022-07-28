using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAircraft : MonoBehaviour
{
    // �ڽ�Ŭ������ ����Ҽ��ֵ��� protected�� ����
    [Header("EnemyAircraft")]

    [SerializeField]
    protected GameObject explosionPrefab;

    [SerializeField]
    protected float explosionRadius = 10.0f;

    [SerializeField]
    protected float explosionForce = 1000.0f;

    [SerializeField]
    private GameObject SupplyBoxPrefab;

    protected bool isExplode = false;

    private int RandomDropIndex;

    public abstract void TakeDamage(int damage);

    protected IEnumerator ExplodeAircraft()
    {
        // ��ó�� �����Ⱑ ������ �ٽ� ���� �����⸦ ��Ʈ������ �� ��(StackOverflow ����)
        isExplode = true;

        // ���� ����Ʈ ����
        Bounds bounds = GetComponent<Collider>().bounds;
        Instantiate(explosionPrefab, new Vector3(bounds.center.x, bounds.min.y, bounds.center.z), transform.rotation);

        gameObject.SetActive(false);

        yield return null;
    }

    protected void ItemAirDrop()
    {
        // 33% Ȯ���� ���

        RandomDropIndex = Random.Range(1, 3);

        if (RandomDropIndex == 1)
        {
            GameObject item = Instantiate(SupplyBoxPrefab, transform.position, SupplyBoxPrefab.transform.rotation);
            item.SetActive(true);
        }
    }
}
