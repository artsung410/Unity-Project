using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public WeaponBase weapon;             // 현재 정보가 출력되는 무기

    [Header("Components")]
    [SerializeField]
    private Status status;             // 플레이어의 상태 (이동속도, 체력)

    [Header("Weapon Base")]
    public TextMeshProUGUI textWeaponName;     // 무기 이름
    public Image imageWeaponIcon;    // 무기 아이콘
    public Sprite[] spriteWeaponIcons;  // 무기 아이콘에 사용되는 sprite 배열
    [SerializeField]
    private Vector2[] sizeWeaponIcons;    // 무기 아이콘의 UI 크기 배열

    [Header("HP & BloodScreen UI")]
    [SerializeField]
    private TextMeshProUGUI textHP;             // 플레이어의 체력을 출력하는 Text
    [SerializeField]
    private Image imageBloodScreen;   // 플레이어가 공격받았을 대 화면에 표시되는 Image
    [SerializeField]
    private AnimationCurve curveBloodScreen;

    [SerializeField]
    private Image hpBar;

    // overHeat관련
    [SerializeField]
    private Image overheatBar;

    [SerializeField]
    private GameObject exceptionMark;

    Animator weaponAnimator;

    private void Awake()
    {
        status.onHPEvent.AddListener(UpdateHPHUD);
        weapon.onOverHeatEvent.AddListener(UpdateOverHeatHUD);
        weaponAnimator = imageWeaponIcon.GetComponent<Animator>();
    }


    //public void SetupAllWeapons(WeaponBase[] weapons)
    //{
    //    //SetupMagazine();

    //    // 사용 가능한 모든 무기의 이벤트 등록

    //    //for (int i = 0; i < weapons.Length; ++i)
    //    //{
    //    //    weapons[i].onAmmoEvent.AddListener(UpdateAmmoHUD);
    //    //    weapon[i].onMagazineEvent.AddListener(UpdateMagazineHUD);
    //    //}
    //}

    // 텍스트 웨폰 네임에 무기 이름을 출력하고 이미지 웨폰 아이콘에 무기 이미지를 출력한다.
    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        weapon = newWeapon;

        SetupWeapon();
    }

    private void SetupWeapon()
    {
        textWeaponName.text = weapon.WeaponName.ToString();
        imageWeaponIcon.sprite = spriteWeaponIcons[(int)weapon.WeaponName];
        imageWeaponIcon.rectTransform.sizeDelta = sizeWeaponIcons[(int)weapon.WeaponName];
    }

    void UpdateHPHUD(int previous, int current)
    {
        hpBar.fillAmount = current / 100f;
        textHP.text = string.Format("                                       HP {0} / 100", current);

        // 체력이 증가했을 때는 화면에 빨간색 이미지를 출력하지 않도록 return
        if (previous <= current) return;

        if (previous - current > 0)
        {
            StopCoroutine("OnBloodScreen");
            StartCoroutine("OnBloodScreen");
        }
    }

    void UpdateOverHeatHUD(float HeatCount)
    {
        overheatBar.fillAmount = HeatCount / 100f;

        float fillValue = overheatBar.fillAmount;

        int colorValue = 255 - (int)((fillValue) * 255);
        overheatBar.color = new Color32(255, (byte)colorValue, (byte)colorValue, 170);

        if (HeatCount > 99.9)
        {
            exceptionMark.SetActive(true);
            weaponAnimator.SetBool("onOverHeat", true);

        }
        else
        {
            exceptionMark.SetActive(false);
            weaponAnimator.SetBool("onOverHeat", false);
        }
    }

    private IEnumerator OnBloodScreen()
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime;

            Color color = imageBloodScreen.color;
            color.a = Mathf.Lerp(1, 0, curveBloodScreen.Evaluate(percent));
            imageBloodScreen.color = color;

            yield return null;
        }
    }
}
