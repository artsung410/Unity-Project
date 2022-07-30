using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    [Header("Missile")]
    public  GameObject  target;                             
    public  GameObject  explosionPrefab;
    public  float       flightSpeed         = 10f;          
    public  float       explosionDelayTime  = 0.1f;
    public  float       searchTime          = 2f;
    public  float       explosionRadius     = 10.0f;
    public  float       explosionForce      = 1000.0f;
    public  bool        IsSreachedPlayer;                   

    private void Awake()
    {
        IsSreachedPlayer = false;
    }

    private void Update()
    {
        MoveAndSelfBoom();
    }

    void MoveAndSelfBoom()
    {
        Vector3 to = new Vector3(target.transform.position.x, target.transform.position.y - 4, target.transform.position.z);
        Vector3 from = transform.position;

        if (IsSreachedPlayer == false)
        {
            // �÷��̾ ȸ�������� �ڵ����� �����ϵ��� ó��
            
            StartCoroutine("NotFoundTarget");

            transform.rotation = Quaternion.LookRotation(to - from);
            transform.Translate(Vector3.forward * Time.deltaTime * flightSpeed);

            if (gameObject == null)
            {
                StopCoroutine("NotFoundTarget");
            }
        }

        else
        {
            StartCoroutine("ExplodeMissile");
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

    private IEnumerator ExplodeMissile()
    {
        yield return new WaitForSeconds(explosionDelayTime);

        // ���� ����Ʈ ����
        Bounds bounds = GetComponent<Collider>().bounds;
        Instantiate(explosionPrefab, new Vector3(bounds.center.x, bounds.min.y, bounds.center.z), transform.rotation);

        //// ���� ������ �ִ� ��� ������Ʈ�� collider ������ �޾ƿ� ���� ȿ�� ó��
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            // ���� ������ �ε��� ������Ʈ�� �÷��̾��� �� ó��
            PlayerController player = hit.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(5);
                continue;
            }
        }

        gameObject.SetActive(false);
    }

    private IEnumerator NotFoundTarget()
    {
        yield return new WaitForSeconds(searchTime);

        // ���� ����Ʈ ����
        Bounds bounds = GetComponent<Collider>().bounds;
        Instantiate(explosionPrefab, new Vector3(bounds.center.x, bounds.min.y, bounds.center.z), transform.rotation);

        if (gameObject != null)
        {
            gameObject.SetActive(false);
        }
    }
}