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

    private RotateToMouse rotateToMouse;

    private void Awake()
    {
        rotateToMouse = GetComponentInParent<RotateToMouse>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyDot = Instantiate(EnemyDotPrefab, GameObject.Find("Canvas").transform);

            float worldPlayerX = player.position.x;
            float worldPlayerY = player.position.z;

            float wolrdEnemyX = other.transform.position.x;
            float wolrdEnemyY = other.transform.position.z;

            float deltaX = (wolrdEnemyX - worldPlayerX);
            float deltaY = (wolrdEnemyY - worldPlayerY);

            Vector3 CanvasPos = new Vector3(RaderCanvas.position.x, RaderCanvas.position.y, 0);

            Vector3 Distance = new Vector3(deltaX, deltaY, 0) * 0.7f;

            Vector3 NewDirection = new Vector3(0, 0, 1f);

            Vector3 direction = Quaternion.AngleAxis(rotateToMouse.eulerAngleY * 1f, NewDirection) * Distance;


            EnemyDot.transform.position = CanvasPos + direction;

            Destroy(EnemyDot, 0.02f);
        }
    }
}
