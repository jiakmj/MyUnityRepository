using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;


public class PlayerController : MonoBehaviour
{
    public static int hp = 3;
    public static string state; //�÷��� ����
    bool inDamage = false; //�������� �޴� �������� Ȯ��

    public float speed = 3.0f;
    public List<string> anime_list = new List<string>
    {"PlayerDown", "PlayerUp", "PlayerLeft", "PlayerRight", "PlayerDead"};

    string current = " ";
    string previous = "";
    float h, v; //������� �����࿡ ���� ��
    public float z = -90.0f; //ȸ�� ��
    Rigidbody2D rbody; //������Ʈ
    bool isMove = false; //�����̴� �������� Ȯ��
    Animator animator;

    void Start()
    {
        state = "playing";
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        previous = anime_list[0]; //ó�� ������ �Ʒ��� ���� �ֵ���
    }

    
    void Update()
    {
        //�÷��� ���°� �ƴϰų�, �������� �޴� ��Ȳ���� ������ ó������ �ʵ��� �����մϴ�.
        if(state != "playing" || inDamage)
        {
            return;
        }        

        if(isMove == false)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }

        Vector2 from = transform.position;

        Vector2 to = new Vector2(from.x + h, from.y + v);

        z = GetAngle(from, to); //Ű �Է��� ���� ���� ���� �̵� ������ ����� �Լ� GetAngle

        //������ ���� ����� �ִϸ��̼� ����
        if(z >= -45 && z < 45)
        {
            //������
            current = anime_list[3];
        }
        else if(z >= 45 && z <= 135)
        {
            //����
            current = anime_list[1];
        }
        else if (z >= -135 && z <= -45)
        {
            //�Ʒ���
            current = anime_list[0];
        }
        else
        {        
            //����
            current = anime_list[2];
        }

        if(current != previous)
        {
            previous = current;
            animator.Play(current);

        }                               
        
    }

    private void FixedUpdate()
    {
        //�÷��� ���°� �ƴϰų�, �������� �޴� ��Ȳ���� ������ ó������ �ʵ��� �����մϴ�.
        if (state != "playing" || inDamage)
        {
            return;
        }

        //�������� �޴� ���
        if (inDamage)
        {
            float value = Mathf.Sin(Time.time * 50);

            if (value > 0)
            {
                //�̹��� ǥ��
                GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                //�̹��� ��Ȱ��ȭ
                GetComponent<SpriteRenderer>().enabled = false;
            }
            //������ �޴� ������ ���� �Ұ�
            return;
        }
        rbody.linearVelocity = new Vector2(h, v) * speed;
    }

    //�÷��̾�� �������� �浹�� �߻��� ���
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GetDamage(collision.gameObject);

        }
    }

    //�����κ��� �޴� �������� ���� ����
    private void GetDamage(GameObject enemy)
    {
        if (state == "playing")
        {
            hp--;
            if (hp > 0)
            {
                //�̵� ����
                rbody.linearVelocity = new Vector2(0, 0);

                Vector3 to = (transform.position - enemy.transform.position).normalized;

                //�� ��ǥ���� �� 4ĭ���� �־�������
                rbody.AddForce(new Vector2(to.x * 4, to.y * 4), ForceMode2D.Impulse);

                inDamage = true;

                //������ ���� �� ȣ���� �Լ� ó��
                Invoke("OnDamageExit", 0.25f);
            }
            else
            {
                //ü���� 0�� �Ǹ� ���� ����
                GameOver();
            }
        }
    }

    private void OnDamageExit()
    {
        inDamage = false; //������ �� �޴� ���·� ��ȯ
        GetComponent<SpriteRenderer>().enabled = true; //�̹��� �ٽ� Ű��
    }
    private void GameOver()
    {
        state = "gameover";
        GetComponent<CircleCollider2D>().enabled = false;
        rbody.linearVelocity = new Vector2(0, 0);
        rbody.gravityScale = 1;
        rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        GetComponent<Animator>().Play(anime_list[4]);
        Destroy(gameObject, 1.0f);
    }

    /// <summary>
    /// from���� to ������ ������ ����ϴ� �Լ�
    /// </summary>
    /// <param name="from">���� ��ġ(A ����)</param>
    /// <param name="to">������ ��ġ(B ����)</param>   
    private float GetAngle(Vector2 from, Vector2 to)
    {
        float angle;

        if (h != 0 || v != 0)
        {
            //from�� to�� ���̸� ����մϴ�.
            float dx = to.x - from.x;
            float dy = to.y - from.y;

            float radian = Mathf.Atan2(dy, dx);
            //Atan ���� ���� x��ǥ�� 0�� ��� ����� �� �˴ϴ�.
            
            angle = radian * Mathf.Rad2Deg;
        }
        else
        {
            angle = z;
        }
        return angle;
    }
}
