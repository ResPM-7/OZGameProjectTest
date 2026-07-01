using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip Instance;

    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        HideTooltip();
    }

    public void ShowTooltip(Item item)
    {
        tooltipObject.SetActive(true);

        itemNameText.text = item.itemName;
        itemDescText.text = item.itemDesc;
    }

    public void HideTooltip()
    {
        tooltipObject.SetActive(false);
    }
}
