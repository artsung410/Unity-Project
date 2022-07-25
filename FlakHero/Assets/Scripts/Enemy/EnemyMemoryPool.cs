using System.Collections;
using UnityEngine;

public class EnemyMemoryPool : MonoBehaviour
{
    [SerializeField]
    private Transform target;                   // ���� ��ǥ (�÷��̾�)

    [SerializeField]
    private GameObject enemySpawnPointPrefab;   // ���� �����ϱ� �� ���� ���� ��ġ�� �˷��ִ� ������

    [SerializeField]
    private GameObject enemyPrefabA;             // �����Ǵ� �� ������

    [SerializeField]
    private GameObject enemyPrefabB;             // �����Ǵ� �� ������

    [SerializeField]
    private float enemySpawnTime = 2;           // �� ���� �ֱ�

    [SerializeField]
    private float enemySpawnLatency = 1;        // Ÿ�� ���� �� ���� �����ϱ���� ���ð�

    private MemoryPool spawnPointMemoryPool;    // �� ���� ��ġ�� �˷��ִ� ������Ʈ ����, Ȱ�� / ��Ȱ�� ����
    private MemoryPool enemyMemoryPoolA;         // ī��ī�� ����� Ȱ�� / ��Ȱ�� ����
    private MemoryPool enemyMemoryPoolB;         // �̻��� ���ݱ� Ȱ�� / ��Ȱ�� ����

    private int numberOfEnemiesSpawnedAtOnce = 1; // ���ÿ� �����Ǵ� ���� ����
    private Vector2Int mapSize = new Vector2Int(100, 100); // �� ũ��

    private void Awake()
    {
        spawnPointMemoryPool = new MemoryPool(enemySpawnPointPrefab);
        enemyMemoryPoolA = new MemoryPool(enemyPrefabA);
        enemyMemoryPoolB = new MemoryPool(enemyPrefabB);

        StartCoroutine("SpawnTile");
    }

    // �� ���� ������ ��ġ�� �� ������ �˸��� ���� ����� �����Ѵ�.

    // ���ʿ��� �ϳ��� �����ǰ� �ð��� �帧�� ���� ���ÿ� �����Ǵ� ���ڰ� �þ��.
    private IEnumerator SpawnTile()
    {
        int currentNumber = 0;
        int maximumNumber = 50;

        while (true)
        {
            for (int i = 0; i < numberOfEnemiesSpawnedAtOnce; ++i)
            {
                GameObject item = spawnPointMemoryPool.ActivatePoolItem(); // ��� ������Ʈ�� �����ϰ�

                // �� ���ο� �����ϰ� ��ġ�ϵ��� �����Ѵ�.
                float randomX = Random.Range(-mapSize.x, mapSize.x);
                float randomZ = Random.Range(-mapSize.y, mapSize.y);

                if ((randomX < 50 && randomX > -50) || (randomZ < 50 && randomZ > -50))
                {
                    spawnPointMemoryPool.DeactivatePoolItem(item);
                }
                else
                {
                    item.transform.position = new Vector3(randomX, 40, randomZ);

                    // ���� �ð� �Ŀ� �����ġ���� ���� �����ǵ��� �޼��带 ȣ���Ѵ�.
                    if ((int)randomX % 2 == 0)
                    {
                        StartCoroutine("SpawnEnemyA", item);
                    }
                    else
                    {
                        StartCoroutine("SpawnEnemyB", item);
                    }
                }
            }

            currentNumber++;

            if (currentNumber >= maximumNumber)
            {
                currentNumber = 0;
                numberOfEnemiesSpawnedAtOnce++;
            }

            yield return new WaitForSeconds(enemySpawnTime);
        }
    }

    private IEnumerator SpawnEnemyA(GameObject point)
    {
        yield return new WaitForSeconds(enemySpawnLatency);

        // �� ������Ʈ�� �����ϰ�, ���� ��ġ�� point�� ��ġ�� ����
        GameObject item = enemyMemoryPoolA.ActivatePoolItem();
        item.transform.position = point.transform.position;

        //item.GetComponent<EnemyFSM>().Setup(target, this);

        // Ÿ�� ������Ʈ�� ��Ȱ��ȭ
        spawnPointMemoryPool.DeactivatePoolItem(point);
    }

    public void DeactivateEnemyA(GameObject enemy)
    {
        enemyMemoryPoolA.DeactivatePoolItem(enemy);
    }

    private IEnumerator SpawnEnemyB(GameObject point)
    {
        yield return new WaitForSeconds(enemySpawnLatency);

        // �� ������Ʈ�� �����ϰ�, ���� ��ġ�� point�� ��ġ�� ����
        GameObject item = enemyMemoryPoolB.ActivatePoolItem();
        item.transform.position = point.transform.position;

        // Ÿ�� ������Ʈ�� ��Ȱ��ȭ
        spawnPointMemoryPool.DeactivatePoolItem(point);
    }

    public void DeactivateEnemyB(GameObject enemy)
    {
        enemyMemoryPoolB.DeactivatePoolItem(enemy);
    }

}
