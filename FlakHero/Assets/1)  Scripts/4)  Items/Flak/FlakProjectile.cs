using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlakProjectile : MonoBehaviour
{
    [SerializeField]
    private float Speed = 20f; // 비행 스피드

    [SerializeField]
    int bulletDamage = 50;

    void Update()
    {
        Transform target = GameManager.Instance.realTimeTarget;

        if (target != null)
        {
            Vector3 to = target.transform.position;
            Vector3 from = transform.position;
            Vector3 dir = to - from;

            transform.rotation = Quaternion.LookRotation(dir);
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyAircraft>().TakeDamage(bulletDamage);
        }
    }
}
