using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileAircraft : EnemyAircraft
{
    [Header("MisslieAircraft")]

    [SerializeField]
    private float flightSpeed = 10f; // 비행 스피드

    Vector3 destoryDirection; // 플레이어를 지나쳤을경우 추후 파괴지점 설정

    [SerializeField]
    private GameObject target; // 플레이어 게임오브젝트 가져오기

    public bool IsRreadyToLaunchMissile; // 미사일 발사 준비 여부

    private void Awake()
    {
        currentHP = maxHP; // 현재 체력 = 최대체력으로 초기화
        destoryDirection = new Vector3(transform.position.x, transform.position.y, -transform.position.z);
        IsRreadyToLaunchMissile = false;
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
            transform.rotation = Quaternion.LookRotation(destoryDirection * 2 - from);
            transform.Translate(Vector3.forward * Time.deltaTime * flightSpeed * 2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameManager.Instance.EffectiveRange)
        {
            IsRreadyToLaunchMissile = true;

            Debug.Log("F22 충돌");
        }
    }
}
