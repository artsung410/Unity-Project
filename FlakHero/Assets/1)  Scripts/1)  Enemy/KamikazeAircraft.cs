using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeAircraft : EnemyAircraft
{
    [Header("KamikazeAircraft")]
    public  GameObject  target;                      // �÷��̾� ���ӿ�����Ʈ ��������
    public  int         maxHP               = 100;   // �ִ�ü��
    public  int         currentHP;                   // ����ü��
    public  float       flightSpeed         = 10f;   // ���� ���ǵ�
    public  bool        IsSreachedPlayer;            // �÷��̾�� ���� ����

    private void Awake()
    {
        currentHP = maxHP;
        IsSreachedPlayer = false; 
    }

    private void Update()
    {
        MoveAndSelfDestruct();
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

    void MoveAndSelfDestruct()
    {
        Vector3 to = new Vector3(target.transform.position.x, target.transform.position.y + 5, target.transform.position.z);
        Vector3 from = transform.position;

        if (IsSreachedPlayer == false)
        {
            transform.rotation = Quaternion.LookRotation(to - from);
            transform.Translate(Vector3.forward * Time.deltaTime * flightSpeed);
        }
        else
        {
            StartCoroutine("ExplodeAircraft");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameManager.Instance.Player)
        {
            IsSreachedPlayer = true;

            Debug.Log("���� �÷��̾�� ������");
        }
    }
}