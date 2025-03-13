using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public EZombieState currentState = EZombieState.Idle;
    public Transform target; //Ÿ�� = �÷��̾ �ǹ�
    public float attackRange = 1.0f; //���� ����
    public float attackDelay = 2.0f; //���� ������
    private float nextAttackTime = 0.0f; //���� ���� �ð�����
    public Transform[] patrolPoints; //���� ��� ������
    private int currentPoint = 0; //���� ���� ��� ���� �ε���
    public float moveSpeed = 2.0f; //�̵��ӵ�
    private float trackingRange = 3.0f; //���� ���� ����
    private bool isAttack = false; //���� ����
    private float evadeRange = 5.0f; //���� ���� ȸ�� �Ÿ�
    private float zombieHp = 10.0f; //������ hp
    private float distanceTotarget; //Target���� �Ÿ� ��� ��
    private bool isWaiting = false; //���� ��ȯ �� ��� ���� ����
    public float idleTime = 2.0f; //�� ���� ��ȯ �� ��� �ð�


    void Start()
    {
        
    }

    void Update()
    {
        distanceTotarget = Vector3.Distance(transform.position, target.position); //������ �� �϶�� �ڵ带 ���� �� ����
        Debug.Log("distanceTotarget : " + distanceTotarget);

        if (distanceTotarget < trackingRange)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.LookAt(transform.position);
            Debug.Log("Player ����");
        }
        else if (distanceTotarget < attackRange)
        {
            Debug.Log("Player ����");
        }
        else
        {
            if (patrolPoints.Length > 0)
            {
                Debug.Log("������");
                Transform targetPoint = patrolPoints[currentPoint];
                //���� ������ ������
                Vector3 direction = (targetPoint.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
                transform.LookAt(transform.position);

                if (Vector3.Distance(transform.position, targetPoint.position) < 0.3f)
                {
                    currentPoint = (currentPoint + 1) % patrolPoints.Length;
                }
            }
        }
        
    }

}
public enum EZombieState
{
    Patro, //����
    Chase, //�߰�
    Attack, //����
    Evade, //����
    Damage, //�ǰ�
    Idle, //�⺻
    Die, //���
}
