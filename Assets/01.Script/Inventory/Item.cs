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
    public int itemID;
    public string itemName;
    public Sprite itemImg;
    public string itemDesc;//섫명창
    public int amount; //데미지나 포션회복 등등
    public int price;

    public int count = 1; //몇개나 겹쳐있는지 확인
    public int maxStack = 99; //최대 수량

    public Item(Item other)
    {
        this.itemType = other.itemType;
        this.itemID = other.itemID;
        this.itemName = other.itemName;
        this.itemImg = other.itemImg;
        this.itemDesc = other.itemDesc;
        this.amount = other.amount;
        this.price = other.price; 
        this.count = other.count;
        this.maxStack = other.maxStack;
    }

    public bool Use(PlayerCharacter player)
    {
        if (itemType == ItemType.Consumables)
        {
            player.Heal(amount);
            return true;
        }
        return false;
    }

    public Item Clone()
    {
        return new Item(this);
    }
}
