using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public enum MonsterType
{
    None,
    FlyingMonster,
    GroundMonster,
}

public enum StateType
{
    Idle, PatrolWalk, PatrolRun, ChaseWalk, ChaseRun, StrongAttack, Attack, 
}
public class EnemyManager : MonoBehaviour
{
    private Color originalColor;
    private Renderer objectRenderer;
    public float colorChangeDuration = 0.5f; 
    public float hp = 2.0f;
    public float damage = 1.0f;
    public float speed = 2.0f;
    public float maxDistance = 3.0f;
    private Vector3 startPos;
    private int direction = 1;
    public GroundType currentGroundType;
    public MonsterType monsterType = MonsterType.None;
    
    public StateType stateType = StateType.Idle;

    private Animator animator;
    public Transform player;
    private bool isWalk = false;
    private bool isJump = false;
    public float jumpHeight = 2.0f;
    public float jumpDuration = 1.0f;
    public bool isAttack = false;
    public float chaseRange = 5.0f;
    public float attackRange = 1.5f;
    public float attackCooldown = 3.0f;
    private float lastAttackTime = 0f;

    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    private float stateChangeInterval = 3.0f;
    private Coroutine stateChangeRoutine;


    void Start()
    {
        animator = GetComponent<Animator>();
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
        startPos = transform.position;
        int randomChoice = Random.Range(0, 1);

        if ( player == null )
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
        else
        {
            stateType = StateType.PatrolRun;
        }

        stateChangeRoutine = StartCoroutine(RandomStateChanger());
    }

    private void Update()
    {        
        if (monsterType == MonsterType.None || player == null || isAttack) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        MonsterAttack(distanceToPlayer);

        if (!isAttack)
        {
            MonsterChase(distanceToPlayer);
            MonsterIdleOrPatrol(distanceToPlayer);
        }
    }

    private void MonsterAttack(float distanceToPlayer)
    {
        if (distanceToPlayer <= attackRange && !isAttack && Time.time >= lastAttackTime + attackCooldown)
        {
            if (stateType != StateType.Attack)
            {
                isAttack = true;
                StopAllCoroutines();
                stateType = StateType.StrongAttack;
                StartCoroutine(AttackRoutine());
                lastAttackTime = Time.time;

            }
            return;
        }
    }

    private void MonsterChase(float distanceToPlayer)
    {
        if (stateType == StateType.StrongAttack || stateType == StateType.Attack)
            return;

        if (distanceToPlayer <= chaseRange)
        {
            Vector3 directiontoPlayer = (player.position - transform.position).normalized;
            float chaseSpeed = stateType == StateType.ChaseRun ? speed * 2 : speed;

            if (directiontoPlayer.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (directiontoPlayer.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }

            transform.position += directiontoPlayer * chaseSpeed * Time.deltaTime;
            animator.SetBool("isWalk", true);
            return;
        }
    }

    private void MonsterIdleOrPatrol(float distanceToPlayer)
    {
        if (stateType == StateType.StrongAttack || stateType == StateType.Attack)
            return;

        if ((stateType == StateType.ChaseWalk || stateType == StateType.ChaseRun) && distanceToPlayer > chaseRange)
        {
            Debug.Log("[상태 복귀] 추적 종료");
            stateType = StateType.Idle;
            animator.SetBool("isWalk", false);
            animator.Play("SlimeIdle1");
            if (stateChangeRoutine == null)
            {
                stateChangeRoutine = StartCoroutine(RandomStateChanger());
            }
            // if (stateType == StateType.Attack) return;
            PatrolMovement();
        }
    }

    private void PatrolMovement()
    {

        animator.SetBool("isWalk", true);
        if (currentGroundType == GroundType.UpGround)
        {
            if (stateType == StateType.PatrolWalk || stateType == StateType.PatrolRun)
            {
                if (transform.position.y > startPos.y + maxDistance)
                {
                    direction = -1;
                }
                else if (transform.position.y < startPos.y - maxDistance)
                {
                    direction = 1;
                }
                float movespeed = stateType == StateType.PatrolRun ? speed * 2 : speed;
                transform.position += new Vector3(0, movespeed * direction * Time.deltaTime, 0);
            }
        }
        else
        {
            if (transform.position.x > startPos.x + maxDistance)
            {
                direction = -1;
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (transform.position.x < startPos.x - maxDistance)
            {
                direction = 1;
                GetComponent<SpriteRenderer>().flipX = false;
            }

            transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            StartCoroutine(ChangeColorTemporatily());
        }
        //else if (collision.CompareTag("DeadZone"))
        //{
        //    SoundManager.Instance.PlaySFX(SFXType.DeadSound);
        //    // 사망
        //}
    }


    IEnumerator ChangeColorTemporatily()
    {
        SoundManager.Instance.PlaySFX(SFXType.DamageSound);
        animator.SetTrigger("Damage");
        objectRenderer.material.color = Color.red;
        yield return new WaitForSeconds(colorChangeDuration);
        objectRenderer.material.color = originalColor;
    }

    IEnumerator RandomStateChanger()
    {
        while(true)
        {
            yield return new WaitForSeconds(stateChangeInterval);
            int randomState = Random.Range(0, 3);
            stateType = (StateType)randomState;
            Debug.Log($"[랜덤 상태]현재 상태 : {stateType}");
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttack = true;
        yield return new WaitForSeconds(1f);
        if (player != null)
        {
            Vector3 dirToPlayer = player.position - transform.position;
            if (dirToPlayer.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        animator.SetTrigger("Attack");
               
        yield return new WaitForSeconds(5f); // 후딜

        isAttack = false;
        Debug.Log("[공격 상태] 공격 종료, 상태 복귀");

        if (stateChangeRoutine != null)
        {
            StopCoroutine(stateChangeRoutine);
        }

        stateType = StateType.Idle; 

        stateChangeRoutine = StartCoroutine(RandomStateChanger());
    }

    public void ApplyDamage()
    {
        Debug.Log($"[몬스터 공격] ApplyDamage() 호출됨. Time: {Time.time}");

        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= attackRange)
            {
                PlayerController pc = player.GetComponent<PlayerController>();
                if (pc != null && !pc.IsInvincible())
                {
                    pc.TriggerDamageEffects();
                }
            }
        }
    }

    public void TakeDamage(float amount)
    {
        StartCoroutine(CameraShakeManager.Instance.Shake(shakeDuration, shakeMagnitude));
        CameraShakeManager.Instance.GenerateCameraImpulse();

        hp -= amount;
        hp = Mathf.Max(hp, 0);
        Debug.Log($"[몬스터 피해] {amount} 데미지 입음 | 현재 체력: {hp}");

        ParticleManager.Instance.ParticlePlay(ParticleType.PlayerAttack, transform.position, Vector3.one * 2f);

        if (hp <= 0)
        {
            Die();
        }
        else
        {            
            StartCoroutine(ChangeColorTemporatily());
        }
    }

    private void Die()
    {
        Debug.Log("몬스터 사망");
        animator.ResetTrigger("Damage");
        animator.SetTrigger("Die");

        Destroy(gameObject, 1f);
    }
    /* 이펙트
     * if (spriteRenderer.flipX)
    attackEffect.transform.localScale = new Vector3(-1, 1, 1); // 왼쪽
else
    attackEffect.transform.localScale = new Vector3(1, 1, 1); // 오른쪽

     */

}
