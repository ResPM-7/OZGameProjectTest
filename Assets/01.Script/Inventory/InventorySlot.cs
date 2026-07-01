using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;

    private Item item;

    private int slotIndex;
    private PlayerInventory inventory;

    public void SetupSlot(int index, PlayerInventory inven)
    {
        slotIndex = index;
        inventory = inven;
        ClearSlot();
    }

    public void AddItem(Item newItem)
    {
        item = newItem;

        iconImage.sprite = item.itemImg;
        iconImage.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;

        iconImage.sprite = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            inventory.UseItem(slotIndex);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            ItemTooltip.Instance.ShowTooltip(item);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            ItemTooltip.Instance.HideTooltip();
        }
    }
}
