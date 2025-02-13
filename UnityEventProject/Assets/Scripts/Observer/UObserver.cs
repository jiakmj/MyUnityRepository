using UnityEngine;
//������ ����(Observer)
//�� ������Ʈ�� ���°� ������ �Ǹ� �� ��ü�� �����ϰ� �ִ� �ٸ� ��ü�鿡�� �ڵ����� ������ ���ŵǴ� ���� ���

//abstract class�� �������̽�ó�� �޼ҵ忡 ���� ������ ������ �� �ִ� Ŭ�����Դϴ�.

/// <summary>
/// �������� ���� ����, Ȱ���� �����ϱ� ���� �������̽�
/// </summary>
public interface ISubject
{
    //������ ���
    void Add(UObserver observer);
    //������ ����
    void Remove(UObserver observer);
    //������ ����
    void Notify();
}

public abstract class UObserver : MonoBehaviour
{
    public abstract void OnNotify();

    
}

public class Observer1 : UObserver
{
    public override void OnNotify()
    {
        Debug.Log("UOnserver action #1");        
    }

}
public class Observer2 : UObserver
{
    public override void OnNotify()
    {
        Debug.Log("UOnserver action #2");
    }

}