using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderGroundCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropBox"))
        {
            SupplyBox obj = other.GetComponent<SupplyBox>();
            SupplyBoxPool.ReturnObject(obj);
        }

        if (other.CompareTag("ImpactNormal"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
