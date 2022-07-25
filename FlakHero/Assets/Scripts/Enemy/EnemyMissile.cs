using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    [Header("Missile")]

    [SerializeField]
    private float flightSpeed = 10f; // ���� ���ǵ�

    [SerializeField]
    private GameObject target; // �÷��̾� ���ӿ�����Ʈ ��������

    [SerializeField]
    protected GameObject explosionPrefab;

    [SerializeField]
    protected float explosionDelayTime = 0.1f;

    [SerializeField]
    protected float explosionRadius = 10.0f;

    [SerializeField]
    protected float explosionForce = 1000.0f;

    public bool IsSreachedPlayer; // �÷��̾�� ���� ����

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

    private IEnumerator ExplodeAircraft()
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

        Destroy(gameObject);
    }
}
