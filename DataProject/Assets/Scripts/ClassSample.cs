using UnityEngine;
//�⺻���� ����Ƽ class ������ ����

class Unit
{
    //Ŭ�������� �ش� Ŭ������ ���� ��ü�� ������ �ۼ��մϴ�.
    public string unit_name;

    //Ŭ������ �ش� Ŭ������ ���� ��ü�� ����, ����� �ۼ��� �� �ֽ��ϴ�.(�޼ҵ�)
    public static void UnitAction()
    {
        Debug.Log("������ �����մϴ�.");
    }

    public void Cry()
    {
        Debug.Log("������ ���¢�����ϴ�.");
    }
}


public class ClassSample : MonoBehaviour
{
    Unit unit; //Unit Ŭ���� �������θ��� unit ��ü(object)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unit.unit_name = "MiniGin";
        //Ŭ����������.�ʵ带 ���� Ŭ������ ������ �ִ� �ʵ�(����)�� ����� �� �ֽ��ϴ�.

        unit.Cry();
        //�ν��Ͻ���.�޼ҵ�()�� ���� Ŭ������ ������ �ִ� �޼ҵ�(�Լ�)�� ����� �� �ֽ��ϴ�.

        //Ŭ���� ������� ���� ��Ī
        //��ü: ���� ������, Ŭ������ �� ��ü�� ����� ���� ���ø� ����(���� �ص� ��ü)
        //ex) Animal cat; (��ü)
        //    Animal cat = new Animal(); (��ü)

        //���۷���: ��ü�� �޸� �󿡼��� ��ġ�� ����Ű�� ��
        //Ŭ������ �迭, �������̽� � �ش��մϴ�.

        //�ν��Ͻ�: ��ü�� ����Ʈ����� ��üȭ�� ��(new�� ���ؼ� ����������� �ν��Ͻ�)
        //ex) Animal cat = new Animal();

        Unit.UnitAction();
        //static�� ���� ������ �Լ��� ��ü�� �������� �ʰ� Ŭ�������� �ٷ� �� ����� ������ ����մϴ�.

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
