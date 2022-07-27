using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderGroundCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DropBox"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
