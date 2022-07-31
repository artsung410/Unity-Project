using System.Collections;
using UnityEngine;

public class EnemyAircraftManager : MonoBehaviour
{

    [SerializeField]
    private float enemySpawnTime = 2;           // �� ���� �ֱ�

    [SerializeField]
    private float enemySpawnLatency = 1;        // Ÿ�� ���� �� ���� �����ϱ���� ���ð�

    //private MemoryPool spawnPointMemoryPool;    // �� ���� ��ġ�� �˷��ִ� ������Ʈ ����, Ȱ�� / ��Ȱ�� ����
    //private MemoryPool enemyMemoryPoolA;         // ī��ī�� ����� Ȱ�� / ��Ȱ�� ����
    //private MemoryPool enemyMemoryPoolB;         // �̻��� ���ݱ� Ȱ�� / ��Ȱ�� ����

    private int numberOfEnemiesSpawnedAtOnce = 1; // ���ÿ� �����Ǵ� ���� ����
    private Vector2Int mapSize = new Vector2Int(100, 100); // �� ũ��

    private void Awake()
    {
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
                EnemySpawnBox spanwBox = EnemySpawnBoxPool.GetObject();// ��� ������Ʈ�� �����ϰ�

                // �� ���ο� �����ϰ� ��ġ�ϵ��� �����Ѵ�.
                float randomX = Random.Range(-mapSize.x, mapSize.x);
                float randomZ = Random.Range(-mapSize.y, mapSize.y);

                if ((randomX < 50 && randomX > -50) || (randomZ < 50 && randomZ > -50))
                {
                    EnemySpawnBoxPool.ReturnObject(spanwBox);
                }
                else
                {
                    spanwBox.transform.position = new Vector3(randomX, 40, randomZ);

                    // ���� �ð� �Ŀ� �����ġ���� ���� �����ǵ��� �޼��带 ȣ���Ѵ�.
                    if ((int)randomX % 2 == 0)
                    {
                        StartCoroutine(SpawnEnemyA(spanwBox));
                    }
                    else
                    {
                        StartCoroutine(SpawnEnemyB(spanwBox));
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

    private IEnumerator SpawnEnemyA(EnemySpawnBox spanwBox)
    {
        yield return new WaitForSeconds(enemySpawnLatency);

        // �� ������Ʈ�� �����ϰ�, ���� ��ġ�� point�� ��ġ�� ����
        KamikazeAircraft enemy = KamikazeAircraftPool.GetObject();
        enemy.transform.position = spanwBox.transform.position;

        //item.GetComponent<EnemyFSM>().Setup(target, this);

        // Ÿ�� ������Ʈ�� ��Ȱ��ȭ
        EnemySpawnBoxPool.ReturnObject(spanwBox);
    }

    private IEnumerator SpawnEnemyB(EnemySpawnBox spanwBox)
    {
        yield return new WaitForSeconds(enemySpawnLatency);

        // �� ������Ʈ�� �����ϰ�, ���� ��ġ�� point�� ��ġ�� ����
        MissileAircraft enemy = MissileAircraftPool.GetObject();
        enemy.transform.position = spanwBox.transform.position;

        //item.GetComponent<EnemyFSM>().Setup(target, this);

        // Ÿ�� ������Ʈ�� ��Ȱ��ȭ
        EnemySpawnBoxPool.ReturnObject(spanwBox);
    }

}
