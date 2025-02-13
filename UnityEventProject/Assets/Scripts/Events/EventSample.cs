using System;
using UnityEngine;
//event
//��ü�� �۾� ������ �˸��� �޼���
//�̺�Ʈ�� �̺�Ʈ �����ڿ��� Ư�� �۾��� �˷��ִ� ���

//Event Handler : �̺�Ʈ�� �߻��� �� � ����� �������� �������ִ� ��

//�̺�Ʈ�� += �����ڸ� ���� �̺�Ʈ �ڵ鷯�� �̺�Ʈ�� �߰��� �� ������,
//-= �����ڸ� ���� �̺�Ʈ �ڵ鷯�� �����ϴ� ���� �����մϴ�.

//�ϳ��� �̺�Ʈ���� ���� ���� �̺�Ʈ �ڵ鷯�� �߰��ϴ� ���� �����ϸ�,
//�߰��� �ڵ鷯���� ���������� ȣ��˴ϴ�.

class SpecialPortalEvent
{
    public event EventHandler Kill;

    int count = 0;

    public void OnKill()
    {
        CountPlus(); //ų �� ����

        if (count == 5) //ī��尡 5�� �Ǹ�
        {
            count = 0; //����
            Kill(this, EventArgs.Empty); //�̺�Ʈ �ڵ鷯���� ȣ���մϴ�.
        }        
        else
        {   //�Ϲ����� ����� �̺�Ʈ ���࿡ ���� ���
            Debug.Log($"ų �̺�Ʈ�� ���� ���Դϴ�! [{count} / 5] ");
        }
    }

    public void CountPlus() => count++;

}


public class EventSample : MonoBehaviour
{
    //1. �̺�Ʈ ����
    //�̺�Ʈ��  ������ = new �̺�Ʈ��();
    SpecialPortalEvent specialPortalEvent = new SpecialPortalEvent();



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //2. �̺�Ʈ �ڵ鷯�� �̺�Ʈ ����
        specialPortalEvent.Kill += new EventHandler(MonsterKill);

        for (int i = 0; i < 5; i++)
        {
            specialPortalEvent.OnKill(); //3. �̺�Ʈ ������ ���� ��� ����
        }
    }
    
    //4. �̺�Ʈ�� �߻����� �� ����� �ڵ�
    private void MonsterKill(object sender, EventArgs e)
    {
        Debug.Log("��Ż�� ���Ƚ��ϴ�.");
    }
}
