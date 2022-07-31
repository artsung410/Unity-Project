using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemColider : MonoBehaviour
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

            StartCoroutine("OnAndDeactiveEmp");
        }
    }

    private IEnumerator OnAndDeactiveEmp()
    {
        yield return new WaitForSeconds(3f);

        empColider.isActiveEmp = false;
    }
}