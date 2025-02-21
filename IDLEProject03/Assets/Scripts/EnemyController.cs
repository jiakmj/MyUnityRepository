using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int hp = 3;
    public float speed = 0.5f;
    public float pattern_distance = 4.0f; //������ ���� �Ÿ�, �ʹ� �ָ� �������� ����
    public List<string> anime_list = new List<string>
    {"EnemyIdle", "EnemyDown", "EnemyUp", "EnemyLeft", "EnemyRight", "EnemyDead"};

    string current = "";
    string previous = "";
    float h, v; //������� �����࿡ ���� ��    
    Rigidbody2D rbody; //������Ʈ
    bool isActive = false;
    public int arrangId = 0; //�ĺ���

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //1. �÷��̾ ������ ã���ϴ�.
        var player = GameObject.FindGameObjectWithTag("Player");

        //�÷��̾ �����Ѵٸ�
        if (player != null)
        {
            //���Ͱ� Ȱ��ȭ ������ ��
            if (isActive)
            {
                float dx = player.transform.position.x - transform.position.x;
                float dy = player.transform.position.y - transform.position.y;
                float radian = Mathf.Atan2(dy, dx);
                float degree = radian * Mathf.Rad2Deg;

                if (degree > -45.0f && degree <= 45.0f)
                {
                    current = anime_list[4]; //right
                }
                else if (degree > 45.0f && degree <= 135.0f)
                {
                    current = anime_list[2]; //up
                }
                else if (degree > -135.0f && degree <= -45.0f)                    
                {
                    current = anime_list[1]; //down
                }
                else
                {
                    current = anime_list[3]; //left
                }

                h = Mathf.Cos(radian) * speed;
                v = Mathf.Sin(radian) * speed;
            }
            else //������ ������� ���� ������ ��
            {
                //�÷��̾���� �Ÿ��� ����
                float distance = Vector2.Distance(transform.position, player.transform.position);

                //���� ���� ���� ���� ��� Ȱ��ȭ ó��
                if (distance <= pattern_distance)
                {
                    isActive = true;
                }
            }
        }
        else if (isActive)
        {
            isActive = false;
            rbody.linearVelocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        //Ȱ��ȭ �����̸鼭 ü���� �������� ���
        if(isActive && hp > 0)
        {
            //����� ��ǥ�� �̵�
            rbody.linearVelocity = new Vector2(h, v);

            //�ִϸ��̼��� �޶�����, ���� �� �÷��� �����մϴ�.
            if(current != previous)
            {
                previous = current;
                var animator = GetComponent<Animator>();
                animator.Play(current);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Arrow")
        {
            hp--;

            if (hp <= 0)
            {
                //���̻��� �浹�� �߻����� �ʽ��ϴ�.
                GetComponent<CircleCollider2D>().enabled = false;
                //�ӷ��� 0�� �˴ϴ�.
                rbody.linearVelocity = new Vector2(0, 0);

                //������ ���� �ִϸ��̼��� ó���մϴ�.
                var animator = GetComponent<Animator>();
                animator.Play(anime_list[5]);

                //������Ʈ�� �ı��մϴ�.
                Destroy(gameObject, 0.5f);
            }
        }
    }
}
