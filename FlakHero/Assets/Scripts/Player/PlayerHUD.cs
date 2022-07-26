using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    public WeaponBase   weapon;             // ���� ������ ��µǴ� ����

    [Header("Components")]
    [SerializeField]
    private Status              status;             // �÷��̾��� ���� (�̵��ӵ�, ü��)

    [Header("Weapon Base")]
    public TextMeshProUGUI      textWeaponName;     // ���� �̸�
    public Image                imageWeaponIcon;    // ���� ������
    public Sprite[]             spriteWeaponIcons;  // ���� �����ܿ� ���Ǵ� sprite �迭
    [SerializeField]
    private Vector2[]           sizeWeaponIcons;    // ���� �������� UI ũ�� �迭

    //[Header("Ammo")]
    //public TextMeshProUGUI textAmmo;           // ���� / �ִ� ź �� ��� Text

    //[Header("Magazine")]
    //public GameObject magazineUIPrefab;             // źâ UI ������

    //public Transform            magazineParent;     // źâ UI�� ��ġ�Ǵ� panel

    //[SerializeField]
    //private int                 maxMagazineCount;   // ó�� �����ϴ� �ִ� źâ ��

    //private List<GameObject>    magazineList;       // źâ UI ����Ʈ

    [Header("HP & BloodScreen UI")]
    [SerializeField]
    private TextMeshProUGUI     textHP;             // �÷��̾��� ü���� ����ϴ� Text
    [SerializeField]
    private Image               imageBloodScreen;   // �÷��̾ ���ݹ޾��� �� ȭ�鿡 ǥ�õǴ� Image
    [SerializeField]
    private AnimationCurve      curveBloodScreen;

    private void Awake()
    {
        status.onHPEvent.AddListener(UpdateHPHUD);
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

    //private void UpdateAmmoHUD(int currentAmmo, int maxAmmo)
    //{
    //    textAmmo.text = $"<size=40>{currentAmmo} / </size>{maxAmmo}";
    //}

    //private void SetupMagazine()
    //{
    //    // weapon�� ��ϵǾ� �ִ� �ִ� źâ ������ŭ Image Icon�� ����
    //    // magazineParent ������Ʈ�� �ڽ����� ��� �� ��� ��Ȱ��ȭ/����Ʈ�� ����
    //    magazineList = new List<GameObject>();
    //    for (int i = 0; i < maxMagazineCount; ++i)
    //    {
    //        GameObject clone = Instantiate(magazineUIPrefab);
    //        clone.transform.SetParent(magazineParent);
    //        clone.SetActive(false);

    //        magazineList.Add(clone);
    //    }

    //    // weapon�� ��ϵǾ� �ִ� ���� źâ ������ŭ ������Ʈ Ȱ��ȭ
    //    for (int i = 0; i < weapon.CurrentMagazine; ++i)
    //    {
    //        magazineList[i].SetActive(true);
    //    }
    //}

    //private void UpdateMagazineHUD(int currentMagazine)
    //{
    //    // ���� ��Ȱ��ȭ�ϰ�, currentMagazine ������ŭ Ȱ��ȭ
    //    for ( int i = 0; i < magazineList.Count; ++i )
    //    {
    //        magazineList[i].SetActive(false);
    //    }

    //    for ( int i = 0; i < currentMagazine; ++i )
    //    {
    //        magazineList[i].SetActive(true);
    //    }
    //}

    void UpdateHPHUD(int previous, int current)
    {
        textHP.text = "HP" + current;

        // ü���� �������� ���� ȭ�鿡 ������ �̹����� ������� �ʵ��� return
        if (previous <= current) return;

        if ( previous - current > 0)
        {
            StopCoroutine("OnBloodScreen");
            StartCoroutine("OnBloodScreen");
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
