using UnityEngine;

//����Ƽ�� �������̽�(interface)
//�������� Ư¡�� ���� ���� ���� �� ȿ�����Դϴ�.
//�Լ��� ������Ƽ ���� ���Ǹ� ���� ���� ������ �� �ֵ��� �����ִ� ����Դϴ�.
//�������̽��� ��ø� �ϱ� ������, ����ϱ� ���ؼ��� �ݵ�� ����� ���� �籸������ �����մϴ�.

public interface ICountAble
{
    //int a = 0; �������̽� �������� ���� �����մϴ�.
    int Count { get; set; }

    void CountPlus();

}

public interface IUseAble
{
    void Use();
}



//�������̽��� ���ó�� ����� �� �ֽ��ϴ�.
//�������̽��� ��� ���� ����� �����մϴ�.
class Potion : Item, ICountAble, IUseAble
{
    public int count;
    private string name;

    public int Count
    {
        get
        {
            return count;
        }
        set
        {
            if (count > 99)
            {
                Debug.Log("count�� 99���� �ִ��Դϴ�.");
                count = 99;
            }
            count = value;
        }
    }

    public string Name { get => name; set => name = value; }

    public void CountPlus() => Count++;
    //{
    //    Count++;
    //}

    public void Use() // => Count--;
    {
        Debug.Log($"{Name}�� ����߽��ϴ�.");
        Count--;
    }
}



public class InterfaceSample : MonoBehaviour
{
    Potion potion = new Potion();

    //ICountAble incountable = new Potion();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�ϼ��� Ŭ���� ����ϱ�
        potion.Count = 99;
        potion.Name = "���� ����";
        potion.CountPlus();
        potion.Use();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
