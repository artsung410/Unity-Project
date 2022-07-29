using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurret : MonoBehaviour
{
    // 1, 2, 3, 4 분면
    int[] dx = { 10, -10, -10, 10 };
    int[] dz = { 10, 10, -10, -10 };

    public int randomPos;

    private bool isReadyToLaunch = false;

    [SerializeField]
    private float SpawnCoolTime = 2f;

    [SerializeField]
    private float LifeTime = 1f;

    [SerializeField]
    private GameObject ProjectilePrefab;

    [SerializeField]
    private GameObject FlakInitEffect;

    [SerializeField]
    private Transform BulletSpawnPoint;

    [SerializeField]
    private GameObject FlakHead;

    public GameObject target;


    private MemoryPool bulletMemoryPool;


    private void Awake()
    {
        bulletMemoryPool = new MemoryPool(ProjectilePrefab);

    }

    private void Start()
    {
        StartCoroutine("BulletSpawn");

        do
        {
            randomPos = Random.Range(0, 4);
            bool[] IsExistTurretsOnMap = GameManager.Instance.IsAutoTurretOnWorld;

            if (false == IsExistTurretsOnMap[randomPos])
            {
                transform.position = new Vector3(dx[randomPos], 1, dz[randomPos]);

                // 생성 되었을 때 이펙트 효과
                GameObject particle = Instantiate(FlakInitEffect, transform.position, Quaternion.identity);
                Destroy(particle.gameObject, 1f);

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
        if (target != null)
        {
            Vector3 to = target.transform.position;
            Vector3 from = FlakHead.transform.position;
            Vector3 dir = to - from;

            FlakHead.transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            target = other.gameObject;
            isReadyToLaunch = true;

            if (other.gameObject.activeSelf == false)
            {
                isReadyToLaunch = false;
                StopCoroutine("BulletSpawn");
            }
        }
    }

    IEnumerator BulletSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(SpawnCoolTime);

            if (isReadyToLaunch && target.activeSelf != false)
            {
                GameObject bullet = bulletMemoryPool.ActivatePoolItem();
                bullet.transform.position = BulletSpawnPoint.position;
                bullet.transform.rotation = BulletSpawnPoint.rotation;
                StartCoroutine(DeActiveBullet(bullet));
            }
        }
    }

    IEnumerator DeActiveBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(LifeTime);
        bulletMemoryPool.DeactivatePoolItem(bullet);
    }
}
