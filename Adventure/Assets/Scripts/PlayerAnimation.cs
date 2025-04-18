using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private bool isBow = false;
    private int bowLayerIndex = 1;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleBow()
    {
        isBow = !isBow;
        animator.SetLayerWeight(bowLayerIndex, isBow ? 1f : 0f);
    }

    public bool IsBow() => isBow;

    public void TriggerBowAttack()
    {
        animator.SetTrigger("BowAttack");
        SoundManager.Instance.PlaySFX(SFXType.Attack);
    }

    public void TriggerAttack()
    {
        animator.SetTrigger("Attack");
        SoundManager.Instance.PlaySFX(SFXType.Attack);
    }

    public void SetWalking(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }

    public void SetRunning(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }

    public void SetPushing(bool ispushing)
    {
        animator.SetBool("ispushing", ispushing);
    }

    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
    }

    public void TriggerHit()
    {
        animator.SetTrigger("Hit");
    }

    public void TriggerDead()
    {
        animator.SetTrigger("Dead");
    }

    public void OnPlayWalkSound()
    {
        SoundManager.Instance.PlaySFX(SFXType.StepSound);
    }

    public void OnPlayJumpSound()
    {
        SoundManager.Instance.PlaySFX(SFXType.JumpSound);
    }
}