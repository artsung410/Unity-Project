using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerCollider : MonoBehaviour
{

    EmpColider empColider;

    private void Awake()
    {
        empColider = FindObjectOfType<EmpColider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            other.GetComponent<ItemBase>().Use(transform.parent.gameObject);
        }

        if (other.CompareTag("Emp"))
        {
            other.GetComponent<ItemBase>().Use(transform.parent.gameObject);

            empColider.isActiveEmp = true;

            StartCoroutine("onEmp");
        }
    }

    private IEnumerator onEmp()
    {
        yield return new WaitForSeconds(3f);

        empColider.isActiveEmp = false;
    }
}
