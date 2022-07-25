using System.Collections;
using UnityEngine;

public class EnemyMemoryPool : MonoBehaviour
{
    [SerializeField]
    private Transform target;                   // ���� ��ǥ (�÷��̾�)

    [SerializeField]
    private GameObject enemySpawnPointPrefab;   // ���� �����ϱ� �� ���� ���� ��ġ�� �˷��ִ� ������

    [SerializeField]
    private GameObject enemyPrefab;             // �����Ǵ� �� ������

    [SerializeField]
    private float enemySpawnTime = 2;           // �� ���� �ֱ�

    [SerializeField]
    private float enemySpawnLatency = 1;        // Ÿ�� ���� �� ���� �����ϱ���� ���ð�

    private MemoryPool spawnPointMemoryPool;    // �� ���� ��ġ�� �˷��ִ� ������Ʈ ����, Ȱ�� / ��Ȱ�� ����
    private MemoryPool enemyMemoryPool;         // �� ����, Ȱ�� / ��Ȱ�� ����

    private int numberOfEnemiesSpawnedAtOnce = 1; // ���ÿ� �����Ǵ� ���� ����
    private Vector2Int mapSize = new Vector2Int(100, 100); // �� ũ��

    private void Awake()
    {
        spawnPointMemoryPool = new MemoryPool(enemySpawnPointPrefab);
        enemyMemoryPool = new MemoryPool(enemyPrefab);

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
                    StartCoroutine("SpawnEnemy", item);
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

    private IEnumerator SpawnEnemy(GameObject point)
    {
        yield return new WaitForSeconds(enemySpawnLatency);

        // �� ������Ʈ�� �����ϰ�, ���� ��ġ�� point�� ��ġ�� ����
        GameObject item = enemyMemoryPool.ActivatePoolItem();
        item.transform.position = point.transform.position;

        //item.GetComponent<EnemyFSM>().Setup(target, this);

        // Ÿ�� ������Ʈ�� ��Ȱ��ȭ
        spawnPointMemoryPool.DeactivatePoolItem(point);
    }

    public void DeactivateEnemy(GameObject enemy)
    {
        enemyMemoryPool.DeactivatePoolItem(enemy);
    }

}
