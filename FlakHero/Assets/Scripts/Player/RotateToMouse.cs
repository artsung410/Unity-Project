using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{
    public float rotCamXAxisSpeed = 5f; // ī�޶� x�� ȸ���ӵ�

    public float rotCamYAxisSpeed = 3f; // ī�޶� y�� ȸ���ӵ�

    private float limitMinX = -80; // ī�޶� x�� ȸ�� ���� (�ּ�)
    private float limitMaxX = 50; // ī�޶� x�� ȸ�� ���� (�ִ�)
    private float eulerAngleX;
    private float eulerAngleY;

    public GameObject Rader_Player;

    public void UpdateRotate(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed; // ���콺 �� / �� �̵����� ī�޶� y�� ȸ��
        eulerAngleX -= mouseY * rotCamYAxisSpeed; // ���콺 �� / �Ʒ� �̵����� ī�޶� x�� ȸ��

        // ī�޶� x�� ȸ���� ��� ȸ�� ������ ����
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);

        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);

        Rader_Player.transform.rotation = Quaternion.Euler(0, 0, -eulerAngleY * 1.1f);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }

        if (angle > 360)
        {
            angle -= 360;
        }

        return Mathf.Clamp(angle, min, max);
    }
}
