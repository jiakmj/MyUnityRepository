using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public float speed = 3.0f; //이동속도
    public float jump = 3.0f; //점프
    float axisH = 0.0f;
    Rigidbody2D rigid;    
    bool isJump = false;
    bool onGround = false;
    public LayerMask groundLayer;

    public enum ANIME_STATE
    {
        PlayerIDLE, PlayerBlink, PlayerRun, PlayerJump
    }

    Animator animator;
    public static string state;
    //public List<string> anime_list = new List<string>
    //{"PlayerBlink", "PlayerRun", "PlayerJump"};
    public string current = " ";
    public string previous = " ";

    //낚시구현시작
    int curFish;
    int pullGauge;

    void Start()
    {
        state = "playing";
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        previous = Enum.GetName(typeof(ANIME_STATE), 0);
    }

    void Update()
    {
        if (state != "playing")
        {
            return;
        }

        axisH = Input.GetAxisRaw("Horizontal");               


        if (axisH > 0.0f)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (axisH < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1);
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }               

    }
    private void FixedUpdate()
    {
        if (state != "playing")
        {
            return;
        }

        onGround = Physics2D.Linecast(transform.position, transform.position - (transform.up * 0.1f), groundLayer);
        //지정한 두 점을 연결하는 가상의 선에 게임 오브젝트가 접촉하는지 조사해 true 또는 false로 return 해주는 함수
        //up은 Vector 기준(0, 1, 0) 입니다.
        //(플레이어의 현재 pivot은 bottom)

        //지면 위에 있거나 또는 속도가 0이 아닌 경우
        if (onGround || axisH != 0)
        {
            rigid.linearVelocity = new Vector2(speed * axisH, rigid.linearVelocityY);
        }

        //지면 위에 있는 상태에서 점프 키가 눌린 상황
        if (onGround && isJump)
        {
            Vector2 jumpPw = new Vector2(0, jump); //플레이어가 가진 점프 수치만큼 벡터 설계
            rigid.AddForce(jumpPw, ForceMode2D.Impulse); //해당 위치로 힘을 가합니다.
            isJump = false;
        }

        if (onGround)
        {
            if (axisH == 0)
            {
                current = Enum.GetName(typeof(ANIME_STATE), 1);

            }

            else
            {
                current = Enum.GetName(typeof(ANIME_STATE), 2);
            }
        }
        else
        {
            //공중인 경우
            current = Enum.GetName(typeof(ANIME_STATE), 3);
        }
        if (current != previous)
        {
            previous = current; //이전 동작에 대한 세이브
            animator.Play(current); //현재의 모션 플레이
        }
        
    }

    private void Jump()
    {
        isJump = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
        {
            isJump = false;
        }
    }

}
