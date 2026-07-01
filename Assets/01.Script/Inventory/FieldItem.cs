using UnityEngine;

public class FieldItem : MonoBehaviour
{
    [SerializeField] private int targetItemID;

    private Item itemData;

    private void Start()
    {
        foreach(Item dbItem in ItemDateBase.instance.itemDB)
        {
            if(dbItem.itemID == targetItemID)
            {
                itemData = dbItem;
                break;
            }
        }
        if (itemData != null)
        {
            Debug.Log("아이템 로그에 없음");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();

            if (inventory != null)
            {
                bool isLooted = inventory.AcquireItem(itemData);

                if(isLooted)
                {
                    string poolKey = gameObject.name.Replace("(Clone)", "");
                    ObjectPoolManager.Instance.ReturnObject(poolKey, gameObject);
                }
            }
        }
    }
}
