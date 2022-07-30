using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAircraft : MonoBehaviour
{
    [Header("EnemyAircraft")]
    public      GameObject      explosionPrefab;
    public      GameObject      SupplyBoxPrefab;
    public      float           explosionRadius = 10.0f;
    public      float           explosionForce = 1000.0f;
    public      bool            isRandomItemDrop;
    private     int             RandomDropIndex;
    protected   bool            isExplode = false;

    public abstract void TakeDamage(int damage);

    public IEnumerator ExplodeAircraft()
    {
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

        yield return null;
    }

    protected void ItemAirDrop()
    {
        // 33% Ȯ���� ���
        if (isRandomItemDrop == true)
        {
            RandomDropIndex = Random.Range(1, 3);

            if (RandomDropIndex == 1)
            {
                ItemInit();
            }
        }
        else
        {
            ItemInit();
        }
    }

    void ItemInit()
    {
        GameObject item = Instantiate(SupplyBoxPrefab, transform.position, SupplyBoxPrefab.transform.rotation);
        item.SetActive(true);
    }
}
