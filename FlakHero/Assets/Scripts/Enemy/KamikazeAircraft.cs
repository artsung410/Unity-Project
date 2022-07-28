using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeAircraft : EnemyAircraft
{
    [Header("KamikazeAircraft")]

    [SerializeField]
    protected int maxHP = 100; // 최대체력

    [SerializeField]
    protected int currentHP; // 현재체력

    [SerializeField]
    private float flightSpeed = 10f; // 비행 스피드

    [SerializeField]
    private GameObject target; // 플레이어 게임오브젝트 가져오기

    public bool IsSreachedPlayer; // 플레이어와 접촉 여부

    private void Awake()
    {
        currentHP = maxHP; // 현재 체력 = 최대체력으로 초기화
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

            Debug.Log("적이 플레이어와 접촉함");
        }
    }
}
