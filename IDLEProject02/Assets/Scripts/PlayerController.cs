using System;
using UnityEngine;


[RequireComponent (typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public enum ANIME_STATE
    {
        PlayerIDLE, PlayerClear, PlayerOver, PlayerRun, PlayerJump
    }

    Animator animator;
    public string current = ""; //���� �������� �ִϸ��̼�
    public string previous = ""; //������ �������̴� �ִϸ��̼�


    Rigidbody2D rigid;
    float axisH = 0.0f;
    public float speed = 3.0f;
    public float jump = 9.0f;
    public LayerMask groundLayer;
    bool goJump = false;
    bool onGround = false;

    public static string state = "playing"; //������ ����(�÷��� ��)
   
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        state = "playing";
    }

    
    void Update()
    {
        if(state != "playing")
        {
            return;
        }

        axisH = Input.GetAxisRaw("Horizontal"); //���� �̵�

        if (axisH > 0.0f)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if(axisH < 0.0f)
        {
            transform.localScale = new Vector2(-1, 1);
            //���Ͱ� -�� ������ �Ǹ� �¿� ����)
        }

        if(Input.GetButtonDown("Jump"))
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
        //up�� Vector ���� (0, 1, 0) �Դϴ�.
        //(�÷��̾��� ���� pivot�� bottom)

        //���� ���� �ְų� �Ǵ� �ӵ��� 0�� �ƴ� ���
        if(onGround || axisH != 0)
        {
            rigid.linearVelocity = new Vector2(speed * axisH, rigid.linearVelocityY);
        }

        //���� ���� �ִ� ���¿��� ���� Ű�� ���� ��Ȳ
        if(onGround && goJump)
        {
            Vector2 jumpPw = new Vector2(0, jump); //�÷��̾ ���� ���� ��ġ��ŭ ���� ����
            rigid.AddForce(jumpPw, ForceMode2D.Impulse); //�ش� ��ġ�� ���� ���մϴ�.
            goJump = false;
        }

        //
        if(onGround)
        {
            if(axisH == 0)
            {
                current = Enum.GetName(typeof(ANIME_STATE), 0);
                //Enum.GetName(typeof(enum��), ��);
                //�ش� enum�� �ִ� �� ���� �̸��� ������ ���
            }

            else
            {
                current = Enum.GetName(typeof(ANIME_STATE), 3);
            }
        }
        else
        {
            //������ ���
            current = Enum.GetName(typeof(ANIME_STATE), 4);
        }

        //������ ����� ������ ��ǰ� �ٸ� ���(�ִϸ��̼��� �ٲ� ���)
        if(current != previous)
        {
            previous = current; //���� ���ۿ� ���� ���̺�
            animator.Play(current); //������ ��� �÷���
        }
    }

    private void Jump()
    {
        goJump = true; //�÷��� Ű�� �۾�
    }
        
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Goal")
        {
            Goal();
        }
        else if(collision.gameObject.tag == "Dead")
        {
            GameOver();
        }        
    }

    private void Goal()
    {
        animator.Play(Enum.GetName(typeof (ANIME_STATE), 1));
        state = "gameclear";
        GameStop();
    }

    public void GameOver()
    {
        animator.Play(Enum.GetName(typeof(ANIME_STATE), 2));
        state = "gameover";
        GameStop();
        GetComponent<CapsuleCollider2D>().enabled = false;
        //���� �÷��̾ ������ �ִ� �ݶ��̴��� Ȱ��ȭ�� ��Ȱ��ȭ�� �����մϴ�.(���̻��� �浹�� �߻����� �ʵ���)
        rigid.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        //���� ��¦ �پ������ ����
    }

    private void GameStop()
    {
        //var rigid = GetComponent<Rigidbody2D>();
        rigid.linearVelocity = new Vector2(0, 0); //�ӷ��� 0���� ���� �������� ���ϰ� ��
    }
}
