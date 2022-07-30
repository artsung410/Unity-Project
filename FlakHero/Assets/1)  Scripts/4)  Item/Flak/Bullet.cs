using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float Speed = 20f; // ���� ���ǵ�

    [SerializeField]
    int bulletDamage = 50;

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * Speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyAircraft>().TakeDamage(bulletDamage);
        }
    }
}
