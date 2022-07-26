using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rader : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyDotPrefab;

    [SerializeField]
    private GameObject MissileDotPrefab;

    [SerializeField]
    private Transform RaderCanvas;

    [SerializeField]
    private Transform player;

    private GameObject EnemyDot;

    private GameObject MissileDot;

    private List<Collider> enemy;
    void Start()
    { 
        Debug.Log("sartÇÔ¼ö È£ÃâµÊ.");
    }

    private void Update()
    {
        //dot.transform.Translate(Vector3.up * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            float raderCenterX = RaderCanvas.position.x - 80;
            float raderCenterY = RaderCanvas.position.y - 90;

            float wolrdPlayerX = player.position.x;
            float wolrdPlayerY = player.position.z;

            float wolrdEnemyX = other.transform.position.x;
            float wolrdEnemyY = other.transform.position.z;

            float deltaX = (wolrdEnemyX - wolrdPlayerX);
            float deltaY = (wolrdEnemyY - wolrdPlayerY);

            Vector3 PlayerRaderDirection = new Vector3(raderCenterX, raderCenterY, 0);
            Vector3 EnemyRaderDirection = new Vector3(deltaX + RaderCanvas.position.x, deltaY + RaderCanvas.position.y, 0);
            Vector3 PlayerToEnemyDistanceOnRader = EnemyRaderDirection - PlayerRaderDirection;

            EnemyDot = Instantiate(EnemyDotPrefab, PlayerToEnemyDistanceOnRader, Quaternion.identity, GameObject.Find("Canvas").transform);
            Debug.Log($"{other.tag}ÀÇ ÁÂÇ¥°ª: {other.transform.position.x} , {other.transform.position.y}, {other.transform.position.z} ");

            Destroy(EnemyDot, 0.05f);
        }

        if (other.tag == "Missile")
        {
            MissileDot = Instantiate(MissileDotPrefab, (other.transform.position - player.position), Quaternion.identity, GameObject.Find("Canvas").transform);
            Debug.Log("¹Ì»çÀÏ ´å »ý¼º");

            Destroy(MissileDot, 0.05f);
        }
    }
}
