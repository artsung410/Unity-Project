using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    [Header("Missile")]
    public  GameObject  target;                             
    public  float       flightSpeed         = 10f;          
    public  float       explosionDelayTime  = 0.1f;
    public  float       searchTime          = 2f;
    public  float       explosionRadius     = 10.0f;
    public  float       explosionForce      = 1000.0f;
    public  bool        IsSreachedPlayer;                   

    private void Start()
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

    public IEnumerator ExplodeMissile()
    {
        yield return new WaitForSeconds(explosionDelayTime);

        OnExplosion();

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
        EnemyMissilePool.ReturnObject(this);
    }

    private IEnumerator NotFoundTarget()
    {
        yield return new WaitForSeconds(searchTime);

        OnExplosion();

        if (gameObject != null)
        {
            EnemyMissilePool.ReturnObject(this);
        }
    }

    void OnExplosion()
    {
        Bounds bounds = GetComponent<Collider>().bounds;
        var obj = ProjectileExplosionPool.GetObject();
        obj.transform.position = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
        obj.transform.rotation = transform.rotation;
    }
}