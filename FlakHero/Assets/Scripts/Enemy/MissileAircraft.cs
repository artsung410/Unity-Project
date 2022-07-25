using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileAircraft : EnemyAircraft
{
    [Header("MisslieAircraft")]

    [SerializeField]
    private float flightSpeed = 10f; // ���� ���ǵ�

    Vector3 destoryDirection; // �÷��̾ ����������� ���� �ı����� ����

    [SerializeField]
    private GameObject target; // �÷��̾� ���ӿ�����Ʈ ��������

    [SerializeField]
    private GameObject MissilePrefab; // �̻��� ������ ��������

    [SerializeField]
    private Transform MissileSpawnPoint; // �̻��� ������ ��������

    public bool IsRreadyToLaunchMissile; // �̻��� �߻� �غ� ����
    public bool IsMissileLunched;

    private void Awake()
    {
        currentHP = maxHP; // ���� ü�� = �ִ�ü������ �ʱ�ȭ
        destoryDirection = new Vector3(transform.position.x, transform.position.y, -transform.position.z);
        IsRreadyToLaunchMissile = false;
        IsMissileLunched = false;
    }

    private void Update()
    {
        MoveAndFireMissile();
    }

    void MoveAndFireMissile()
    {
        Vector3 to = new Vector3(target.transform.position.x, target.transform.position.y + 5, target.transform.position.z);
        Vector3 from = transform.position;

        if (IsRreadyToLaunchMissile == false)
        {
            transform.rotation = Quaternion.LookRotation(to - from);
            transform.Translate(Vector3.forward * Time.deltaTime * flightSpeed);
        }

        else
        {
            //StartCoroutine("ExplodeAircraft");

            transform.rotation = Quaternion.LookRotation(destoryDirection * 2 - from);
            transform.Translate(Vector3.forward * Time.deltaTime * flightSpeed * 2);

            if (IsMissileLunched == false)
            {
                GameObject misslie = Instantiate(MissilePrefab, MissileSpawnPoint.position, MissileSpawnPoint.rotation);
                misslie.SetActive(true);
                IsMissileLunched = true;
            }

            Destroy(gameObject, 2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameManager.Instance.EffectiveRange)
        {
            IsRreadyToLaunchMissile = true;

            Debug.Log("F22 �浹");
        }
    }
}
