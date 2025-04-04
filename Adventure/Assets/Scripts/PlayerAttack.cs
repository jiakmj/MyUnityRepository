using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private Animator animator;
    public List<GameObject> attackObjList = new List<GameObject>();

    private bool isAttacking = false;

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;
    private Vector3 originalPos;

    [Header("�ִϸ��̼� ���� �̸�")]
    public string attackStateName = "Attack";

    //[Header("ī�޶� ����ũ ����")]
    //public CinemachineImpulseSource impulseSource;

    private void Start()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        animator = GetComponent<Animator>();

        if (Camera.main != null )
        {
            originalPos = Camera.main.transform.localPosition;
        }
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
        //float shakeDuration = 0.1f;
        //float shakeMagnitude = 0.1f;
        StartCoroutine(Shake(shakeDuration, shakeMagnitude));
        //GenerateCameraImpulse();
        ParticleManager.Instance.ParticlePlay(ParticleType.PlayerAttack, transform.position, new Vector3(3, 3, 3));
        yield return null; //�������� ���� �Ϻη� ���� ���� �����ӱ��� ��ٸ��� �ҷ���
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(attackStateName)) //attack �ִϸ��̼��� �ϰ� ������
        {
            float animationLength = stateInfo.length; //�ִϸ��̼� ��� ���̸�ŭ ��ٸ�
            yield return new WaitForSeconds(animationLength);
        }
        else 
        {
            yield return new WaitForSeconds(0.5f);
        }

        isAttacking = false; //��� ������ false�� ����
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

    private IEnumerator Shake(float duration, float magnitude)
    {
        originalPos = Camera.main.transform.localPosition;
        Camera.main.GetComponent<CinemachineBrain>().enabled = false;
        if (Camera.main == null)
        {
            yield return null;
        }

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, originalPos.y + y, -10);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
    }

    //private void GenerateCameraImpulse()
    //{
    //    if (impulseSource != null)
    //    {
    //        Debug.Log("ī�޶� ���޽� �߻�");
    //        impulseSource.GenerateImpulse();
    //    }
    //    else
    //    {
    //        Debug.LogWarning("ImpulseSource�� ������ �� �Ǿ��ֽ��ϴ�.");
    //    }
    //}
}
