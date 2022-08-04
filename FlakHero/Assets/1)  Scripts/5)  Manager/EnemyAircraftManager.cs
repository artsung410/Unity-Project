using System.Collections;
using UnityEngine;

public class EnemyAircraftManager : MonoBehaviour
{

    [SerializeField]
    private float enemySpawnTime = 2;           

    [SerializeField]
    private float enemySpawnLatency = 1;       
    private int numberOfEnemiesSpawnedAtOnce = 1; 
    private Vector2Int mapSize = new Vector2Int(100, 100); 

    private void Awake()
    {
        StartCoroutine("SpawnTile");
    }

    private IEnumerator SpawnTile()
    {
        yield return new WaitForSeconds(1f);
        int currentNumber = 0;
        int maximumNumber = 50;

        while (true)
        {
            for (int i = 0; i < numberOfEnemiesSpawnedAtOnce; ++i)
            {
                EnemySpawnBox spanwBox = EnemySpawnBoxPool.GetObject();
                float randomX = Random.Range(-mapSize.x, mapSize.x);
                float randomZ = Random.Range(-mapSize.y, mapSize.y);

                if ((randomX < 50 && randomX > -50) || (randomZ < 50 && randomZ > -50))
                {
                    EnemySpawnBoxPool.ReturnObject(spanwBox);
                }
                else
                {
                    spanwBox.transform.position = new Vector3(randomX, 40, randomZ);
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

        KamikazeAircraft enemy = KamikazeAircraftPool.GetObject();
        enemy.transform.position = spanwBox.transform.position;
        EnemySpawnBoxPool.ReturnObject(spanwBox);
    }

    private IEnumerator SpawnEnemyB(EnemySpawnBox spanwBox)
    {
        yield return new WaitForSeconds(enemySpawnLatency);

        MissileAircraft enemy = MissileAircraftPool.GetObject();
        enemy.transform.position = spanwBox.transform.position;

        EnemySpawnBoxPool.ReturnObject(spanwBox);
    }

}
