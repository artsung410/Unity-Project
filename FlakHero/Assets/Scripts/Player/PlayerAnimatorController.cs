using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        // "Player" ������Ʈ �������� �ڽ� ������Ʈ�� "arms_assault_01" ������Ʈ�� Animator ������Ʈ�� �ִ�.
        animator = GetComponentInChildren<Animator>();
    }

    public float MoveSpeed
    {
        set => animator.SetFloat("movementSpeed", value);
        get => animator.GetFloat("movementSpeed");
    }

    /// <summary>
    /// ������ �ִϸ��̼��� ����Ѵ�.
    /// </summary>
    public void OnRelaod()
    {
        animator.SetTrigger("onReload");
    }

    // Assault Rifle ���콺 ������ Ŭ�� �׼� (default / aim mode)
    public bool AimModeIs
    {
        set => animator.SetBool("isAimMode", value);
        get => animator.GetBool("isAimMode");
    }

    public void Play(string stateName, int layer, float normalizedTime)
    {
        animator.Play(stateName, layer, normalizedTime);
    }

    /// <summary>
    /// ���� ��������� Ȯ���ϰ� �� ����� ��ȯ��.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool CurrentAnimationIs(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
}
