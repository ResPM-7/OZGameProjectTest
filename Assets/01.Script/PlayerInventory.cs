using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private Transform slotParent;
    
    private InventorySlot[] slots;

    public Item[] items;

    private void Start()
    {
        slots = slotParent.GetComponentsInChildren<InventorySlot>();
        items = new Item[slots.Length];
    }

    public bool AcquireItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                slots[i].AddItem(item);
                return true;
            }
        }
        return false;
    }
}
