using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public string slotType; // ���Կ� ���� ������ ����
    public Image iconImage; // ���Կ� ǥ�õ� ������ ������
    private InventoryItem equippedItem; // ���� ������ ������

    public void Equip(InventoryItem item) // ������ ���� �Լ�
    {
        if(item.itemtype == ItemType.Weapon)
        {
            equippedItem = item;
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
        }
    }

    public InventoryItem GetEquippedItem() // ��Ȯ�� ������ ��ȯ
    {
        return equippedItem;
    }
}
