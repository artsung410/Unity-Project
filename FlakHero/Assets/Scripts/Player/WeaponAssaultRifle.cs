using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }

// ������ źâ�� ������ �ٲ𶧸��� �ܺο� �ִ� �޼ҵ带 �ڵ����� ȣ���Ҽ��ְ� �����.
[System.Serializable]
public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }


public class WeaponAssaultRifle : MonoBehaviour
{
    // �ءءءءءءءءءءءءءءءءءءءء� Event Instance �ءءءءءءءءءءءءءءءءءءءء�
    [HideInInspector]
    public AmmoEvent                    onAmmoEvent = new AmmoEvent();

    [HideInInspector]
    public MagazineEvent                onMagazineEvent = new MagazineEvent();

    // �ءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءء�

    [Header("Fire Effects")]
    public GameObject                   muzzleFlashEffect;      // �ѱ� ����Ʈ (On / Off)

    [Header("Spawn Points")]
    public Transform                    casingSpawnPoint;       // ź�� ���� ��ġ 
    public Transform                    bulletSpawnPoint;       // �Ѿ� ���� ��ġ

    [Header("Audio Clips")]
    public AudioClip                    audioClipTakeOutWeapon; // ���� ���� ����
    public AudioClip                    audioClipFire;          // ���� ����
    public AudioClip                    audioClipReload;        // ������ ����

    [Header("Weapon Setting")]
    public WeaponSetting                weaponSetting;          // ���� ����

    [Header("Aim UI")]
    public Image                        imageAim;               // default / aim ��忡 ���� Aim �̹��� Ȱ�� / ��Ȱ��

    private float                       lastAttackTime = 0;     // ������ �߻�ð� üũ��
    private bool                        isReload = false;       // ������ ������ üũ
    private bool                        isAttack = false;       // ���� ���� üũ��
    private bool                        isModeChange = false;   // ��� ��ȯ ���� üũ��
    private float                       defaultModeFOV = 60;    // �⺻��忡�� ī�޶� FOV
    private float                       aimModeFOV = 30;        // AIM��忡���� ī�޶� FOV



    private AudioSource                 audioSource;            // ���� ���������Ʈ
    private PlayerAnimatorController    animator;               // �ִϸ��̼� ��� ����
    private CasingMemoryPool            casingMemoryPool;       // ź�� ���� �� Ȱ�� / ��Ȱ�� ����
    private ImpactMemoryPool            impactMemoryPool;       // ���� ȿ�� ���� �� Ȱ�� / ��Ȱ�� ����
    private Camera                      mainCamera;             // ���� �߻�


    // �ءءءءءءءءء� �ܺο��� �ʿ��� ������ �����ϱ� ���� ������ Get Property' s �ءءءءءءءءء�
    public            WeaponName WeaponName => weaponSetting.weaponName;
    public int        CurrentMagazine => weaponSetting.currentMagazine;
    public int        MaxMagazine => weaponSetting.maxMagazine;

    // �ءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءءء�


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInParent<PlayerAnimatorController>();
        casingMemoryPool = GetComponent<CasingMemoryPool>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        mainCamera = Camera.main;

        // ó�� źâ ���� �ִ�� ����
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;

        // ó�� ź ���� �ִ�� ����
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }

    private void OnEnable()
    {
        // ���� ���� ���� ���
        PlaySound(audioClipTakeOutWeapon);

        // �ѱ� ����Ʈ ������Ʈ ��Ȱ��ȭ
        muzzleFlashEffect.SetActive(false);

        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ źâ ������ �����Ѵ�
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);

        // ���Ⱑ Ȱ��ȭ�� �� �ش� ������ ź �� ������ �����Ѵ�.
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

        ResetVariables();
    }

    public void StartWeaponAction(int type = 0)
    {
        // ������ ���� ���� ���� �׼��� �� �� ����.
        if (isReload == true) return;

        // ��� ��ȯ���̸� ���� �׼��� �� �� ����.
        if (isModeChange == true) return;

        // ���콺 ���� Ŭ�� (���� ����)
        if (type == 0)
        {
            if (weaponSetting.isAutomaticAttack == true)
            {
                // ���� ����
                isAttack = true;
                StartCoroutine("OnAttackLoop");
            }

            else
            {
                // �ܹ� ����
                OnAttack();
            }
        }

        // ���콺 ������ Ŭ�� (��� ��ȯ)
        else
        {
            // ���� ���� ���� ��� ��ȯ�� �� �� ����.
            if (isAttack == true) return;

            StartCoroutine("OnModeChange");
        }    
    }

    public void StopWeaponAction(int type = 0)
    {
        // ���콺 ���� Ŭ�� (���� ����)
        if (type == 0)
        {
            isAttack = false;
            StopCoroutine("OnAttackLoop");
        }
    }

    public void StartReload()
    {
        // ���� ������ ���̰ų� źâ ���� 0�̸� ������ �Ұ���
        if (isReload == true || weaponSetting.currentMagazine <= 0) return;

        // ���� �׼� ���߿� 'R' Ű�� ���� �������� �õ��ϸ� ���� �׼� ���� �� ������
        StopWeaponAction();

        StartCoroutine("OnReload");
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
        if (Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            // �ٰ����� ���� ������ �� ����.
            if (animator.MoveSpeed > 0.5f)
            {
                return;
            }

            // �����ֱⰡ �Ǿ�� ���� �� �� �ֵ��� �ϱ� ���� ���� �ð� ����
            lastAttackTime = Time.time;

            // ź ���� ������ ���� �Ұ���
            if (weaponSetting.currentAmmo <= 0)
            {
                return;
            }

            // ���ݽ� currentAmmo 1 ����, ź �� UI ������Ʈ
            weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            // ���� �ִϸ��̼� ���
            string animation = animator.AimModeIs == true ? "AimFire" : "Fire";
            animator.Play(animation, -1, 0);

            // �ѱ� ����Ʈ ��� ( default mode �� ���� ��� )
            if (animator.AimModeIs == false)
            {
                StartCoroutine("OnMuzzleFlashEffect");
            }

            // ���� ���� ���
            PlaySound(audioClipFire);

            // ź�� ����
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right);

            // ������ �߻��� ���ϴ� ��ġ ���� (+Impact Effect)
            TwoStepRaycast();
        }
    }

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        // ȭ���� �߾� ��ǥ (Aim �������� Raycast ����)
        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);

        // ���� ��Ÿ� (attackDistance) �ȿ� �ε����� ������Ʈ�� ������ targetPoint�� ������ �ε��� ��ġ
        if (Physics.Raycast(ray, out hit, weaponSetting.attackDistance))
        {
            targetPoint = hit.point;
        }

        // ���� ��Ÿ� �ȿ� �ε����� ������Ʈ�� ������ targetPoint�� �ִ� ��Ÿ� ��ġ
        else
        {
            targetPoint = ray.origin + ray.direction * weaponSetting.attackDistance;
        }

        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.attackDistance, Color.red);

        // ù��° Raycast�������� ����� targetPoint�� ��ǥ�������� �����ϰ�,
        // �ѱ��� ������������ �Ͽ� Raycast ����
        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;

        if ( Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.attackDistance))
        {
            impactMemoryPool.SpawnImpact(hit);

            if (hit.transform.CompareTag(GameManager.Instance.Enemy))
            {
                hit.transform.GetComponent<EnemyAircraft>().TakeDamage(weaponSetting.damage);
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

        while ( percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            // mode�� ���� ī�޶��� �þ߰��� ����
            mainCamera.fieldOfView = Mathf.Lerp(start, end, percent);

            yield return null;
        }

        isModeChange = false;
    }

    // ������ ���ݼӵ����� ������ muzzleFlashEffect�� ��� Ȱ��ȭ �� �� ��Ȱ��ȭ �Ѵ�.
    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);

        muzzleFlashEffect.SetActive(false);
    }

    private IEnumerator OnReload()
    {
        isReload = true;

        // ������ �ִϸ��̼�, ���� ���
        animator.OnRelaod();
        PlaySound(audioClipReload);

        while(true)
        {
            // ���尡 ������� �ƴϰ�, ���� �ִϸ��̼��� Movement�̸�
            // ������ �ִϸ��̼�(, ����) ����� ����Ǿ����� ��
            if (audioSource.isPlaying == false && animator.CurrentAnimationIs("Movement"))
            {
                isReload = false;

                // ���� źâ ���� 1 ���ҽ�Ű��, �ٲ� źâ ������ Text UI�� ������Ʈ
                weaponSetting.currentMagazine--;
                onMagazineEvent.Invoke(weaponSetting.currentMagazine);

                // ���� ź ���� �ִ�� �����ϰ�, �ٲ� ź �� ������ Text UI�� ������Ʈ
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

                yield break; 
            }

            yield return null;
        }

    }

    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
        isModeChange = false;
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

}