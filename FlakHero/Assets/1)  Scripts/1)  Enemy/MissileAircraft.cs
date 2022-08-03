using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileAircraft : EnemyAircraft
{
    [Header("MisslieAircraft")]
    public  GameObject  target;                     
    public  Transform   MissileSpawnPoint;          
    public  int         maxHP = 100;                
    public  int         currentHP;                  
    public  float       flightSpeed = 10f;          
    public  bool        IsRreadyToLaunchMissile;    
    public  bool        IsMissileLunched;
    public  float       survivalTime = 2f;

    Vector3 destoryDirection; // 플레이어를 지나쳤을경우 추후 파괴지점 설정

    private void Awake()
    {
        reset();
    }

    private void Update()
    {
        MoveAndFireMissile();
    }

    public override void TakeDamage(int damage)
    {
        if (currentHP <= 0)
        {
            ExplodeAircraft();

            reset();
            MissileAircraftPool.ReturnObject(this);

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
                EnemyMissile missile = EnemyMissilePool.GetObject();
                missile.gameObject.transform.position = MissileSpawnPoint.position;
                missile.gameObject.transform.rotation = MissileSpawnPoint.rotation;
                IsMissileLunched = true;
            }
            StartCoroutine("DeactiveSelf");
        }
    }

    private IEnumerator DeactiveSelf()
    {
        yield return new WaitForSeconds(survivalTime);

        reset();
        MissileAircraftPool.ReturnObject(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GameManager.Instance.EffectiveRange)
        {
            IsRreadyToLaunchMissile = true;
        }
    }

    private void reset()
    {
        currentHP = maxHP; // 현재 체력 = 최대체력으로 초기화
        destoryDirection = new Vector3(transform.position.x, transform.position.y, -transform.position.z);
        IsRreadyToLaunchMissile = false;
        IsMissileLunched = false;
    }
}
