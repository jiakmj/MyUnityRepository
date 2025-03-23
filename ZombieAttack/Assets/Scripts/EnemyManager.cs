using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;


public class EnemyManager : MonoBehaviour
{
    public EEnemyState currentState = EEnemyState.Idle;

    private float enemyHp = 100.0f; //적 hp
    private float moveSpeed = 2.0f;
    private float runSpeed = 5.0f;
    public float attackRange = 1.0f; //공격 범위
    public float attackDelay = 2.0f; //공격 딜레이
    public float attackDamage = 10.0f; //공격력
    public Transform[] patrolPoints; //순찰 경로 지점
    private int currentPoint = 0; //현재 순찰 경로 지점 인덱스
    private float trackingRange = 3.0f; //추적 범위 설정
    private float distanceTotarget; //Target과의 거리 계산 값
    public float idleTime = 2.0f; //각 상태 전환 후 대기 시간
    private Coroutine stateRoutine; //현재 실행중인 코루틴을 저장하는 변수

    private Animator animator;

    private NavMeshAgent agent;

    private bool isJumping = false;
    private Rigidbody rb;
    public float jumpHeight = 2.0f;
    public float jumpDuration = 1.0f;
    private NavMeshLink[] navMeshLinks;


    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentState = EEnemyState.Idle; //처음에 어떨지는 나중에 선택하면 됨
        rb = GetComponent<Rigidbody>();
        if (rb == null) //오류방지
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true;
        navMeshLinks = FindObjectsOfType<NavMeshLink>();
        if (currentState == EEnemyState.Idle)
        {
            StartCoroutine(Idle());
        }
        else if (currentState == EEnemyState.Patrol)
        {
            StartCoroutine(Patrol());
        }
    }
    
    void Update()
    {
        distanceTotarget = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);
    }

    public void ChangeState(EEnemyState newState)
    {
        if (isJumping) return; //점프했을 때 다른 행동 하지 말라고

        if (stateRoutine != null)
        {
            StopCoroutine(stateRoutine);
        }

        currentState = newState;

        switch (currentState)
        {
            case EEnemyState.Idle:
                stateRoutine = StartCoroutine(Idle());
                break;
            case EEnemyState.Patrol:
                stateRoutine = StartCoroutine(Patrol());
                break;
            case EEnemyState.Chase:
                stateRoutine = StartCoroutine(Chase());
                break;
            case EEnemyState.Attack:
                stateRoutine = StartCoroutine(Attack());
                break;
            case EEnemyState.Die:
                stateRoutine = StartCoroutine(Die());
                break;
        }
    }

    private IEnumerator Idle()
    {
        Debug.Log(gameObject.name + "대기중");
        animator.Play("Idle");

        while (currentState == EEnemyState.Idle)
        {
            float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);

            if (distance < trackingRange)
            {
                ChangeState(EEnemyState.Chase);
            }
            else if (distance < attackRange)
            {
                ChangeState(EEnemyState.Attack);
            }
            yield return null;
        }
    }

    private IEnumerator Patrol()
    {
        Debug.Log(gameObject.name + "순찰중");
        animator.Play("Patrol");

        while (currentState == EEnemyState.Patrol)
        {
            if (patrolPoints.Length > 0)
            {
                animator.SetBool("isWalk", true);
                Transform targetPoint = patrolPoints[currentPoint];
                Vector3 direction = (targetPoint.position - transform.position).normalized;

                agent.speed = moveSpeed;
                agent.isStopped = false;
                agent.destination = PlayerManager.Instance.transform.position;

                //transform.position += direction * moveSpeed * Time.deltaTime;
                //transform.LookAt(targetPoint.transform);

                if (agent.isOnOffMeshLink)
                {
                    StartCoroutine(JumpAcrossLink());
                }

                if (Vector3.Distance(transform.position, targetPoint.position) < 0.3f)
                {
                    currentPoint = (currentPoint + 1) % patrolPoints.Length;
                }

                float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);
                if (distance < trackingRange)
                {
                    ChangeState(EEnemyState.Chase);
                }
                else if (distance < attackRange)
                {
                    ChangeState(EEnemyState.Attack);
                }
                else if (enemyHp <= 30)
                {
                    agent.speed = runSpeed;
                    animator.SetBool("isWalk", false);
                    animator.SetBool("isRun", true);
                }
            }
            yield return null;
        }
    }

    private IEnumerator Chase()
    {
        Debug.Log(gameObject.name + "플레이어 추적중");

        while (currentState == EEnemyState.Chase)
        {
            float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);

            //플레이어에게 이동
            //Vector3 direction = (PlayerManager.Instance.transform.position - transform.position).normalized;

            agent.speed = moveSpeed;
            agent.isStopped = false;
            agent.destination = PlayerManager.Instance.transform.position;

            //transform.position += direction * moveSpeed * Time.deltaTime;
            //transform.LookAt(target.transform);
            animator.SetBool("isWalk", true);

            if (agent.isOnOffMeshLink)
            {
                StartCoroutine(JumpAcrossLink());
            }

            if (distance > trackingRange * 2)
            {
                ChangeState(EEnemyState.Patrol);
            }
            else if (distance < attackRange)
            {
                ChangeState(EEnemyState.Attack);
            }
            else if (enemyHp <= 30)
            {
                agent.speed = runSpeed;
                animator.SetBool("isWalk", false);
                animator.SetBool("isRun", true);                
            }
            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        Debug.Log(gameObject.name + "공격중");
        
        agent.isStopped = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackDelay);

        while (currentState == EEnemyState.Attack)
        {
            float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);

            if (distance > attackRange)
            {
                Debug.Log("공격>추적 변함");
                agent.isStopped = false;
                ChangeState(EEnemyState.Chase);
                yield break;
            }

            PlayerManager playerHp = PlayerManager.Instance.GetComponent<PlayerManager>();
            if (playerHp != null)
            {
                playerHp.TakeDamage(attackDamage);
                Debug.Log($"플레이어에게 {attackDamage} 피해를 입힘");
            }
        }        
    }

    private IEnumerator Die()
    {
        Debug.Log(gameObject.name + "사망");
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(3.0f);
        gameObject.SetActive(false);
        yield break;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject + " : " + damage + "대미지 받음 ");
        animator.SetTrigger("Damage");
        enemyHp -= damage;

        if (enemyHp <= 0)
        {
            ChangeState(EEnemyState.Die);
        }
        else
        {
            ChangeState(EEnemyState.Chase); //도망가거나 쫓아오거나... 원하는거
        }
        //무적 상태이나 상세적인건 알아서 
    }

    private IEnumerator JumpAcrossLink()
    {
        Debug.Log(gameObject.name + "적 점프");

        isJumping = true;

        agent.isStopped = true; //에이전트 멈춤

        //NavMeshLink의 시작과 끝 좌표를 가져옴
        OffMeshLinkData linkData = agent.currentOffMeshLinkData;
        Vector3 startPos = linkData.startPos;
        Vector3 endPos = linkData.endPos;

        //점프 경로 계산(포물선을 그리며 점프)
        float elsapsedTime = 0;
        while (elsapsedTime < jumpDuration)
        {
            float t = elsapsedTime / jumpDuration;
            Vector3 currentPosition = Vector3.Lerp(startPos, endPos, t);
            currentPosition.y += Mathf.Sin(t * Mathf.PI) * jumpHeight; //포물선 경로 이거 활용하면 수류탄도 만들 수 있을듯
            transform.position = currentPosition;

            elsapsedTime += Time.deltaTime;
            yield return null;
        }

        //도착점에 위치
        transform.position = endPos;
        //NavMeshAgent 경로 재개
        agent.CompleteOffMeshLink();
        agent.isStopped = false;
        isJumping = false;
    }

}

public enum EEnemyState
{
    Idle, //기본
    Patrol, //순찰
    Chase, //추적
    Attack, //공격      
    Die, //사망
}
