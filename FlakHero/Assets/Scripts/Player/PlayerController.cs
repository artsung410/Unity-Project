using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCodes")]
    public      KeyCode                     KeyCodeRun = KeyCode.LeftShift;     // 달리기 키
    public      KeyCode                     KeyCodeJump = KeyCode.Space;        // 점프 키
    public      KeyCode                     keyCodeReload = KeyCode.R;          // 탄 재장전 키

    [Header("Audio Clips")]
    public      AudioClip                   audioClipWalk;  // 걷기 사운드
    public      AudioClip                   audioClipRun;   // 달리기 사운드
    
    private     RotateToMouse               rotateToMouse;  // 마우스 이동으로 카메라 회전
    private     PlayerMovement              movement;       // 키보드 입력으로 플레이어 이동, 점프
    private     Status                      status;         // 이동속도 등의 플레이어 정보
    private     PlayerAnimatorController    animator;       // 애니메이션 재생제어
    private     AudioSource                 audioSource;    // 사운드 재생 제어
    private     WeaponAssaultRifle          Weapon;         // 무기를 이용한 공격 제어

    void Awake()
    {
        // 마우스 커서를 보이지 않게 설정하고, 현재 위치에 고정시킨다.
        Cursor.visible      = false;
        Cursor.lockState    = CursorLockMode.Locked;

        rotateToMouse       = GetComponent<RotateToMouse>();
        movement            = GetComponent<PlayerMovement>();
        status              = GetComponent<Status>();
        animator            = GetComponent<PlayerAnimatorController>();
        audioSource         = GetComponent<AudioSource>();
        Weapon              = GetComponentInChildren<WeaponAssaultRifle>();
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver == false)
        {
            UpdateRotate();
            UpdateMove();
            UpdateJump();
            UpdateWeaponAction();
        }
    }

    void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 이동중 일 때 (걷기 or 뛰기)
        if (x != 0 || z != 0)
        {
            bool isRun = false;

            // 옆이나 뒤로 이동할 때는 달릴 수 없다.
            if (z > 0)
            {
                isRun = Input.GetKey(KeyCodeRun);
            }
            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
            animator.MoveSpeed = isRun == true ? 1 : 0.5f;
            audioSource.clip   = isRun == true ? audioClipRun : audioClipWalk;

            // 방향키 입력 여부는 매 프레임 확인하기 때문에
            // 재생중일 때는 다시 재생하지 않도록 isPlaying으로 체크하여 재생
            if (audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }

        // 제자리에 멈춰있을 때
        else
        {
            movement.MoveSpeed = 0;
            animator.MoveSpeed = 0;

            // 멈췄을 때 사운드가 재생중이면 정지
            if (audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }

        movement.MoveTo(new Vector3(x, 0, z));
    }

    void UpdateJump()
    {
        if (Input.GetKeyDown(KeyCodeJump))
        {
            movement.Jump();
        }
    }

    void UpdateWeaponAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Weapon.StartWeaponAction();
        }

        else if (Input.GetMouseButtonUp(0))
        {
            Weapon.StopWeaponAction();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Weapon.StartWeaponAction(1);
        }

        else if (Input.GetMouseButtonUp(1))
        {
            Weapon.StopWeaponAction(1);
        }

        if (Input.GetKeyDown(keyCodeReload))
        {
            Weapon.StartReload();
        }
    }

    public void TakeDamage(int damage)
    {
        bool isDie = status.DecreaseHP(damage);

        if (isDie == true)
        {
            Debug.Log("GameOver");
        }
    }

}
