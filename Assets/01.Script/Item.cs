using UnityEngine;

public enum ItemType
{
    Equipment,
    Consumables,
    Etc
}

[System.Serializable]
public class Item
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImg;
    public string itemDesc;
    public int amount;

    public bool Use(PlayerCharacter player)
    {
        if (itemType == ItemType.Consumables)
        {
            player.Heal(amount);
            return true;
        }
        return false;
    }
}
