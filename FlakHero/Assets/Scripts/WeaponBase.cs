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
    protected WeaponType weaponType; // 무기종류

    [SerializeField]
    protected WeaponSetting weaponSetting; // 무기 설정

    protected float lastAttackTime = 0; // 마지막 발사시간 체크용
    protected bool isReload = false; // 재장전 중인지 체크
    protected bool isAttack = false; // 공격 여부 체크용
    protected AudioSource audioSource; // 사운드 재생 컴포넌트
    protected PlayerAnimatorController animator; // 애니메이션 재생 제어

    // 외부에서 이벤트 함 수 등록을 할 수 있도록 public 선언

    // ※※※※※※※※※※※※※※※※※※※※※ Event Instance ※※※※※※※※※※※※※※※※※※※※※
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();

    [HideInInspector]
    public MagazineEvent magazineEvent = new MagazineEvent();

    [HideInInspector]
    public OverHeatEvent onOverHeatEvent = new OverHeatEvent();

    // ※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※

    // overHat 관리

    public float heatingValue = 0;

    public int OverHeatMaxCount = 100;

    public bool IsOnOverHeat = false;

    public float DisableWeaponTime = 3f;


    // 외부에서 필요한 정보를 열람하기 위해 정의한 Get Property's
    public PlayerAnimatorController Animator => animator;
    public WeaponName WeaponName => weaponSetting.weaponName;
    public int CurrnetMagazine => weaponSetting.currentMagazine;
    public int MaxMagaine => weaponSetting.maxMagazine;

    // 모든 무기마다 행동이 다르기 때문에 무기마다 메소드를 정의할 수 있도록 추상메소드로 선언한다.
    public abstract void StartWeaponAction(int type = 0);
    public abstract void StopWeaponAction(int type = 0);
    public abstract void StartReload();

    protected void PlaySound(AudioClip clip)
    {
        audioSource.Stop(); // 기존에 재생중인 사운드를 정지하고,
        audioSource.clip = clip; // 새로운 사운드 clip으로 교체 후
        audioSource.Play(); // 사운드 재생
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