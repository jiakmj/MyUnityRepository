using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<InventoryItem> inventory = new List<InventoryItem>(); // ������ ����Ʈ

    private string savePath => Application.persistentDataPath + "/SaveData.json"; // ���� ���� ���

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            LoadInventory(); // ���� ���� �� �κ��丮 �ҷ�����
        }
    }

    public void AddItem(InventoryItem item) // ������ �߰�
    {
        inventory.Add(item);
        InventoryUI.Instance.UpdateInventoryUI();
        SaveInventory(); // ����
    }

    public void SaveInventory() // �κ��丮 ����(Json ���)
    {
        string json = JsonUtility.ToJson(new InventorySaveWrapper(inventory), true);
        File.WriteAllText(savePath, json);
    }

    public void LoadInventory() // �κ��丮 �ҷ�����
    {
        if(File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            InventorySaveWrapper data = JsonUtility.FromJson<InventorySaveWrapper>(json);
            inventory = data.items;
        }
    }

    [System.Serializable]
    private class InventorySaveWrapper // Json ������ ���� ���δ� Ŭ����
    {
        public List<InventoryItem> items;

        public InventorySaveWrapper(List<InventoryItem> itemsList)
        {
            this.items = items;
        }
    }
}
