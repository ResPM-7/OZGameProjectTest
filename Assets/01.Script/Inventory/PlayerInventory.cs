using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform slotParent;

    [SerializeField] private InventorySlot[] slots;

    public Item[] items;

    private PlayerCharacter player;

    private void Start()
    {
        slots = slotParent.GetComponentsInChildren<InventorySlot>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        items = new Item[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetupSlot(i, this);
        }
    }

    public bool AcquireItem(Item item)
    {
        if (item.isStackable)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null && items[i].itemID == item.itemID)
                {
                    items[i].currentCount++;
                    slots[i].UpdateCountUI(items[i].currentCount);
                    return true;

                    
                }
            }
        }
        for(int i = 0; i<items.Length; i++)
        {
            if(items[i] == null)
            {
                Item newItem = new Item(item);

                items[i] = item;
                slots[i].AddItem(item);
                return true;
            }
        }

        return false;
    }

    public void UseItem(int index)
    {
        if (items[index] != null)
        {
            bool isUsed = items[index].Use(player);

            if (isUsed)
            {
                items[index] = null;
                slots[index].ClearSlot();
            }
        }
    }
}
