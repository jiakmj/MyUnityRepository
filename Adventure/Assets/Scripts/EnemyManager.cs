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
    public float hp = 10.0f;
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
        if (monsterType == MonsterType.None || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && !isAttack)
        {
            if (stateType != StateType.Attack)
            {
                StopAllCoroutines();
                stateType = StateType.StrongAttack;
                StartCoroutine(AttackRoutine());

            }
            return;
        }
        if (distanceToPlayer <= chaseRange)
        {
            if (stateType != StateType.ChaseWalk && stateType != StateType.ChaseRun)
            {
                if (stateChangeRoutine != null)
                {
                    StopCoroutine(stateChangeRoutine);
                }
                int chaseType = Random.Range(0, 2);
                stateType = chaseType == 0 ? StateType.ChaseWalk : StateType.ChaseRun;
                animator.Play("SlimeMove");
                Debug.Log($"[상태 변환] 추적 상태 : {stateType}");                
            }

            Vector3 directiontoPlayer = (player.position - transform.position).normalized;
            float chaseSpeed = stateType == StateType.ChaseRun ? speed * 2 : speed;
            transform.position += directiontoPlayer * chaseSpeed * Time.deltaTime;
            return;
        }

        if ((stateType == StateType.ChaseWalk || stateType == StateType.ChaseRun) && distanceToPlayer > chaseRange)
        {
            Debug.Log("[상태 복귀] 추적 종료");
            stateType = StateType.Idle;
            animator.Play("SlimeIdle1");
            if (stateChangeRoutine == null)
            {
                stateChangeRoutine = StartCoroutine(RandomStateChanger());
            }
            if (stateType == StateType.Attack) return;
            PatrolMovement();

        }

        //    if (monsterType != MonsterType.None)
        //    {
        //        if (currentGroundType == GroundType.UpGround && stateType == StateType.PatrolWalk)
        //        {
        //            if (transform.position.y > startPos.y + maxDistance)
        //            {
        //                direction = -1;                   
        //            }
        //            else if (transform.position.y < startPos.y - maxDistance)
        //            {
        //                direction = 1;
        //            }
        //            transform.position += new Vector3(0, speed * direction * Time.deltaTime, 0);
        //        }
        //        else if (currentGroundType == GroundType.UpGround && stateType == StateType.PatrolRun)
        //        {
        //            if (transform.position.y > startPos.y + maxDistance)
        //            {
        //                direction = -1;
        //                speed *= 2;
        //            }
        //            else if (transform.position.y < startPos.y - maxDistance)
        //            {
        //                direction = 1;
        //                speed *= 2;
        //            }
        //            transform.position += new Vector3(0, speed * direction * Time.deltaTime, 0);
        //        }
        //        else
        //        {
        //            if (transform.position.x > startPos.x + maxDistance)
        //            {
        //                direction = -1;
        //                GetComponent<SpriteRenderer>().flipX = true;
        //            }
        //            else if (transform.position.x < startPos.x - maxDistance)
        //            {
        //                direction = 1;
        //                GetComponent<SpriteRenderer>().flipX = false;
        //            }
        //            transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
        //        }
    
    }

    private void PatrolMovement()
    {
        {
            if (currentGroundType == GroundType.UpGround)
            {
                if (stateType == StateType.PatrolWalk || stateType == StateType.PatrolRun)
                {
                    if (transform.position.y > startPos.y + maxDistance)
                    {
                        direction = -1;
                    }
                    else if (transform.position.y <  startPos.y - maxDistance)
                    {
                        direction = 1;
                    }
                    float movespeed = stateType == StateType.PatrolRun ? speed * 2 : speed;
                    transform.position += new Vector3(0, movespeed * direction * Time.deltaTime, 0);
                }
            }
            else
            {
                if(transform.position.x > startPos.x + maxDistance)
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
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerAttack"))
        {
            StartCoroutine(ChangeColorTemporatily());
        }
    }

    IEnumerator ChangeColorTemporatily()
    {
        SoundManager.Instance.PlaySFX(SFXType.DamageSound);
        animator.Play("SlimeHit");
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
        Debug.Log("[공격 상태] 공격 시작");
        animator.Play("SlimeHit");
        yield return new WaitForSeconds(1.0f);
        isAttack = false;
        Debug.Log("[공격 상태] 공격 종료, 상태 복귀");

        stateChangeRoutine = StartCoroutine(RandomStateChanger());
    }


}
