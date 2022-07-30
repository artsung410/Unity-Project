using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }


public class Status : MonoBehaviour
{
    [HideInInspector]
    public HPEvent      onHPEvent = new HPEvent();

    [Header("Walk, Run Speed")]
    [SerializeField]
    private float       walkSpeed;
    [SerializeField]
    private float       runSpeed;

    [Header("HP")]
    [SerializeField]
    private int         maxHP = 100;
    private int         currentHP;
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    private WeaponAssaultRifle rifle;

    private void Awake()
    {
        currentHP = maxHP;
        rifle = GetComponentInChildren<WeaponAssaultRifle>();
    }

    public bool DecreaseHP(int damage)
    {
        int previousHP = currentHP;

        currentHP = currentHP - damage > 0 ? currentHP - damage : 0;

        onHPEvent.Invoke(previousHP, currentHP);

        if ( currentHP == 0)
        {
            GameManager.Instance.End();
            return true;
        }

        return false;
    }

    public void IncreaseHP(int hp)
    {
        int previousHP = currentHP;

        currentHP = currentHP + hp > maxHP ? maxHP : currentHP + hp; // 현재 체력을 최대 체력을 넘지않게 조건식 정의

        onHPEvent.Invoke(previousHP, currentHP);
    }
}
