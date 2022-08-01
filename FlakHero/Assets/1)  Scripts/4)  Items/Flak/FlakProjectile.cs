﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlakProjectile : MonoBehaviour
{
    [SerializeField]
    private float Speed = 20f; // ºñÇà ½ºÇÇµå

    [SerializeField]
    int bulletDamage = 50;

    void Update()
    {
        Vector3 targetPos = GameManager.Instance.RealTargetPos;
        Speed += Mathf.Pow(Time.deltaTime * 2f, 3);

        if (targetPos != null)
        {
            Vector3 to = targetPos;
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
            gameObject.SetActive(false);
        }
    }
}