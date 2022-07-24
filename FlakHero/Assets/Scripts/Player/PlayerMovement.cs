using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float        moveSpeed; // 이동속도
    private Vector3     moveForce; // 이동 힘 (x, z와 y축을 별도로 계산해 실제 이동에 적용)

    public float        jumpForce; // 점프 힘
    public float        gravity; // 중력 계수

    public float        MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value);
        get => moveSpeed;
    }

    private CharacterController characterController; // 플레이어 이동 제어를 위한 컴포넌트

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 허공에 떠있으면 중력만큼 y축 이동속도 감소
        if ( !characterController.isGrounded)
        {
            moveForce.y += gravity * Time.deltaTime;
        }

        // 1초당 moveForce 속력으로 이동
        characterController.Move(moveForce * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        // 이동 방향 = 캐릭터 회전 값 * 방향 값 ( 카메라 회전으로 전방 방향이 변하기 때문에 회전값을 곱해서 연산해야한다.)
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);


        // 이동 힘 = 이동방향 * 속도 ( 위나 아래를 바라보고 이동할 경우 캐릭터가 공중으로 뜨거나 아래로 가라 앉으려 하기 때문에 direction을 그대로 사용하지 않고, moveForce변수에 x, z값만 넣어서 사용
        moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);
    }

    public void Jump()
    {
        // 플레이어가 바닥에 있을 때만 점프 가능
        if (characterController.isGrounded)
        {
            moveForce.y = jumpForce;
        }
    }
}
