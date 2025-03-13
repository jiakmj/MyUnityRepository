using UnityEngine;

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
    private float trackingRange = 3.0f; //추적 범위 설정
    private bool isAttack = false; //공격 상태
    private float evadeRange = 5.0f; //도망 상태 회피 거리
    private float zombieHp = 10.0f; //좀비의 hp
    private float distanceTotarget; //Target과의 거리 계산 값
    private bool isWaiting = false; //상태 전환 후 대기 상태 여부
    public float idleTime = 2.0f; //각 상태 전환 후 대기 시간


    void Start()
    {
        
    }

    void Update()
    {
        distanceTotarget = Vector3.Distance(transform.position, target.position); //가까우면 뭘 하라는 코드를 만들 수 있음
        Debug.Log("distanceTotarget : " + distanceTotarget);

        if (distanceTotarget < trackingRange)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
            transform.LookAt(transform.position);
            Debug.Log("Player 추적");
        }
        else if (distanceTotarget < attackRange)
        {
            Debug.Log("Player 공격");
        }
        else
        {
            if (patrolPoints.Length > 0)
            {
                Debug.Log("순찰중");
                Transform targetPoint = patrolPoints[currentPoint];
                //이하 위에랑 동일함
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
    Patro, //순찰
    Chase, //추격
    Attack, //공격
    Evade, //도망
    Damage, //피격
    Idle, //기본
    Die, //사망
}
