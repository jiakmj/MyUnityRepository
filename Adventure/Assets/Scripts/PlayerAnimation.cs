using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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

    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
    }

    public void OnPlayWalkSound()
    {
        SoundManager.Instance.PlaySFX(SFXType.StepSound);
    }

    public void OnPlayJumpSound()
    {
        SoundManager.Instance.PlaySFX(SFXType.JumpSound);
    }

    //public void OnPlayLandingSound()
    //{

    //}


    //public void SetJumping(bool isJumping)
    //{
    //    animator.SetBool("isJumping", isJumping);
    //}

    //public void SetFalling(bool isFalling)
    //{
    //    animator.SetBool("isFalling", isFalling);
    //}

    //public void PlayLanding()
    //{
    //    animator.SetTrigger("Land");
    //    animator.SetBool("isJumping", false);
    //    animator.SetBool("isFalling", false);
    //}
}
