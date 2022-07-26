using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rader : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyDotPrefab;

    [SerializeField]
    private RectTransform RaderCanvas;

    [SerializeField]
    private Transform player;

    private GameObject EnemyDot;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Vector3 PlayerToEnemyDistanceOnRader = CalculateDistanceOfRader(other);
            EnemyDot = Instantiate(EnemyDotPrefab, PlayerToEnemyDistanceOnRader, Quaternion.identity, GameObject.Find("PanelRader").transform);

            Destroy(EnemyDot, 0.03f);
        }
    }

    private Vector3 CalculateDistanceOfRader(Collider enemy)
    {
        float raderCenterX = RaderCanvas.position.x - 95;
        float raderCenterY = RaderCanvas.position.y - 105;

        float worldPlayerX = player.position.x;
        float worldPlayerY = player.position.z;

        float wolrdEnemyX = enemy.transform.position.x;
        float wolrdEnemyY = enemy.transform.position.z;

        float deltaX = (wolrdEnemyX - worldPlayerX);
        float deltaY = (wolrdEnemyY - worldPlayerY);

        Vector3 PlayerRaderDirection = new Vector3(raderCenterX, raderCenterY, 0);
        Vector3 EnemyRaderDirection = new Vector3(deltaX * 0.7f + RaderCanvas.position.x, deltaY * 0.7f + RaderCanvas.position.y, 0);
        Vector3 Distance = EnemyRaderDirection - PlayerRaderDirection;

        return Distance;
    }
}
