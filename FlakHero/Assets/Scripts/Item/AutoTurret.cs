using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurret : MonoBehaviour
{
    // 1, 2, 3, 4 ºÐ¸é
    int[] dx = { 20, -20, -20, 20 };
    int[] dz = { 20, 20, -20, -20 };

    public int randomPos;

    private bool isReadyToLaunch = false;

    [SerializeField]
    private float SpawnCoolTime = 0.5f;

    [SerializeField]
    private GameObject ProjectilePrefab;

    [SerializeField]
    private Transform ProjectileSpawnPoint;

    public GameObject target;

    private void Start()
    {
        do
        {
            randomPos = Random.Range(0, 4);
            bool[] IsExistTurretsOnMap = GameManager.Instance.IsAutoTurretOnWorld;

            if (false == IsExistTurretsOnMap[randomPos])
            {
                transform.position = new Vector3(dx[randomPos], 1, dz[randomPos]);
                GameManager.Instance.IsAutoTurretOnWorld[randomPos] = true;
                break;
            }

            int TrueCount = 0;

            for (int turret = 0; turret < IsExistTurretsOnMap.Length; turret++)
            {
                if (IsExistTurretsOnMap[turret])
                {
                    TrueCount++;
                }

                if (TrueCount == 4)
                {
                    Destroy(gameObject);
                    return;
                }
            }

        } while (GameManager.Instance.IsAutoTurretOnWorld[randomPos]);
    }

    private void Update()
    {
        if (isReadyToLaunch)
        {
            Vector3 to = target.transform.position;
            Vector3 from = transform.position;
            Vector3 dir = to - from;

            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            target = other.gameObject;
            isReadyToLaunch = true;

            StartCoroutine("bulletLaunch");

            if (other.gameObject.activeSelf == false)
            {
                isReadyToLaunch = false;
                StopCoroutine("bulletLaunch");
            }
        }
    }

    IEnumerator bulletLaunch()
    {
        while (true)
        {
            if (isReadyToLaunch && target.activeSelf != false)
            {
                GameObject bullet = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, ProjectileSpawnPoint.rotation);
                bullet.SetActive(true);
            }

            yield return new WaitForSeconds(SpawnCoolTime);


        }
    }
}
