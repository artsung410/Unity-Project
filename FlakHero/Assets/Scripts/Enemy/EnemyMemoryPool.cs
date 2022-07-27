using System.Collections;
using UnityEngine;

public class EnemyMemoryPool : MonoBehaviour
{
    [SerializeField]
    private Transform target;                   // 적의 목표 (플레이어)

    [SerializeField]
    private GameObject enemySpawnPointPrefab;   // 적이 등장하기 전 적의 등장 위치를 알려주는 프리팹

    [SerializeField]
    private GameObject enemyPrefabA;             // 생성되는 적 프리팹

    [SerializeField]
    private GameObject enemyPrefabB;             // 생성되는 적 프리팹

    [SerializeField]
    private GameObject enemyMissilePrefab;            // 생성되는 미사일 프리팹

    [SerializeField]
    private float enemySpawnTime = 2;           // 적 생성 주기

    [SerializeField]
    private float enemySpawnLatency = 1;        // 타일 생성 후 적이 등장하기까지 대기시간

    private MemoryPool spawnPointMemoryPool;    // 적 등장 위치를 알려주는 오브젝트 생성, 활성 / 비활성 관리
    private MemoryPool enemyMemoryPoolA;         // 카미카제 비행기 활성 / 비활성 관리
    private MemoryPool enemyMemoryPoolB;         // 미사일 폭격기 활성 / 비활성 관리

    [HideInInspector]
    public MemoryPool enemyMissile;         // 미사일  / 비활성 관리

    private int numberOfEnemiesSpawnedAtOnce = 1; // 동시에 생성되는 적의 숫자
    private Vector2Int mapSize = new Vector2Int(100, 100); // 맵 크기

    private void Awake()
    {
        spawnPointMemoryPool = new MemoryPool(enemySpawnPointPrefab);
        enemyMemoryPoolA = new MemoryPool(enemyPrefabA);
        enemyMemoryPoolB = new MemoryPool(enemyPrefabB);
        enemyMissile = new MemoryPool(enemyMissilePrefab);

        StartCoroutine("SpawnTile");
    }

    // 맵 내부 임의의 위치에 적 등장을 알리는 빨간 기둥을 생성한다.

    // 최초에는 하나씩 생성되고 시간이 흐름에 따라 동시에 생성되는 숫자가 늘어난다.
    private IEnumerator SpawnTile()
    {
        int currentNumber = 0;
        int maximumNumber = 50;

        while (true)
        {
            for (int i = 0; i < numberOfEnemiesSpawnedAtOnce; ++i)
            {
                GameObject item = spawnPointMemoryPool.ActivatePoolItem(); // 기둥 오브젝트를 생성하고

                // 맵 내부에 랜덤하게 위치하도록 설정한다.
                float randomX = Random.Range(-mapSize.x, mapSize.x);
                float randomZ = Random.Range(-mapSize.y, mapSize.y);

                if ((randomX < 50 && randomX > -50) || (randomZ < 50 && randomZ > -50))
                {
                    spawnPointMemoryPool.DeactivatePoolItem(item);
                }
                else
                {
                    item.transform.position = new Vector3(randomX, 40, randomZ);

                    // 일정 시간 후에 기둥위치에서 적이 생성되도록 메서드를 호출한다.
                    if ((int)randomX % 2 == 0)
                    {
                        StartCoroutine(SpawnEnemy(item, enemyMemoryPoolA));
                    }
                    else
                    {
                        StartCoroutine(SpawnEnemy(item, enemyMemoryPoolB));
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

    private IEnumerator SpawnEnemy(GameObject point, MemoryPool eMP)
    {
        yield return new WaitForSeconds(enemySpawnLatency);

        // 적 오브젝트를 생성하고, 적의 위치를 point의 위치로 설정
        GameObject item = eMP.ActivatePoolItem();
        item.transform.position = point.transform.position;

        //item.GetComponent<EnemyFSM>().Setup(target, this);

        // 타일 오브젝트를 비활성화
        spawnPointMemoryPool.DeactivatePoolItem(point);
    }

    public void DeactivateEnemy(GameObject enemy, MemoryPool eMP)
    {
        eMP.DeactivatePoolItem(enemy);
    }

}
