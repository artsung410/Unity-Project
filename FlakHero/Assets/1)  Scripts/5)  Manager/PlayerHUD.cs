using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public WeaponBase weapon;           

    [Header("Components")]
    [SerializeField]
    private Status status;            

    [Header("Weapon Base")]
    public TextMeshProUGUI textWeaponName;    
    public Image imageWeaponIcon;    
    public Image[] WeaponIcons;  
    [SerializeField]
    private Vector2[] sizeWeaponIcons;   

    [Header("HP & BloodScreen UI")]
    [SerializeField]
    private TextMeshProUGUI textHP;             
    [SerializeField]
    private Image imageBloodScreen;   
    [SerializeField]
    private AnimationCurve curveBloodScreen;

    [SerializeField]
    private Image hpBar;

    // overHeat°ü·Ã
    [SerializeField]
    private Image overheatBar;

    [SerializeField]
    private GameObject exceptionMark;

    Animator weaponAnimator;

    private void Awake()
    {
        status.onHPEvent.AddListener(UpdateHPHUD);
        weapon.onOverHeatEvent.AddListener(UpdateOverHeatHUD);
        weaponAnimator = WeaponIcons[0].GetComponent<Animator>();
    }

    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        weapon = newWeapon;

        SetupWeapon();
    }

    private void SetupWeapon()
    {
        textWeaponName.text = weapon.WeaponName.ToString();

        imageWeaponIcon.gameObject.SetActive(false);
        
        imageWeaponIcon = WeaponIcons[(int)weapon.WeaponName];
        imageWeaponIcon.rectTransform.sizeDelta = sizeWeaponIcons[(int)weapon.WeaponName];

        imageWeaponIcon.gameObject.SetActive(true);
        weaponAnimator = imageWeaponIcon.GetComponent<Animator>();
    }

    void UpdateHPHUD(int previous, int current)
    {
        hpBar.fillAmount = current / 100f;
        textHP.text = string.Format("                                       HP {0} / 100", current);

        if (previous <= current) return;

        if (previous - current > 0)
        {
            StopCoroutine("OnBloodScreen");
            StartCoroutine("OnBloodScreen");
        }
    }

    void UpdateOverHeatHUD(float HeatCount)
    {
        if (imageWeaponIcon == WeaponIcons[0])
        {
            overheatBar.fillAmount = HeatCount / 100f;
        }
        else
        {
            overheatBar.fillAmount = 0;
        }

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
