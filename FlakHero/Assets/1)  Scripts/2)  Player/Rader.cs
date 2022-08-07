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
        if (other.tag == "EnemyA" || other.tag == "EnemyB")
        {
            RadarDot EnemyDot = RadarDotPool.GetObject();
            EnemyDot.gameObject.transform.position = CalculateDirection(other);
            StartCoroutine(DeActivationDot(EnemyDot));
        }
    }

    Vector3 CalculateDirection(Collider enemy)
    {
        // 월드상 플레이어 좌표
        float worldPlayerX = player.position.x;
        float worldPlayerY = player.position.z;

        // 월드상 적 좌표 
        float wolrdEnemyX = enemy.transform.position.x;
        float wolrdEnemyY = enemy.transform.position.z;

        // 실제 캔버스상에 레이더 좌표를 구한다.
        Vector3 CanvasVec = new Vector3(RaderCanvas.position.x, RaderCanvas.position.y, 0);

        // 플레이어가 적을 바라보는 방향벡터를 정의한다.
        Vector3 DeltaVec = new Vector3(wolrdEnemyX - worldPlayerX, wolrdEnemyY - worldPlayerY, 0) * 0.7f;

        // 적 좌표가 회전하는 축
        Vector3 StandardAxis = new Vector3(0, 0, 1f);

        Vector3 NewDeltaVec = Quaternion.AngleAxis(rotateToMouse.eulerAngleY * 1f, StandardAxis) * DeltaVec;

        Vector3 EnemyDirectionOnRader = CanvasVec + NewDeltaVec;

        return EnemyDirectionOnRader;
    }

    IEnumerator DeActivationDot(RadarDot Dot)
    {
        yield return new WaitForSeconds(0.02f);
        RadarDotPool.ReturnObject(Dot);
    }
}
