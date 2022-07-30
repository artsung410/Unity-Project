using UnityEngine;

public enum WeaponType { Main=0, Sub, Melee, Throw}


[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }

[System.Serializable]
public class MagazineEvent : UnityEngine.Events.UnityEvent<int> { }

[System.Serializable]
public class OverHeatEvent : UnityEngine.Events.UnityEvent<float> { }


public abstract class WeaponBase : MonoBehaviour
{
    [Header("WeaponBase")]
    [SerializeField]
    protected   WeaponType               weaponType;                // ��������

    [SerializeField]
    protected   WeaponSetting            weaponSetting;             // ���� ����
        
    protected   float                    lastAttackTime = 0;        // ������ �߻�ð� üũ��
    protected   bool                     isReload = false;          // ������ ������ üũ
    protected   bool                     isAttack = false;          // ���� ���� üũ��
    protected   AudioSource              audioSource;               // ���� ��� ������Ʈ
    protected   PlayerAnimatorController animator;                  // �ִϸ��̼� ��� ����

    //// Event Instance
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();

    [HideInInspector]
    public MagazineEvent magazineEvent = new MagazineEvent();

    [HideInInspector]
    public  OverHeatEvent   onOverHeatEvent = new OverHeatEvent();   // ������� �ý��� ������Ʈ�� ���� �̺�Ʈ �ν��Ͻ� ���� 

    // overHat ����
    [HideInInspector]
    public  float           heatingValue        = 0;                 // ������� ��
    [HideInInspector]
    public  int             OverHeatMaxCount    = 100;               // �ִ� ������� �ִ�ġ
    [HideInInspector]
    public  bool            IsOnOverHeat        = false;             // overHeat���� üũ

    public  float           DisableWeaponTime   = 3f;                // ����������϶� �������� �Ұ����� �ð�

    // Get Property's
    public      PlayerAnimatorController    Animator => animator;
    public      WeaponName                  WeaponName => weaponSetting.weaponName;
    public      int                         CurrnetMagazine => weaponSetting.currentMagazine;
    public      int                         MaxMagaine => weaponSetting.maxMagazine;


    public abstract void StartWeaponAction(int type = 0);
    public abstract void StopWeaponAction(int type = 0);
    public abstract void StartReload();

    protected void PlaySound(AudioClip clip)
    {
        audioSource.Stop();         // ������ ������� ���带 �����ϰ�,
        audioSource.clip = clip;    // ���ο� ���� clip���� ��ü ��
        audioSource.Play();         // ���� ���
    }

    protected void Setup()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<PlayerAnimatorController>();
    }

    public void AircoolingWeapon()
    {
        heatingValue -= 0.2f;
        heatingValue = Mathf.Clamp(heatingValue, 0, OverHeatMaxCount);
        onOverHeatEvent.Invoke(heatingValue);
    }

    public void HeatWeapon()
    {
        heatingValue += 0.5f;
        heatingValue = Mathf.Clamp(heatingValue, 0, OverHeatMaxCount);
        onOverHeatEvent.Invoke(heatingValue);
    }
}