using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAircraft : MonoBehaviour
{
    // �ڽ�Ŭ������ ����Ҽ��ֵ��� protected�� ����
    [Header("EnemyAircraft")]
    [SerializeField]
    protected int maxHP = 100; // �ִ�ü��

    [SerializeField]
    protected int currentHP; // ����ü��

    [SerializeField]
    protected GameObject explosionPrefab;

    [SerializeField]
    protected float explosionDelayTime = 0.1f;

    [SerializeField]
    protected float explosionRadius = 10.0f;

    [SerializeField]
    protected float explosionForce = 1000.0f;

    [SerializeField]
    private GameObject SupplyBoxPrefab;

    protected bool isExplode = false;

    private int RandomDropIndex;

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0 && isExplode == false)
        {
            StartCoroutine("ExplodeAircraft");

            RandomDropIndex = Random.Range(1, 3);
            //Debug.Log(RandomDropIndex);

            if (RandomDropIndex == 1)
            {
                GameObject item = Instantiate(SupplyBoxPrefab, transform.position, SupplyBoxPrefab.transform.rotation);
                item.SetActive(true);
            }
        }
    }

    private IEnumerator ExplodeAircraft()
    {
        yield return new WaitForSeconds(explosionDelayTime);

        // ��ó�� �����Ⱑ ������ �ٽ� ���� �����⸦ ��Ʈ������ �� ��(StackOverflow ����)
        isExplode = true;

        // ���� ����Ʈ ����
        Bounds bounds = GetComponent<Collider>().bounds;
        Instantiate(explosionPrefab, new Vector3(bounds.center.x, bounds.min.y, bounds.center.z), transform.rotation);

        //// ���� ������ �ִ� ��� ������Ʈ�� collider ������ �޾ƿ� ���� ȿ�� ó��
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            // ���� ������ �ε��� ������Ʈ�� �÷��̾��� �� ó��
            PlayerController player = hit.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(5);
                continue;
            }
        }

        gameObject.SetActive(false);
    }
}
