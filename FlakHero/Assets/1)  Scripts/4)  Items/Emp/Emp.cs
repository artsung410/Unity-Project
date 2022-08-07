using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emp : MonoBehaviour
{
    public int Damage;
    private void Start()
    {
        StartCoroutine("DeactivateObect");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyA"))
        {
            OnExplosion(other);
            KamikazeAircraft obj = other.gameObject.GetComponent<KamikazeAircraft>();
            KamikazeAircraftPool.ReturnObject(obj);
            GameManager.Instance.AddScore();
        }

        if (other.CompareTag("EnemyB"))
        {
            OnExplosion(other);
            MissileAircraft obj = other.gameObject.GetComponent<MissileAircraft>();
            MissileAircraftPool.ReturnObject(obj);
            GameManager.Instance.AddScore();
        }

        if (other.CompareTag("Missile"))
        {
            OnExplosion(other);
            EnemyMissile obj = other.gameObject.GetComponent<EnemyMissile>();
            EnemyMissilePool.ReturnObject(obj);
            GameManager.Instance.AddScore();
        }
    }

    IEnumerator DeactivateObect()
    {
        yield return new WaitForSeconds(2f);
        EmpPool.ReturnObject(this);
    }

    void OnExplosion(Collider other)
    {
        var obj = EnemyExplosionPool.GetObject();
        obj.transform.position = other.gameObject.transform.position;
        obj.transform.rotation = other.gameObject.transform.rotation;
    }
}
