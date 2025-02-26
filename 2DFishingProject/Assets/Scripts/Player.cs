using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using System.Linq.Expressions;

public class Player : MonoBehaviour
{
    public float speed = 3.0f; //이동속도
    float h, v; //가로축 세로축
    public float z = -90.0f; //회전 각
    Rigidbody2D rigid;
    bool isMove = false;
    public int power = 1;

    Animator animator;
    public static string state;
    public List<string> anime_list = new List<string>
    {"PlayerDown", "PlayerUp", "PlayerLeft", "PlayerRight"};
    string current = " ";
    string previous = " ";

    void Start()
    {
        state = "playing";
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        previous = anime_list[0];
    }

    void Update()
    {
        if (state != "playing")
        {
            return;
        }

        rigid.linearVelocity = new Vector2(h, v) * speed;

        if (isMove == false)
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
        }

        Vector2 from = transform.position;

        Vector2 to = new Vector2(from.x + h, from.y + v);

        z = GetAngle(from, to);

        if (z >= -45 && z < 45)
        {
            //오른쪽
            current = anime_list[3];
        }
        else if (z >= 45 && z <= 135)
        {
            //위쪽
            current = anime_list[1];
        }
        else if (z >= -135 && z <= -45)
        {
            //아래쪽
            current = anime_list[0];
        }
        else
        {
            //왼쪽
            current = anime_list[2];
        }

        if (current != previous)
        {
            previous = current;
            animator.Play(current);

        }

    }

    private float GetAngle(Vector2 from, Vector2 to)
    {
        float angle;

        if (h != 0 || v != 0)
        {
            float dx = to.x - from.x;
            float dy = to.y - from.y;

            float radian = Mathf.Atan2(dy, dx);

            angle = radian * Mathf.Rad2Deg;

        }
        else
        {
            angle = z;
        }
        return angle;
    }

    void Attack()
    {
        
    }

    //void Pull()
    //{     

    //    if (pullGauge) >= fishHealth[curFish])
    //    {
    //        Catch(curFish);
    //    }
    //    else
    //    {
    //        pullGauge += strengthByLevel[strengthLevel];
    //        //애니메이션추가
    //    }      
                        
    //}

    //void Catch(int curFish)
    //{
    //    Instantiate(curFish, 9);

    //    pullGauge = 0;

    //    SetFishNum();
        
    //}
   
    
}
