using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpColider : MonoBehaviour
{
    public bool isActiveEmp = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && isActiveEmp == true)
        {
            StartCoroutine(other.GetComponent<EnemyAircraft>().ExplodeAircraft());

            GameManager.Instance.AddScore();
        }
    }
}
