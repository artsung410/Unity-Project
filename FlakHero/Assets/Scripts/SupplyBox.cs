using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyBox : MonoBehaviour
{
    // 자식클래스에 사용할수있도록 protected로 선언
    [Header("EnemyAircraft")]
    [SerializeField]
    protected int maxHP = 100; // 최대체력

    [SerializeField]
    protected int currentHP; // 현재체력

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
        // 근처의 전투기가 터져서 다시 터트리려고 할 때(StackOverflow 방지)
        isExplode = true;

        //// 폭발 이펙트 생성
        //Bounds bounds = GetComponent<Collider>().bounds;
        //Instantiate(explosionPrefab, new Vector3(bounds.center.x, bounds.min.y, bounds.center.z), transform.rotation);
        gameObject.SetActive(false);

        yield return null;
    }
}
