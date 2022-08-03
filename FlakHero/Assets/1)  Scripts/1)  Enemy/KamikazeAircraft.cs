using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeAircraft : EnemyAircraft
{
    [Header("KamikazeAircraft")]
    public  GameObject  target;                      // 플레이어 게임오브젝트 가져오기
    public  int         maxHP               = 100;   // 최대체력
    public  int         currentHP;                   // 현재체력
    public  float       flightSpeed         = 10f;   // 비행 스피드
    public  bool        IsSreachedPlayer;            // 플레이어와 접촉 여부

    private void Awake()
    {
        reset();
    }

    private void Update()
    {
        MoveAndSelfDestruct();
    }

    public override void TakeDamage(int damage)
    {
        if (currentHP <= 0)
        {
            ExplodeAircraft();
            ItemAirDrop();
            GameManager.Instance.AddScore();

            reset();
            KamikazeAircraftPool.ReturnObject(this);
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
            ExplodeAircraft();
            reset();
            KamikazeAircraftPool.ReturnObject(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            IsSreachedPlayer = true;
            other.GetComponent<PlayerController>().TakeDamage(5);
        }
    }

    private void reset()
    {
        currentHP = maxHP;
        IsSreachedPlayer = false;
    }
}
