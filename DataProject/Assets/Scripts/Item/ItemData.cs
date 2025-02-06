using UnityEngine;

public class ItemData
{
    public string ItemName;
    [TextArea] public string ItemDescription;

    public ItemData(string itemName, string itemDescription)
    {
        ItemName = itemName;
        ItemDescription = itemDescription;
    }
}
