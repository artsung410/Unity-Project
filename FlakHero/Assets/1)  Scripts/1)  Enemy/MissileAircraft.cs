using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileAircraft : EnemyAircraft
{
    [Header("MisslieAircraft")]
    public  GameObject  target;                     // �÷��̾� ���ӿ�����Ʈ ��������
    public  GameObject  MissilePrefab;              // �̻��� ������ ��������
    public  Transform   MissileSpawnPoint;          // �̻��� ������ ��������
    public  int         maxHP = 100;                // �ִ�ü��
    public  int         currentHP;                  // ����ü��
    public  float       flightSpeed = 10f;          // ���� ���ǵ�
    public  bool        IsRreadyToLaunchMissile;    // �̻��� �߻� �غ� ����
    public  bool        IsMissileLunched;
    public  float       survivalTime = 2f;

    Vector3 destoryDirection; // �÷��̾ ����������� ���� �ı����� ����

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

    public override void TakeDamage(int damage)
    {
        if (currentHP <= 0 && isExplode == false)
        {
            StartCoroutine("ExplodeAircraft");

            ItemAirDrop();
            GameManager.Instance.AddScore();
        }

        currentHP -= damage;
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
            transform.rotation = Quaternion.LookRotation(destoryDirection * 2 - from);
            transform.Translate(Vector3.forward * Time.deltaTime * flightSpeed * 2);

            if (IsMissileLunched == false)
            {
                GameObject misslie = Instantiate(MissilePrefab, MissileSpawnPoint.position, MissileSpawnPoint.rotation);
                misslie.SetActive(true);
                IsMissileLunched = true;
            }

            StartCoroutine("DeactiveSelf");
        }
    }

    private IEnumerator DeactiveSelf()
    {
        yield return new WaitForSeconds(survivalTime);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameManager.Instance.EffectiveRange)
        {
            IsRreadyToLaunchMissile = true;
        }
    }
}