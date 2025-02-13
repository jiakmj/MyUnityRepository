using System;
using UnityEngine;


public class DEAD
{    
    public event EventHandler Die;
    int hp = 0;

    public void OnDie()
    {
        HpPlus();

        if (hp == 0)
        {
            hp = 0;
            Die(this, EventArgs.Empty);
        }
        else
        {
            Debug.Log($"HP�� �������� �ֽ��ϴ�. [{hp} / 5]");
        }

    }

    public void HpPlus() => hp--;
}

public interface CountAble
{
    int Count { get; set; }
    void CountPlus();
}

public interface UseAble
{
    void Use();
}

class Player : DEAD, CountAble, UseAble
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
            if (count > 5)
            {
                Debug.Log("����� 5���� �ִ��Դϴ�.");
                count = 5;
            }
            count = value;
        }
    }

    public string Name { get => name; set => name = value; }

    public void CountPlus() => Count++;

    public void Use()
    {
        Debug.Log($"{Name}�� ����߽��ϴ�.");
        Count--;
    }
}
    

public class Practice : MonoBehaviour
{
    Player player = new Player();
    DEAD dead = new DEAD();

    void Start()
    {
        player.Count = 5;
        player.Name = "����";
        player.CountPlus();
        player.Use();

        dead.Die += new EventHandler(PlayerKill);

        for (int i = 5; i > 0; i--)
        {
            dead.OnDie();

        }

    }

    private void PlayerKill(object sender, EventArgs e)
    {
        Debug.Log("�÷��̾ �׾����ϴ�.");
    }


}
