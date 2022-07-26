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
    // �ѹ��� �����ؾ� �ϴ� ���� (���°� �ٲ� �� ����) // yield return null;



    IEnumerator CoroutineIdle()
    {
        Debug.Log("��� ���� ����");

        while(true)
        {
            yield return new WaitForSeconds(2f);
            // �ð����� �����ؾ� �ϴ� ���� (���°� �ٲ� �� ����) 

            ChangeState(EnemyState.Walk);
            yield break;
        }    

    }

    IEnumerator CoroutineWalk()
    {
        Debug.Log("���� ���� ����");

        while (true)
        {
            yield return new WaitForSeconds(2f);
            // �ð����� �����ؾ� �ϴ� ���� (���°� �ٲ� �� ����) 

            ChangeState(EnemyState.Attack);
            yield break;
        }

    }

    IEnumerator CoroutineRun()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            // �ð����� �����ؾ� �ϴ� ���� (���°� �ٲ� �� ����) 
        }

    }

    IEnumerator CoroutineAttack()
    {
        Debug.Log("���� ����");
        while (true)
        {
            yield return new WaitForSeconds(2f);
            // �ð����� �����ؾ� �ϴ� ���� (���°� �ٲ� �� ����) 
        }

    }

    IEnumerator CoroutineKnockBack()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            // �ð����� �����ؾ� �ϴ� ���� (���°� �ٲ� �� ����) 
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

    // �� �����Ӹ��� �����ؾ� �ϴ� ���� (���°� �ٲ� �� ����)

}
