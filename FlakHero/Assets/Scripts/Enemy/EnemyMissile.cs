using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    [Header("Missile")]

    [SerializeField]
    private float flightSpeed = 10f; // 비행 스피드

    [SerializeField]
    private GameObject target; // 플레이어 게임오브젝트 가져오기

    [SerializeField]
    protected GameObject explosionPrefab;

    [SerializeField]
    protected float explosionDelayTime = 0.1f;

    [SerializeField]
    protected float searchTime = 2f;

    [SerializeField]
    protected float explosionRadius = 10.0f;

    [SerializeField]
    protected float explosionForce = 1000.0f;

    public bool IsSreachedPlayer; // 플레이어와 접촉 여부

    private float elapsedTime;
    private void Awake()
    {
        IsSreachedPlayer = false;
    }

    private void Update()
    {
        MoveAndSelfBoom();
    }

    void MoveAndSelfBoom()
    {
        Vector3 to = new Vector3(target.transform.position.x, target.transform.position.y - 4, target.transform.position.z);
        Vector3 from = transform.position;

        if (IsSreachedPlayer == false)
        {
            // 플레이어가 회피했을때 자동으로 폭발하도록 처리
            
            StartCoroutine("NotFoundTarget");

            transform.rotation = Quaternion.LookRotation(to - from);
            transform.Translate(Vector3.forward * Time.deltaTime * flightSpeed);

            if (gameObject == null)
            {
                StopCoroutine("NotFoundTarget");
            }
        }

        else
        {
            StartCoroutine("ExplodeMissile");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameManager.Instance.Player)
        {
            IsSreachedPlayer = true;

            Debug.Log("적이 플레이어와 접촉함");
        }
    }

    private IEnumerator ExplodeMissile()
    {
        yield return new WaitForSeconds(explosionDelayTime);

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
    }

    private IEnumerator NotFoundTarget()
    {
        yield return new WaitForSeconds(searchTime);

        // 폭발 이펙트 생성
        Bounds bounds = GetComponent<Collider>().bounds;
        Instantiate(explosionPrefab, new Vector3(bounds.center.x, bounds.min.y, bounds.center.z), transform.rotation);

        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
}