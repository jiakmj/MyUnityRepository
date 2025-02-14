using UnityEngine;
//����Ƽ�� ������ ���� �ڵ�: Singleton

//���������� ���Ǵ� �����͸� �ϳ��� ��ä(�ν��Ͻ�)�� �������ڽ��ϴ�.

public class Tester : MonoBehaviour
{
    int point = 0;

    private void Start()
    {
        point = Singleton.Instance.point; //�̱��濡 �ִ� ������Ƽ
        Singleton.GetInstance().PointPlus(); //�Ǵ� �޼ҵ带 ���� Ŭ���� ������ ��ü�� �����ؼ� ��ü�� ������ �ִ� ������ ����� �� �ֽ��ϴ�.

        //�̱��� ������ ������ ������ ������ �ʿ� ���� �ٷ� �� ����� ����� �� ����.
        //��� �̱��� �������� ������ �ν��Ͻ��� �ʹ� ���� �����͸� �����ϴ� ����� ������ ��ư� �׽�Ʈ�� ��ٷο����ϴ�.

    }
}


public class Singleton : MonoBehaviour
{
    //1. �ν��Ͻ��̸鼭 �������� ������ �� �ְ� �����մϴ�.
    private static Singleton _instance;

    //2. Ŭ���� ���ο� ǥ���� ���� �����մϴ�.
    public int point = 0;

    public void PointPlus()
    {
        point++;
    }

    public void ViewPoint()
    {
        Debug.Log("���� ����Ʈ : " + point);
    }


    //�޼ҵ带 ���ؼ� ����
    public static Singleton GetInstance()
    {
        //���� ���� ����ִٸ�
        if (_instance == null)
        {
            //���Ӱ� �Ҵ��մϴ�.
            _instance = new Singleton();
        }
        //�Ϲ����� ����� ������ �ν��Ͻ��� return�մϴ�.
        return _instance;
    }

   //������Ƽ�� �����ϴ� �͵� ����
   //�ΰ� �� �ϳ� ��� ���� ��.
   public static Singleton Instance
    {
        get
        {   if (_instance == null)
            {
                _instance = new Singleton();
            }
            return _instance;
        }

    }



}
