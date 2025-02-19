using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    Animator animator;

    //�Ϲ����� ��ġ�� ������ ü���̳� ���ݷ� ���� ��ġ�� �ſ� ���� �� ����.
    public double hp;
    public double atk;
    public float attack_speed; //���� �ӵ��� �ʹ� ������ ������ �� �� ����.

    protected float attack_range = 3.0f; //���� ����
    protected float target_range = 5.0f; //Ÿ�Ͽ� ���� ����

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

 
    protected void SetMotionChange(string motion_name, bool param)
    {
        animator.SetBool(motion_name, param);
    }

    //animation event�� ���� ȣ��� �Լ�
    protected virtual void Thrown()
    {
        Debug.Log("�߻�!");
    }




    protected Transform target; //Ÿ�ٿ� ���� ����(��ġ)

    //Ÿ���� ã�� ���
    protected void TargetSearch<T>(T[] targets) where T : Component
    {
        var units = targets; //���޹��� ���� ���� �Ҵ�
        Transform closet = null; //���� ����� ���� ���� null
        float max_distance = target_range; //�ִ� �Ÿ� == Ÿ���� �Ÿ�

        //Ÿ���� ��ü�� ������� �Ÿ��� üũ�մϴ�.

        foreach(var unit in units)
        {
            //������ �Ÿ� üũ
            float distance = Vector3.Distance(transform.position, unit.transform.position);

            //Ÿ�� �Ÿ����� ������ ���� ����� ��
            if(distance < max_distance)
            {
                closet = unit.transform;
                max_distance = distance;
            }
        }
        //Ÿ�� ����
        target = closet;

        //Ÿ���� �����մϴ�.
        if (target != null)
            transform.LookAt(target.position);
        
    }

}
