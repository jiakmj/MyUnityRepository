using UnityEngine;

public class Player : Character
{
    Vector3 start_pos;
    Quaternion rotation;



    protected override void Start()
    {
        //ĳ���� Ŭ������ Start ����
        base.Start();

        // # ���� �� ����
        start_pos = transform.position;
        rotation = transform.rotation;
    }

    

    void Update()
    {
        if(target == null)
        {
            //����� Ÿ�� ����
            TargetSearch(Spawner.monster_list.ToArray());
            //����Ʈ��.ToArray()�� ���� list -> array

            float pos = Vector3.Distance(transform.position, start_pos);
            if(pos > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, start_pos, Time.deltaTime * 2.0f);
                transform.LookAt(start_pos);
                SetMotionChange("isMOVE", true);
            }
            else
            {
                transform.rotation = rotation;
                SetMotionChange("isMOVE", false);
            }
            return; //�۾� ����
        }

        float distance = Vector3.Distance(transform.position, target.position);

        //Ÿ�� �������� �����鼭 ���� �������� ���� ���
        if(distance <= target_range && distance > attack_range)
        {
            SetMotionChange("isMOVE", true);
            //Ÿ�� �������� �̵�
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * 2.0f);
        }
        //���� ���� �ȿ� ���� ���
        else if(distance <= attack_range)
        {
            //���� �ڼ��� �Ѿ�ϴ�.
            SetMotionChange("isATTACK", true);
        }

    }
}
