using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Heal = 0, Coin, Emp, Flak}

public class SupplyBox : MonoBehaviour
{
    // �ڽ�Ŭ������ ����Ҽ��ֵ��� protected�� ����
    [Header("EnemyAircraft")]
    [SerializeField]
    protected int maxHP = 100; // �ִ�ü��

    [SerializeField]
    protected int currentHP; // ����ü��

    [SerializeField]
    protected GameObject explosionPrefab;

    [SerializeField]
    public GameObject[] ItemPrefabs;      // ������ ������

    protected bool isExplode = false;

    int randNum;

    private void Start()
    {
        randNum = Random.Range(0, 4);
    }
    private void Awake()
    {
        currentHP = maxHP; 
    }

    public void TakeDamage(int damage)
    {
        if (transform.position.y < 0.5)
        {
            currentHP -= damage;
        }

        if (currentHP <= 0 && isExplode == false)
        {
            StartCoroutine("Explode");

            ItemInit(ItemPrefabs);
        }
    }

    private IEnumerator Explode()
    {
        // ��ó�� �����Ⱑ ������ �ٽ� ��Ʈ������ �� ��(StackOverflow ����)
        isExplode = true;

        //// ���� ����Ʈ ����
        //Bounds bounds = GetComponent<Collider>().bounds;
        //Instantiate(explosionPrefab, new Vector3(bounds.center.x, bounds.min.y, bounds.center.z), transform.rotation);
        gameObject.SetActive(false);

        yield return null;
    }

    private void ItemInit(GameObject[] items)
    {
        Debug.Log(randNum);

        Vector3 itemDropPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject Heal = Instantiate(items[randNum], itemDropPosition, transform.rotation);
        Heal.SetActive(true);
    }
}
