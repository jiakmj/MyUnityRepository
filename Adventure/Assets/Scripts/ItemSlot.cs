using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public InventoryItem itemData; // ���Կ� ��� ������
    public Image iconImage; // ������ ������ �̹���
    private Transform originalParent; // ���� �θ� ��ġ ���

    public void SetUp(InventoryItem item)
    {
        itemData = item;
        iconImage.sprite = item.icon;
        iconImage.enabled = true;
    }

    public void OnBeginDrag(PointerEventData eventData) // �巡�� ���� �� ȣ��
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root); // UI �� ���� �ø�
    }

    public void OnDrag(PointerEventData eventData) // �巡�� �� ���콺 ����ٴ�
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) // �巡�� ������ ��
    {
        if(eventData.pointerEnter != null) // ��� ��ġ�� ��񽽷����� Ȯ��
        {
            EquipmentSlot slot = eventData.pointerEnter.GetComponent<EquipmentSlot>();
            if (slot != null)
            {
                slot.Equals(itemData); // �������
                Destroy(gameObject); // �κ��丮 ���� ����
                return;
            }            
        }

        // ���� �� ���� ��ġ��
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
    }
}
