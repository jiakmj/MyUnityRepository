using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private Animator animator;

    private bool isAttacking = false;

    [Header("애니메이션 상태 이름")]
    public string attackStateName = "Attack";

    private void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();
    }

    public void PerformAttack()
    {
        if (isAttacking) //때리고 있을 때 중복호출X
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

        yield return null; //안정성을 위해 일부러 넣음 다음 프레임까지 기다리게 할려고
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(attackStateName)) //attack 애니메이션을 하고 있으면
        {
            float animatiorLength = stateInfo.length; //애니메이션 모션 길이만큼 기다림
            yield return new WaitForSeconds(animatiorLength);
        }
        else 
        {
            yield return new WaitForSeconds(0.5f);
        }

        isAttacking = false; //모션 끝나면 false로 돌림
    }
}
