using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public float speed = 3.0f; //�̵��ӵ�
    public float jump = 3.0f; //����
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

    //���ñ�������
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
        //������ �� ���� �����ϴ� ������ ���� ���� ������Ʈ�� �����ϴ��� ������ true �Ǵ� false�� return ���ִ� �Լ�
        //up�� Vector ����(0, 1, 0) �Դϴ�.
        //(�÷��̾��� ���� pivot�� bottom)

        //���� ���� �ְų� �Ǵ� �ӵ��� 0�� �ƴ� ���
        if (onGround || axisH != 0)
        {
            rigid.linearVelocity = new Vector2(speed * axisH, rigid.linearVelocityY);
        }

        //���� ���� �ִ� ���¿��� ���� Ű�� ���� ��Ȳ
        if (onGround && isJump)
        {
            Vector2 jumpPw = new Vector2(0, jump); //�÷��̾ ���� ���� ��ġ��ŭ ���� ����
            rigid.AddForce(jumpPw, ForceMode2D.Impulse); //�ش� ��ġ�� ���� ���մϴ�.
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
            //������ ���
            current = Enum.GetName(typeof(ANIME_STATE), 3);
        }
        if (current != previous)
        {
            previous = current; //���� ���ۿ� ���� ���̺�
            animator.Play(current); //������ ��� �÷���
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
