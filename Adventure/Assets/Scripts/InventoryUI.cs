using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    public GameObject itemListPanel; // ������ ����� ���� �г�
    public GameObject itemSlotPrefab; // ���� ������(ItemSlot.cs����)
    //public EquipmentSlot weaponSlot;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateInventoryUI() // �κ��丮 UI ����
    {
        foreach (Transform child in itemListPanel.transform) // ���� ���� ����
        {
            Destroy(child.gameObject);
        }

        foreach (InventoryItem item in InventoryManager.Instance.inventory) // �κ��丮 �������� �������� ����
        {
            GameObject slotObj = Instantiate(itemSlotPrefab, itemListPanel.transform);
            ItemSlot slot = slotObj.GetComponent<ItemSlot>();
            slot.SetUp(item);
        }
    }
}
