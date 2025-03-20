using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;


public class ZombieManager : MonoBehaviour
{
    public EZombieState currentState = EZombieState.Idle;

    //public Transform target; //Ÿ�� = �÷��̾ �ǹ� //�÷��̾� �̱���Ἥ����
    public float attackRange = 1.0f; //���� ����
    public float attackDelay = 2.0f; //���� ������
    private float nextAttackTime = 0.0f; //���� ���� �ð�����
    public Transform[] patrolPoints; //���� ��� ������
    private int currentPoint = 0; //���� ���� ��� ���� �ε���
    public float moveSpeed = 2.0f; //�̵��ӵ�
    private bool isWalk = false;
    private float trackingRange = 3.0f; //���� ���� ����
    private bool isAttack = false; //���� ����
    private float evadeRange = 5.0f; //���� ���� ȸ�� �Ÿ�
    private float zombieHp = 100.0f; //������ hp
    private float distanceTotarget; //Target���� �Ÿ� ��� ��
    private bool isWaiting = false; //���� ��ȯ �� ��� ���� ����
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
        currentState = EZombieState.Idle; //ó���� ����� ���߿� �����ϸ� ��
        rb = GetComponent<Rigidbody>(); 
        if (rb == null) //��������
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.isKinematic = true;
        navMeshLinks = FindObjectsOfType<NavMeshLink>();
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
        distanceTotarget = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position); //������ �� �϶�� �ڵ带 ���� �� ����
        //Debug.Log("distanceTotarget : " + distanceTotarget);        
    }

    public void ChangeState(EZombieState newState)
    {
        if (isJumping) return; //�������� �� �ٸ� �ൿ ���� �����

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
        Debug.Log(gameObject.name + "�����");
        animator.Play("Idle");

        while (currentState == EZombieState.Idle)
        {
            float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);
            
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
        Debug.Log(gameObject.name + "������");
        animator.Play("Patrol");

        while (currentState == EZombieState.Patrol)
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
        Debug.Log(gameObject.name + "�÷��̾� ������");

        while (currentState == EZombieState.Chase)
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
                ChangeState(EZombieState.Patrol);
            }
            else if (distance < attackRange)
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
        Debug.Log(gameObject.name + "������");

        agent.destination = PlayerManager.Instance.transform.position;
        //transform.LookAt(target.position);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackDelay);

        float distance = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);
        if (distance > attackRange)
        {
            Debug.Log("����>���� ����");
            ChangeState(EZombieState.Chase);
        }
        else
        {
            ChangeState(EZombieState.Attack);
        }
    }

    private IEnumerator Evade()
    {
        Debug.Log(gameObject.name + "����������");
        animator.SetBool("isWalk", true);            
        Vector3 evadeDirection = (transform.position - PlayerManager.Instance.transform.position).normalized;
        float evadeTime = 3.0f;
        float timer = 0.0f;        

        while (currentState == EZombieState.Evade && timer < evadeTime)
        {
            Quaternion targetRotation = Quaternion.LookRotation(evadeDirection); //LookAt�� Quaternion ���̴� ���� ���� ����� �ٸ� �� ��Ȳ�� ���� ��
            transform.rotation = targetRotation;

            transform.position += evadeDirection * moveSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
        ChangeState(EZombieState.Idle);
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
        zombieHp -= damage;

        if (zombieHp <= 0)
        {
            ChangeState(EZombieState.Die);
        }
        else
        {
            ChangeState(EZombieState.Chase); //�������ų� �Ѿƿ��ų�... ���ϴ°�
        }
        //���� �����̳� �����ΰ� �˾Ƽ� 
    }

    private IEnumerator JumpAcrossLink()
    {
        Debug.Log(gameObject.name + "���� ����");

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

public enum EZombieState
{
    Idle, //�⺻
    Patrol, //����
    Chase, //����
    Attack, //����
    Evade, //����   
    Die, //���
}
