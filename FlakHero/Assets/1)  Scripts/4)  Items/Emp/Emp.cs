using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emp : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAircraft targetEnemy = other.gameObject.GetComponent<EnemyAircraft>();
            StartCoroutine(targetEnemy.ExplodeAircraft());
            GameManager.Instance.AddScore();
        }

        if (other.CompareTag("Missile"))
        {
            EnemyMissile targetMissile = other.gameObject.GetComponent<EnemyMissile>();
            StartCoroutine(targetMissile.ExplodeMissile());
            GameManager.Instance.AddScore();
        }
    }
}
