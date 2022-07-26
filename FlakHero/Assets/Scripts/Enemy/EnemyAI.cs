using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Idle,
    Trace,
    Walk,
    Run,
    Attack,
    KnockBack
};

public class EnemyAI : MonoBehaviour
{
    public EnemyState state;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CoroutineIdle");
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case EnemyState.Idle: UpdateIdle(); break;
            case EnemyState.Walk: UpdateIdle(); break;
            case EnemyState.Run: UpdateIdle(); break;
            case EnemyState.Attack: UpdateIdle(); break;
            case EnemyState.KnockBack: UpdateIdle(); break;
        }
    }

    void UpdateIdle()
    {

    }

    void UpdateWalk()
    {

    }

    void UpdateRun()
    {

    }

    void UpdateAttack()
    {

    }

    void UpdateKnockBack()
    {

    }
    // 한번만 수행해야 하는 동작 (상태가 바뀔 때 마다) // yield return null;



    IEnumerator CoroutineIdle()
    {
        Debug.Log("대기 상태 시작");

        while(true)
        {
            yield return new WaitForSeconds(2f);
            // 시간마다 수행해야 하는 동작 (상태가 바뀔 때 마다) 

            ChangeState(EnemyState.Walk);
            yield break;
        }    

    }

    IEnumerator CoroutineWalk()
    {
        Debug.Log("순찰 상태 시작");

        while (true)
        {
            yield return new WaitForSeconds(2f);
            // 시간마다 수행해야 하는 동작 (상태가 바뀔 때 마다) 

            ChangeState(EnemyState.Attack);
            yield break;
        }

    }

    IEnumerator CoroutineRun()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            // 시간마다 수행해야 하는 동작 (상태가 바뀔 때 마다) 
        }

    }

    IEnumerator CoroutineAttack()
    {
        Debug.Log("공격 시작");
        while (true)
        {
            yield return new WaitForSeconds(2f);
            // 시간마다 수행해야 하는 동작 (상태가 바뀔 때 마다) 
        }

    }

    IEnumerator CoroutineKnockBack()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            // 시간마다 수행해야 하는 동작 (상태가 바뀔 때 마다) 
        }

    }

    void ChangeState(EnemyState nextState)
    {
       state = nextState;

        switch(state)
        {
            case EnemyState.Idle: StartCoroutine(CoroutineIdle()); break;
            case EnemyState.Walk: StartCoroutine(CoroutineWalk()); break;
            case EnemyState.Run: StartCoroutine(CoroutineRun()); break;
            case EnemyState.Attack: StartCoroutine(CoroutineAttack()); break;
            case EnemyState.KnockBack: StartCoroutine(CoroutineKnockBack()); break;
        }
    }

    // 매 프레임마다 수행해야 하는 동작 (상태가 바뀔 때 마다)

}
