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

    protected bool isExplode = false;


    // �߻�޼ҵ�
    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0 && isExplode == false)
        {
            StartCoroutine("ExplodeAircraft");
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

        Destroy(gameObject);
    }
}
