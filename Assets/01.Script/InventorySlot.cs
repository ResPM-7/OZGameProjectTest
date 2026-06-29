using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;

    private Item item;


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
        iconImage.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item != null)
        {
            Debug.Log("아이템 클릭");
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            Debug.Log("아이템 위로 올라왔을경우 설명창");
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            Debug.Log("설명창 끄기");
        }
    }
}
