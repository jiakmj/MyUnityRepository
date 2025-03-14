using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ZombieManager : MonoBehaviour
{
    public EZombieState currentState = EZombieState.Idle;
    public Transform target; //타켓 = 플레이어를 의미
    public float attackRange = 1.0f; //공격 범위
    public float attackDelay = 2.0f; //공격 딜레이
    private float nextAttackTime = 0.0f; //다음 공격 시간관리
    public Transform[] patrolPoints; //순찰 경로 지점들
    private int currentPoint = 0; //현재 순찰 경로 지점 인덱스
    public float moveSpeed = 2.0f; //이동속도
    private bool isWalk = false;
    private float trackingRange = 3.0f; //추적 범위 설정
    private bool isAttack = false; //공격 상태
    private float evadeRange = 5.0f; //도망 상태 회피 거리
    private float zombieHp = 10.0f; //좀비의 hp
    private float distanceTotarget; //Target과의 거리 계산 값
    private bool isWaiting = false; //상태 전환 후 대기 상태 여부
    public float idleTime = 2.0f; //각 상태 전환 후 대기 시간
    private Coroutine stateRoutine; //현재 실행중인 코루틴을 저장하는 변수

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentState = EZombieState.Idle; //처음에 어떨지는 나중에 선택하면 됨
        if (currentState == EZombieState.Idle)
        {
            StartCoroutine(Idle());
        }
        else if (currentState == EZombieState.Patrol)
        {
            StartCoroutine(Patrol());
        }
    }

    void Update()
    {
        distanceTotarget = Vector3.Distance(transform.position, target.position); //가까우면 뭘 하라는 코드를 만들 수 있음
        //Debug.Log("distanceTotarget : " + distanceTotarget);        
    }

    public void ChangeState(EZombieState newState)
    {
        if (stateRoutine != null)
        {
            StopCoroutine(stateRoutine);
        }

        currentState = newState;

        switch (currentState)
        {
            case EZombieState.Idle:
                stateRoutine = StartCoroutine(Idle());
                break;
            case EZombieState.Patrol:
                stateRoutine = StartCoroutine(Patrol());
                break;
            case EZombieState.Chase:
                stateRoutine = StartCoroutine(Chase());
                break;
            case EZombieState.Attack:
                stateRoutine = StartCoroutine(Attack());
                break;
            case EZombieState.Evade:
                stateRoutine = StartCoroutine(Evade());
                break;
            case EZombieState.Die:
                stateRoutine = StartCoroutine(Die());
                break;
        }
    }
    private IEnumerator Idle()
    {
        Debug.Log(gameObject.name + "대기중");
        animator.Play("Idle");

        while (currentState == EZombieState.Idle)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            
            if (distance < trackingRange)
            {
                ChangeState(EZombieState.Chase);
            }
            else if (distance < attackRange)
            {
                ChangeState(EZombieState.Attack);
            }
            yield return null;
        }
    }

    private IEnumerator Patrol()
    {
        Debug.Log(gameObject.name + "순찰중");
        animator.Play("Patrol");

        while (currentState == EZombieState.Patrol)
        {
            if (patrolPoints.Length > 0)
            {
                animator.SetBool("isWalk", true);
                Transform targetPoint = patrolPoints[currentPoint];
                Vector3 direction = (targetPoint.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                transform.LookAt(targetPoint.transform);

                if (Vector3.Distance(transform.position, targetPoint.position) < 0.3f)
                {
                    currentPoint = (currentPoint + 1) % patrolPoints.Length;
                }

                float distance = Vector3.Distance(transform.position, target.position);
                if (distance < trackingRange)
                {
                    ChangeState(EZombieState.Chase);
                }
                else if (distance < attackRange)
                {
                    ChangeState(EZombieState.Attack);
                }
            }
            yield return null;
        }
    }

    private IEnumerator Chase()
    {
        Debug.Log(gameObject.name + "플레이어 추적중");

        while (currentState == EZombieState.Chase)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.LookAt(target.transform);
            animator.SetBool("isWalk", true);

            if (distance < attackRange)
            {
                ChangeState(EZombieState.Attack);
            }
            //else if (distance < evadeRange)
            //{
            //    ChangeState(EZombieState.Idle);
            //}
            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        Debug.Log(gameObject.name + "공격중");        
        transform.LookAt(target.position);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDelay);

        float distance = Vector3.Distance(transform.position, target.position);
        if (distance > attackRange)
        {
            Debug.Log("공격>추적 변함");
            ChangeState(EZombieState.Chase);
        }
        else
        {
            ChangeState(EZombieState.Attack);
        }
    }

    private IEnumerator Evade()
    {
        Debug.Log(gameObject.name + "도망가는중");
        animator.SetBool("isWalk", true);            
        Vector3 evadeDirection = (transform.position - target.position).normalized;
        float evadeTime = 3.0f;
        float timer = 0.0f;        

        while (currentState == EZombieState.Evade && timer < evadeTime)
        {
            Quaternion targetRotation = Quaternion.LookRotation(evadeDirection); //LookAt과 Quaternion 차이는 별로 없음 방법이 다를 뿐 상황에 따라 씀
            transform.rotation = targetRotation;

            transform.position += evadeDirection * moveSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        ChangeState(EZombieState.Idle);
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
        zombieHp -= damage;

        if (zombieHp <= 0)
        {
            ChangeState(EZombieState.Die);
        }
        else
        {
            ChangeState(EZombieState.Chase); //도망가거나 쫓아오거나... 원하는거
        }
        //무적 상태이나 상세적인건 알아서 
    }

}

public enum EZombieState
{
    Idle, //기본
    Patrol, //순찰
    Chase, //추적
    Attack, //공격
    Evade, //도망   
    Die, //사망
}
