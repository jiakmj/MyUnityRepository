using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private Animator animator;

    private bool isAttacking = false;

    [Header("�ִϸ��̼� ���� �̸�")]
    public string attackStateName = "Attack";

    private void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
    }

    public void PerformAttack()
    {
        if (isAttacking) //������ ���� �� �ߺ�ȣ��X
        {
            return;
        }

        if (playerAnimation != null)
        {
            playerAnimation.TriggerAttack();
        }

        StartCoroutine(AttackCooldownByAnimation());
    }

    private IEnumerator AttackCooldownByAnimation()
    {
        isAttacking = true;

        yield return null; //�������� ���� �Ϻη� ���� ���� �����ӱ��� ��ٸ��� �ҷ���
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(attackStateName)) //attack �ִϸ��̼��� �ϰ� ������
        {
            float animatiorLength = stateInfo.length; //�ִϸ��̼� ��� ���̸�ŭ ��ٸ�
            yield return new WaitForSeconds(animatiorLength);
        }
        else 
        {
            yield return new WaitForSeconds(0.5f);
        }

        isAttacking = false; //��� ������ false�� ����
    }
}
