using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class WeaponAssaultRifle : WeaponBase
{
    [Header("Fire Effects")]
    public      GameObject              muzzleFlashEffect;      // 총구 이펙트 (On / Off)

    [Header("Spawn Points")]
    public      Transform               casingSpawnPoint;       // "탄피" 생성 위치 
    public      Transform               bulletSpawnPoint;       // "총알" 생성 위치

    [Header("Audio Clips")]
    public      AudioClip               audioClipTakeOutWeapon; // 무기 "장착" 사운드
    public      AudioClip               audioClipFire;          // 무기 "공격" 사운드
    public      AudioClip               audioClipReload;        // 무기 "재장전" 사운드

    [Header("Aim UI")]
    public      Image                   imageAim;               // default / aim 모드에 따라 Aim 이미지 활성 / 비활성

    private     Camera                  mainCamera;             // 광선 발사

    private     bool                    isModeChange = false;   // 모드 전환 여부 체크용
    private     float                   defaultModeFOV = 60;    // 기본모드에서 카메라 FOV
    private     float                   aimModeFOV = 30;        // AIM모드에서의 카메라 FOV


    private void Awake()
    {
        base.Setup();
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        PlaySound(audioClipTakeOutWeapon);
        muzzleFlashEffect.SetActive(false);
        ResetVariables();
    }

    public override void StartWeaponAction(int type = 0)
    {
        // 모드 전환중이면 무기 액션을 할 수 없다.
        if (isModeChange == true) return;

        // 마우스 왼쪽 클릭 (공격 시작)
        if (type == 0)
        {
            if (weaponSetting.isAutomaticAttack == true)
            {
                // 연속 공격
                isAttack = true;
                StartCoroutine("OnAttackLoop");
            }

            else
            {
                // 단발 공격
                OnAttack();
            }
        }

        // 마우스 오른쪽 클릭 (모드 전환)
        else
        {
            // 공격 중일 때는 모드 전환을 할 수 없다.
            if (isAttack == true) return;

            StartCoroutine("OnModeChange");
        }
    }

    public override void StopWeaponAction(int type = 0)
    {
        // 마우스 왼쪽 클릭 (공격 종료)
        if (type == 0)
        {
            isAttack = false;
            StopCoroutine("OnAttackLoop");
        }
    }

    private IEnumerator OnAttackLoop()
    {
        while (true)
        {
            OnAttack();

            yield return null;
        }
    }

    public void OnAttack()
    {
        if (IsOnOverHeat == false)
        {
            HeatWeapon();
        }

        if (heatingValue > 99.9 && IsOnOverHeat == false)
        {
            IsOnOverHeat = true;
            StartCoroutine("DeactiveOverHeat");
        }

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
            string animation = animator.AimModeIs == true ? "AimFire" : "Fire";
            animator.Play(animation, -1, 0);

            // 총구 이펙트 재생 ( default mode 일 때만 재생 )
            if (animator.AimModeIs == false)
            {
                StartCoroutine("OnMuzzleFlashEffect");
            }

            // 공격 사운드 재생
            PlaySound(audioClipFire);

            // 탄피 생성
            CasingPool.SpawnCasing(casingSpawnPoint.position, transform.right);

            // 광선을 발사해 원하는 위치 공격 (+Impact Effect)
            TwoStepRaycast();
        }
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
            Debug.Log(hit.collider.tag);

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

    private IEnumerator OnModeChange()
    {
        float current = 0;
        float percent = 0;
        float time = 0.35f;

        animator.AimModeIs = !animator.AimModeIs;
        imageAim.enabled = !imageAim.enabled;

        float start = mainCamera.fieldOfView;
        float end = animator.AimModeIs == true ? aimModeFOV : defaultModeFOV;

        isModeChange = true;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            // mode에 따라 카메라의 시야각을 변경
            mainCamera.fieldOfView = Mathf.Lerp(start, end, percent);

            yield return null;
        }

        isModeChange = false;
    }

    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);

        muzzleFlashEffect.SetActive(false);
    }

    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
        isModeChange = false;
    }

    private IEnumerator DeactiveOverHeat()
    {
        yield return new WaitForSeconds(DisableWeaponTime);

        IsOnOverHeat = false;
    }

}