using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAircraft : MonoBehaviour
{
    // 자식클래스에 사용할수있도록 protected로 선언
    [Header("EnemyAircraft")]
    [SerializeField]
    protected int maxHP = 100; // 최대체력

    [SerializeField]
    protected int currentHP; // 현재체력

    [SerializeField]
    protected GameObject explosionPrefab;

    [SerializeField]
    protected float explosionDelayTime = 0.1f;

    [SerializeField]
    protected float explosionRadius = 10.0f;

    [SerializeField]
    protected float explosionForce = 1000.0f;

    protected bool isExplode = false;


    // 추상메소드
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

        Destroy(gameObject);
    }
}
