using UnityEngine;

public enum ItemType
{
    Weapon,
    Armor,
    Potion,
    KeyItem1,
    KeyItem2,
}

public class InventoryItem : MonoBehaviour
{
    public string itemName; // ������ �̸�
    public string description; // ������ ����
    public ItemType itemtype; // ������ Ÿ��
    public Sprite icon; //������ �̹���

    public int attackPower;
    public int defensePower;
    public int healAmount;

    private void SetDefaultByType()
    {
        switch (itemtype)
        {
            case ItemType.Weapon:
                itemName = "�⺻ ��";
                description = "���ݷ� + 5";
                attackPower = 5;
                icon = Resources.Load<Sprite>("Icons/weapon_icon");
                break;
            case ItemType.Armor:
                itemName = "���� ��";
                description = "���� + 5";
                defensePower = 3;
                icon = Resources.Load<Sprite>("Icons/armor_icon");
                break;
            case ItemType.Potion:
                itemName = "ȸ�� ����";
                description = "ü�� + 20";
                healAmount = 20;
                icon = Resources.Load<Sprite>("Icons/potion_icon");
                break;
            case ItemType.KeyItem1:
                itemName = "��1, ��2 ����";                
                icon = Resources.Load<Sprite>("Icons/key1_icon");
                break;
            case ItemType.KeyItem2:
                itemName = "��3, ��4 ����";                
                icon = Resources.Load<Sprite>("Icons/key2_icon");
                break;
        }        
    }
}
