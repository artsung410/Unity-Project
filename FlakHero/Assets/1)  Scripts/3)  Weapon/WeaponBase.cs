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
    protected   WeaponType               weaponType;                // 무기종류

    [SerializeField]
    protected   WeaponSetting            weaponSetting;             // 무기 설정
        
    protected   float                    lastAttackTime = 0;        // 마지막 발사시간 체크용
    protected   bool                     isReload = false;          // 재장전 중인지 체크
    protected   bool                     isAttack = false;          // 공격 여부 체크용
    protected   AudioSource              audioSource;               // 사운드 재생 컴포넌트
    protected   PlayerAnimatorController animator;                  // 애니메이션 재생 제어

    //// Event Instance
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();

    [HideInInspector]
    public MagazineEvent magazineEvent = new MagazineEvent();

    [HideInInspector]
    public  OverHeatEvent   onOverHeatEvent = new OverHeatEvent();   // 무기과열 시스템 업데이트를 위한 이벤트 인스턴스 생성 

    // overHat 관리
    [HideInInspector]
    public  float           heatingValue;                 // 무기과열 값
    [HideInInspector]
    public  int             OverHeatMaxCount;               // 최대 무기과열 최대치
    [HideInInspector]
    public  bool            IsOnOverHeat;             // overHeat인지 체크

    public  float           DisableWeaponTime;                // 무기과열중일때 무기사용이 불가능한 시간

    // Get Property's
    public      PlayerAnimatorController    Animator => animator;
    public      WeaponName                  WeaponName => weaponSetting.weaponName;
    public      int                         CurrnetMagazine => weaponSetting.currentMagazine;
    public      int                         MaxMagaine => weaponSetting.maxMagazine;


    public abstract void StartWeaponAction(int type = 0);
    public abstract void StopWeaponAction(int type = 0);

    protected void PlaySound(AudioClip clip)
    {
        audioSource.Stop();         // 기존에 재생중인 사운드를 정지하고,
        audioSource.clip = clip;    // 새로운 사운드 clip으로 교체 후
        audioSource.Play();         // 사운드 재생
    }

    protected void Setup()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<PlayerAnimatorController>();
    }

    public void AircoolingWeapon()
    {
        heatingValue -= 50f * Time.deltaTime;
        heatingValue = Mathf.Clamp(heatingValue, 0, OverHeatMaxCount);
        onOverHeatEvent.Invoke(heatingValue);
    }

    public void HeatWeapon()
    {
        heatingValue += 100f * Time.deltaTime;
        heatingValue = Mathf.Clamp(heatingValue, 0, OverHeatMaxCount);
        onOverHeatEvent.Invoke(heatingValue);
    }
}