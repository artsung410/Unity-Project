using System.Collections;
using UnityEngine;

public class WeaponRevolver : WeaponBase
{
    [Header("Fire Effects")]
    public GameObject muzzleFlashEffect;

    [Header("Spawn Points")]
    public Transform bulletSpawnPoint;

    [Header("Audio Clips")]
    public AudioClip audioClipFire;
    private Camera mainCamera;

    private void OnEnable()
    {
        muzzleFlashEffect.SetActive(false);

        ResetVariables();
    }

    private void Awake()
    {
        base.Setup();
        mainCamera = Camera.main;
    }

    public override void StartWeaponAction(int type = 0)
    {
        if (type == 0 && isAttack == false && isReload == false)
        {
            OnAttack();
        }

        base.Setup();

    }

    public override void StopWeaponAction(int type = 0)
    {
        isAttack = false;
    }

    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            // 뛰고있을 때는 공격할 수 없다.
            if (animator.MoveSpeed > 0.5f || IsOnOverHeat == true)
            {
                return;
            }

            // 공격주기가 되어야 공격 할 수 있도록 하기 위해 현재 시간 저장
            lastAttackTime = Time.time;


            // 무기 애니메이션 재생
            animator.Play("Fire", -1, 0);

            // 총구 이펙트 재생
            StartCoroutine("OnMuzzleFlashEffect");

            // 공격 사운드 재생
            PlaySound(audioClipFire);

            // 광선을 발사해 원하는 위치 공격
            TwoStepRaycast();
        }
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);

        muzzleFlashEffect.SetActive(false);
    }

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        // 화면의 중앙 좌표 (Aim 기준으로 Raycast 연산)
        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);

        // 공격 사거리 (attackDistance) 안에 부딪히는 오브젝트가 있으면 targetPoint는 광선에 부딪힌 위치
        if (Physics.Raycast(ray, out hit, weaponSetting.attackDistance))
        {
            targetPoint = hit.point;
        }

        // 공격 사거리 안에 부딪히는 오브젝트가 없으면 targetPoint는 최대 사거리 위치
        else
        {
            targetPoint = ray.origin + ray.direction * weaponSetting.attackDistance;
        }

        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.attackDistance, Color.red);

        // 첫번째 Raycast연산으로 얻어진 targetPoint를 목표지점으로 설정하고,
        // 총구를 시작지점으로 하여 Raycast 연산
        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;

        int layerMask = 1 << 6; // player layer

        if (Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.attackDistance, layerMask))
        {
            ImpactPool.SpawnImpact(hit);

            if (hit.transform.CompareTag("EnemyA") || hit.transform.CompareTag("EnemyB"))
            {
                hit.transform.GetComponent<EnemyAircraft>().TakeDamage(weaponSetting.damage);
            }

            else if (hit.transform.CompareTag("DropBox"))
            {
                hit.transform.GetComponent<SupplyBox>().Destruction();
            }
        }

        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.attackDistance, Color.blue);
    }

    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
    }

}
