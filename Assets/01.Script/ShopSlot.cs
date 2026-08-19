using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ShopSlot : MonoBehaviour
{
    [Header("슬롯 데이터")]
    [SerializeField] private int targetItemID; //ID 번호
    private Item itemData; //DB에서 아이템 정보 담아두는 변수

    [Header("UI 연결")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button bnuyButton;

    private void Awake()
    {
        itemData = ItemDateBase.instance.GetItemByID(targetItemID);
        
    }

    private void Start()
    {

        //슬롯 초기화
        if (itemData != null)
        {
            SetupSlot(itemData);
        }

        //버튼 클릭 이벤트 코드로 연결
        if (bnuyButton != null)
        {
            bnuyButton.onClick.AddListener(OnBuyButtonClicked);
        }
    }

    // 아이템 데이터를 받아와서 UI에 덮어씌우는 함수
    public void SetupSlot(Item newItem)
    {
        itemData = newItem;

        if (itemIcon != null) itemIcon.sprite = itemData.itemImg;
        if (priceText != null) priceText.text = itemData.price.ToString();
    }

    // 버튼을 클릭했을 때 실행될 함수
    private void OnBuyButtonClicked()
    {
        if (itemData != null)
        {
            //싱글톤을 호출하여 구매 실행
            ShopManager.instance.BuyItem(itemData);
        }
    }
}