using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;


public class EnemyManager : MonoBehaviour
{
    public EEnemyState currentState = EEnemyState.Idle;

    private float enemyHp = 100.0f; //�� hp
    private float moveSpeed = 2.0f;
    private float runSpeed = 5.0f;
    public float attackRange = 1.0f; //���� ����
    public float attackDelay = 2.0f; //���� ������
    public float attackDamage = 10.0f; //���ݷ�
    public Transform[] patrolPoints; //���� ��� ����
    private int currentPoint = 0; //���� ���� ��� ���� �ε���
    private float trackingRange = 3.0f; //���� ���� ����
    private float distanceTotarget; //Target���� �Ÿ� ��� ��
    public float idleTime = 2.0f; //�� ���� ��ȯ �� ��� �ð�
    private Coroutine stateRoutine; //���� �������� �ڷ�ƾ�� �����ϴ� ����

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
        currentState = EEnemyState.Idle; //ó���� ����� ���߿� �����ϸ� ��
        rb = GetComponent<Rigidbody>();
        if (rb == null) //��������
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
        if (isJumping) return; //�������� �� �ٸ� �ൿ ���� �����

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
        Debug.Log(gameObject.name + "�����");
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
        Debug.Log(gameObject.name + "������");
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
        Debug.Log(gameObject.name + "�÷��̾� ������");

        while (currentState == EEnemyState.Chase)
        {
            float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);

            //�÷��̾�� �̵�
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
        Debug.Log(gameObject.name + "������");
        
        agent.isStopped = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackDelay);

        while (currentState == EEnemyState.Attack)
        {
            float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);

            if (distance > attackRange)
            {
                Debug.Log("����>���� ����");
                agent.isStopped = false;
                ChangeState(EEnemyState.Chase);
                yield break;
            }

            PlayerManager playerHp = PlayerManager.Instance.GetComponent<PlayerManager>();
            if (playerHp != null)
            {
                playerHp.TakeDamage(attackDamage);
                Debug.Log($"�÷��̾�� {attackDamage} ���ظ� ����");
            }
        }        
    }

    private IEnumerator Die()
    {
        Debug.Log(gameObject.name + "���");
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(3.0f);
        gameObject.SetActive(false);
        yield break;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject + " : " + damage + "����� ���� ");
        animator.SetTrigger("Damage");
        enemyHp -= damage;

        if (enemyHp <= 0)
        {
            ChangeState(EEnemyState.Die);
        }
        else
        {
            ChangeState(EEnemyState.Chase); //�������ų� �Ѿƿ��ų�... ���ϴ°�
        }
        //���� �����̳� �����ΰ� �˾Ƽ� 
    }

    private IEnumerator JumpAcrossLink()
    {
        Debug.Log(gameObject.name + "�� ����");

        isJumping = true;

        agent.isStopped = true; //������Ʈ ����

        //NavMeshLink�� ���۰� �� ��ǥ�� ������
        OffMeshLinkData linkData = agent.currentOffMeshLinkData;
        Vector3 startPos = linkData.startPos;
        Vector3 endPos = linkData.endPos;

        //���� ��� ���(�������� �׸��� ����)
        float elsapsedTime = 0;
        while (elsapsedTime < jumpDuration)
        {
            float t = elsapsedTime / jumpDuration;
            Vector3 currentPosition = Vector3.Lerp(startPos, endPos, t);
            currentPosition.y += Mathf.Sin(t * Mathf.PI) * jumpHeight; //������ ��� �̰� Ȱ���ϸ� ����ź�� ���� �� ������
            transform.position = currentPosition;

            elsapsedTime += Time.deltaTime;
            yield return null;
        }

        //�������� ��ġ
        transform.position = endPos;
        //NavMeshAgent ��� �簳
        agent.CompleteOffMeshLink();
        agent.isStopped = false;
        isJumping = false;
    }

}

public enum EEnemyState
{
    Idle, //�⺻
    Patrol, //����
    Chase, //����
    Attack, //����      
    Die, //���
}
