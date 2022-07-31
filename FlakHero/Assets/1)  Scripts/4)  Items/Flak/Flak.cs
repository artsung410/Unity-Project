using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flak : MonoBehaviour
{
    public  GameObject  FlakInitEffect;
    public  GameObject  FlakHead;
    public  GameObject  target;
    public  Transform   BulletSpawnPoint;
    private AudioSource audioSource;
    public  AudioClip   audioClipInitFlak;   // 포탑 생성 소리
    public  AudioClip   audioClipProjectile; // 포탄 발사 소리



    // 1, 2, 3, 4 분면
    int[] dx =      { 10, -10, -10, 10 };
    int[] dz =      { 10, 10, -10, -10 };

    public  int     randomPos;
    public  float   SpawnCoolTime = 2f;
    public  float   LifeTime = 1f;
    private bool    isReadyToLaunch = false;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    private void Start()
    {
        PlaySound(audioClipInitFlak);
        StartCoroutine("ProjectileSpawn");
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
            GameManager.Instance.realTimeTarget = target.transform;

            if (other.gameObject.activeSelf == false)
            {
                isReadyToLaunch = false;
                GameManager.Instance.realTimeTarget = null;
                StopCoroutine("ProjectileSpawn");
            }
        }
    }

    IEnumerator ProjectileSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(SpawnCoolTime);

            if (isReadyToLaunch && target.activeSelf != false)
            {
                PlaySound(audioClipProjectile);
                FlakProjectile projectile = FlakProjectilePool.GetObject();
                projectile.transform.position = BulletSpawnPoint.position;
                projectile.transform.rotation = BulletSpawnPoint.rotation;
                StartCoroutine(DeActiveProjectile(projectile));
            }
        }
    }

    IEnumerator DeActiveProjectile(FlakProjectile projectile)
    {
        yield return new WaitForSeconds(LifeTime);
        FlakProjectilePool.ReturnObject(projectile);
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();         
        audioSource.clip = clip;    
        audioSource.Play();         
    }
}
