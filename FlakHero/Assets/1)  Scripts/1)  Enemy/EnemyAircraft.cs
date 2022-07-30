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
        // 근처의 전투기가 터져서 다시 현재 전투기를 터트리려고 할 때(StackOverflow 방지)
        isExplode = true;

        // 폭발 이펙트 생성
        Bounds bounds = GetComponent<Collider>().bounds;
        Instantiate(explosionPrefab, new Vector3(bounds.center.x, bounds.min.y, bounds.center.z), transform.rotation);

        //// 폭발 범위에 있는 모든 오브젝트의 collider 정보를 받아와 폭발 효과 처리
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            // 폭발 범위에 부딪힌 오브젝트가 플레이어일 때 처리
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
        // 33% 확률로 드랍
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
