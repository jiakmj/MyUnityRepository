using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

//����Ƽ���� �������ִ� ��������Ʈ
//1. using System; �ʿ�


public class UnityDelegateSample : MonoBehaviour
{
    //1) Action delegate
    //��ȯ Ÿ���� ���� ���� ����(void)�� �븮��
    //Action delegate��;
    Action action;

    //2. �Ű������� �ִ� ���
    //Action<T> delegate��;
    Action<int> action2;
    Action<string, int> action3;

    //2) Func delegate
    //��ȯ Ÿ���� ������ ������ �븮��
    Func<int> func01;

    Func<int, int, int> func02;
    //�Ű������� (int, int), ��ȯ Ÿ���� int
    Func<int, int, string> func03;
    //Func<�Ű�����, return>���·� �ۼ��մϴ�.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        action = Sample;
        action();
        action2 = Sample;
        action3 = Sample;

        //�븮���� ��� �ٷ� ����� �����ؼ� ����ϴ� �͵� �����մϴ�.
        func01 = () => 10; //���ٽ�ǥ����
        //func01 = Test; ���� �������� �����Ѱ� �ƴ�


        //����� ���
        //Func<T> �̸� = (�Ű����� �ۼ� ��ġ) => ��;
        Func<int> test = () => 25;

        //�Ű������� �����ϴ� ���
        func02 = (a, b) => a + b;

        //���� ������ ����� �ϴ� ���
        func02 = (a, b) =>
        {
            if ( a > b )
            {
                a = b;
            }
            return a + b;
        };

        func03 = (a, b) => "a + b";
    
    }

    int Test()
    {
        return 10;
    }

    //�����ε�(Overloading) ����
    //�Ϲ������� �Լ����� �����ϰ� �����մϴ�.
    //������ ���� ������ ��ų ��� ���� �̸��� �޼ҵ带 ����� �� �ֽ��ϴ�.

    //�Ű������� ����, Ÿ��, ������ �ٸ��ٸ� ���� �̸����� �޼ҵ� ���ǰ� �����մϴ�.

    //�����ε� ����� ���� �ణ�� ��Ȳ�� ���� �Ź� �޼ҵ��� �̸��� ������� �ʿ� ���� Ư�� ����� �����ϴ� ���� �̸��� �޼ҵ带 ����� �̸��� ������ �� �ֽ��ϴ�.

    //�������̵�(Overriding)
    //�θ� Ŭ�����κ��� ��� ���� �޼ҵ带 �ڽ� Ŭ�������� �������ϴ� ����
    //�������̽�, abstract class ��� ����(����)�� �Ǿ��ִ� �޼ҵ带 ���޹��� ����� ���������� �����ؾ� �մϴ�.

    //���� Ŭ�������� ���ϴ� ��ɿ� ���� ����
    //�������̽�, abstract class ��� �������� Ʋ�� ���� ������ �ڵ� ���� ����
    //�ϳ��� ��ü�� ���� ���¸� ǥ���ϴ� ������ �������� ȿ����
    
    public void Sample() { }   
    
    public void Sample(int a) { }

    public void Sample(int a, string s) { }

    public void Sample(string s, int a) { }

    public int Sample3() { return 0; }



}
