using System;
using UnityEngine;


public class Monster : Character
{
    public float monster_speed;
    public float rate = 0.5f;

    protected override void Start()
    {
        base.Start();
    }


    //Action �׽�Ʈ
    public void MonsterSample()
    {
        Debug.Log("���Ͱ� �����Ǿ����ϴ�.");
    }

    void Update()
    {
        //���� �������� �ü� ����
        transform.LookAt(Vector3.zero);

        //���� ����
        float target_distance = Vector3.Distance(transform.position, Vector3.zero);

        if (target_distance <= rate) //���� �Ÿ��� ��������� �̵� ����
        {
            SetMotionChange("isMOVE", false);
        }
        else //�Ϲ����� ��쿡�� �������� �����մϴ�.
        {
            //�������� ������ �ӵ���ŭ ������ �̵��մϴ�.
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, Time.deltaTime * monster_speed);

            SetMotionChange("isMOVE", true);
        }
    }
}
