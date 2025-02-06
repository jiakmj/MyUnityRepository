using System.Collections.Generic;
using System;
using UnityEngine;
//1. �迭 / List ���·� ����Ǿ� �ִ� json ������ ����ϴ� ����
//2. Resources ������ �̿��� ������ �ε� ����� ����մϴ�.

//"item_name": "�ٳ���",
//      "item_count": 5

//���� �ȿ� �ִ� ������ �ϳ��� ���� ǥ��
[Serializable]
public class Item
{
    public string item_name; //json ���Ͽ��� ����ϰ� �ִ� �̸��� �״�� ����մϴ�.
    public int item_count;
}
//������ ���� ǥ��
[Serializable]
public class Inventory
{
    public List<Item> inventory; //json ���Ͽ��� ����ϰ� �ִ� �̸��� �״�� ����մϴ�.
    //public Item[] inventory; ����Ƽ������ �迭�� ����Ʈ�� ���� ���

}

public class JsonArraySample : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("item_inventory");

        Inventory inventory = JsonUtility.FromJson<Inventory>(textAsset.text);

        int total = 0; //������ ���� ����

        //foreach(Ÿ�� ���� in �迭/����Ʈ)
        //�迭�̳� ����Ʈ�� ������ �ִ� ������ ������ŭ �ݺ��� �����ϴ� ���� ����

        foreach (Item item in inventory.inventory)
        {
            total += item.item_count;
        }

        Debug.Log(total);

        //�迭�� ���
        //for (int i = 0; i < inventory.inventory.Count; i++)
        //{
        //    total += inventory.inventory[i].item_count;
        //}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
