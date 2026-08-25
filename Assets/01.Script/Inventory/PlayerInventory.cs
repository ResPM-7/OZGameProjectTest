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
        player = GetComponent<PlayerCharacter>();
        items = new Item[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetupSlot(i, this);
        }
    }

    public bool AcquireItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].itemID == item.itemID && items[i].count < items[i].maxStack)
            {
                items[i].count += item.count;
                slots[i].UpdateSlotCount();
                return true;
            }
        }

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

    public void UseItem(int index)
    {
        if (items[index] != null)
        {
            bool isUsed = items[index].Use(player);

            if(isUsed)
            {
                items[index].count--;

                if (items[index].count <= 0)
                {
                    // 0개가 되면 슬롯을 완전히 비움
                    items[index] = null;
                    slots[index].ClearSlot();
                }
                else
                {
                    // 아직 남아있다면 숫자만 갱신
                    slots[index].UpdateSlotCount();
                }
            }
        }
    }
}
