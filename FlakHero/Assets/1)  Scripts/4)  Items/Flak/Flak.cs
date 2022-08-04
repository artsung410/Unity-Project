using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flak : MonoBehaviour
{
    public GameObject FlakInitEffect;
    public GameObject FlakHead;
    public GameObject FlakHeadChildren;
    public GameObject target;
    public Transform BulletSpawnPoint;
    private AudioSource audioSource;
    public AudioClip audioClipInitFlak;  
    public AudioClip audioClipProjectile; 

    int[] dx = { 10, -10, -10, 10 };
    int[] dz = { 10, 10, -10, -10 };

    public int randomPos;
    public float SpawnCoolTime = 2f;
    public float ProjectileLifeTime = 1f;
    private bool isReadyToLaunch = false;


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
            bool[] IsExistTurretsOnMap = GameManager.Instance.IsFlakOnWorlds;

            if (false == IsExistTurretsOnMap[randomPos])
            {
                transform.position = new Vector3(dx[randomPos], 1, dz[randomPos]);

                GameObject particle = Instantiate(FlakInitEffect, transform.position, Quaternion.identity);
                Destroy(particle.gameObject, 1f);

                GameManager.Instance.IsFlakOnWorlds[randomPos] = true;
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

        } while (GameManager.Instance.IsFlakOnWorlds[randomPos]);
    }

    private void Update()
    {
        if (target != null)
        {
            GameManager.Instance.RealTargetPos = target.transform.position;

            Vector3 to = target.transform.position;
            Vector3 from = FlakHead.transform.position;
            Vector3 dir = to - from;
            Vector3 newdir = new Vector3(dir.x, dir.y + 10, dir.z);

            FlakHead.transform.rotation = Quaternion.LookRotation(newdir);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyA") || other.CompareTag("EnemyB"))
        {
            target = other.gameObject;

            isReadyToLaunch = true;
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
                StartCoroutine("SetAnimation");

                FlakProjectile projectile = FlakProjectilePool.GetObject();
                projectile.transform.position = BulletSpawnPoint.position;
                projectile.transform.rotation = BulletSpawnPoint.rotation;
                StartCoroutine(DeActiveProjectile(projectile));
            }
        }
    }

    IEnumerator DeActiveProjectile(FlakProjectile projectile)
    {
        yield return new WaitForSeconds(ProjectileLifeTime);
        FlakProjectilePool.ReturnObject(projectile);
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    IEnumerator SetAnimation()
    {
        Animator animator = FlakHeadChildren.GetComponent<Animator>();
        animator.SetBool("FireOn", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("FireOn", false);
    }
}