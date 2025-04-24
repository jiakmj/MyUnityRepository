using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{    
    private PlayerController playerController;
    private PlayerAnimation playerAnimation;
    private Animator animator;

    public ArrowShoot arrowShoot;

    public List<GameObject> attackObjList = new List<GameObject>();

    public GameObject bowObject;
    private Animator bowAnimator;

    private bool isAttacking = false;
    private float attackDelay = 1f;
        
    //public float shakeDuration = 0.5f;
    //public float shakeMagnitude = 0.1f;
    //private Vector3 originalPos;

    [Header("애니메이션 상태 이름")]
    public string attackStateName = "Attack";

    //[Header("카메라 쉐이크 설정")]
    //public CinemachineImpulseSource impulseSource;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();

        if (bowObject != null)
        {
            bowAnimator = bowObject.GetComponent<Animator>();
            bowObject.SetActive(false);
        }

        if (arrowShoot != null)
        {
            arrowShoot.playerSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        //if (Camera.main != null)
        //{
        //    originalPos = Camera.main.transform.localPosition;
        //}
    }

    public void PerformAttack()
    {
        if (isAttacking) //때리고 있을 때 중복호출X
        {
            return;
        }

        if (playerController != null && (playerController.IsMoving() || playerController.IsJumping()))
        {
            Debug.Log("이동 또는 점프 중이므로 공격 불가");
            return;
        }

        if (playerAnimation.IsBow())
        {
            playerAnimation.TriggerBowAttack();

            if (bowObject != null)
            {
                bowObject.SetActive(true);
            }

            if (bowAnimator != null)
            {
                AnimatorStateInfo bowState = bowAnimator.GetCurrentAnimatorStateInfo(0);
                if (!bowState.IsName("BowAttack"))
                {
                    bowAnimator.ResetTrigger("BowAttack");
                    bowAnimator.SetTrigger("BowAttack");
                }
            }
            if (arrowShoot != null)
            {
                arrowShoot.Fire();
            }
        }
        else
        {
            playerAnimation.TriggerAttack();
        }

        StartCoroutine(AttackCooldownByAnimation());
    }

    private IEnumerator AttackCooldownByAnimation()
    {
        isAttacking = true;
        //float shakeDuration = 0.1f;
        //float shakeMagnitude = 0.1f;
        //StartCoroutine(shakeManager.Shake(shakeDuration, shakeMagnitude));
        //shakeManager.GenerateCameraImpulse();
        //ParticleManager.Instance.ParticlePlay(ParticleType.PlayerAttack, attackObjList[1].transform.position, new Vector3(3, 3, 3));
        //yield return null; //안정성을 위해 일부러 넣음 다음 프레임까지 기다리게 할려고

        float waitTime = 0.5f;
        if (playerAnimation.IsBow() && bowAnimator != null)
        {
            AnimatorStateInfo bowState = bowAnimator.GetCurrentAnimatorStateInfo(0);
            if (bowState.IsName("BowMiddleArrowAni"))
            {
                waitTime = bowState.length;
            }
        }
        else
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("PlayerAttack1Ani") || stateInfo.IsName("BowAttackAni")) //attack 애니메이션을 하고 있으면
            {
                waitTime = stateInfo.length;
            }
        }

        yield return new WaitForSeconds(waitTime);


        if (playerAnimation.IsBow() && bowObject != null)
        {
            bowObject.SetActive(false);
        }


        isAttacking = false; //모션 끝나면 false로 돌림               
    }

    public void AttackStart()
    {
        bool isFacingLeft = GetComponent<SpriteRenderer>().flipX;

        if (isFacingLeft)
        {
            if (attackObjList.Count > 0)
            {
                attackObjList[0].SetActive(true);
            }
        }
        else
        {
            if (attackObjList.Count > 0)
            {
                attackObjList[1].SetActive(true);
            }
        }
    }

    public void AttackEnd()
    {
        bool isFacingLeft = GetComponent<SpriteRenderer>().flipX;

        if (isFacingLeft)
        {
            if (attackObjList.Count > 0)
            {
                attackObjList[0].SetActive(false);
            }
        }
        else
        {
            if (attackObjList.Count > 0)
            {
                attackObjList[1].SetActive(false);
            }
        }
        

    }

    //public IEnumerator Shake(float duration, float magnitude)
    //{
    //    originalPos = Camera.main.transform.localPosition;
    //    Camera.main.GetComponent<CinemachineBrain>().enabled = false;
    //    if (Camera.main == null)
    //    {
    //        yield return null;
    //    }

    //    float elapsed = 0.0f;

    //    while (elapsed < duration)
    //    {
    //        float x = Random.Range(-1f, 1f) * magnitude;
    //        float y = Random.Range(-1f, 1f) * magnitude;

    //        Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, originalPos.y + y, -10);

    //        elapsed += Time.deltaTime;

    //        yield return null;
    //    }

    //    Camera.main.transform.localPosition = originalPos;
    //    Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    //}

    //private void GenerateCameraImpulse()
    //{
    //    if (impulseSource != null)
    //    {
    //        Debug.Log("카메라 임펄스 발생");
    //        impulseSource.GenerateImpulse();
    //    }
    //    else

    //    {
    //        Debug.LogWarning("ImpulseSource가 연결이 안 되어있습니다.");
    //    }
    //}
}