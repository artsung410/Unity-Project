using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public WeaponBase weapon;             // ���� ������ ��µǴ� ����

    [Header("Components")]
    [SerializeField]
    private Status status;             // �÷��̾��� ���� (�̵��ӵ�, ü��)

    [Header("Weapon Base")]
    public TextMeshProUGUI textWeaponName;     // ���� �̸�
    public Image imageWeaponIcon;    // ���� ������
    public Sprite[] spriteWeaponIcons;  // ���� �����ܿ� ���Ǵ� sprite �迭
    [SerializeField]
    private Vector2[] sizeWeaponIcons;    // ���� �������� UI ũ�� �迭

    [Header("HP & BloodScreen UI")]
    [SerializeField]
    private TextMeshProUGUI textHP;             // �÷��̾��� ü���� ����ϴ� Text
    [SerializeField]
    private Image imageBloodScreen;   // �÷��̾ ���ݹ޾��� �� ȭ�鿡 ǥ�õǴ� Image
    [SerializeField]
    private AnimationCurve curveBloodScreen;

    [SerializeField]
    private Image hpBar;


    // overHeat����
    [SerializeField]
    private Image overheatBar;

    [SerializeField]
    private GameObject exceptionMark;

    private void Awake()
    {
        status.onHPEvent.AddListener(UpdateHPHUD);
        weapon.onOverHeatEvent.AddListener(UpdateOverHeatHUD);
    }

    public void SetupAllWeapons(WeaponBase[] weapons)
    {
        //SetupMagazine();

        // ��� ������ ��� ������ �̺�Ʈ ���

        //for (int i = 0; i < weapons.Length; ++i)
        //{
        //    weapons[i].onAmmoEvent.AddListener(UpdateAmmoHUD);
        //    weapon[i].onMagazineEvent.AddListener(UpdateMagazineHUD);
        //}
    }

    // �ؽ�Ʈ ���� ���ӿ� ���� �̸��� ����ϰ� �̹��� ���� �����ܿ� ���� �̹����� ����Ѵ�.
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

        // ü���� �������� ���� ȭ�鿡 ������ �̹����� ������� �ʵ��� return
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
        if (weapon.IsOnOverHeat)
        {
            exceptionMark.SetActive(true);
        }
        else
        {
            exceptionMark.SetActive(false);
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
