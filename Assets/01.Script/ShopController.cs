using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopController : Singleton<ShopController>
{
    [Header("UI 연결")]
    [SerializeField] private CoinPresenter coinPresenter; //코인 MVP패턴연결
    [SerializeField] private GameObject shopPanel; // 상점 전체 UI 패널
    [SerializeField] private TextMeshProUGUI systemMessageText; // 중앙 경고 메시지 텍스트

    private PlayerInventory playerInventory;

    private WaitForSecondsRealtime textOn = new WaitForSecondsRealtime(2f);

    private void Start()
    {
        if (shopPanel != null) shopPanel.SetActive(false);
        if (systemMessageText != null) systemMessageText.gameObject.SetActive(false);

        playerInventory = GetComponent<PlayerInventory>();
    }

    public void ToggleShop()
    {
        bool isOpen = !shopPanel.activeSelf;
        shopPanel.SetActive(isOpen);

        // 상점이 열려있을 때는 시간을 멈춤
        Time.timeScale = isOpen ? 0f : 1f;

        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isOpen;
    }

    public void BuyItem(Item itemToBuy)
    {
        //코인이 충분한가?
        if (coinPresenter.Model.TryConsumeCoins(itemToBuy.price))
        {
            // 2. 인벤토리에 추가 시도
            bool isAcquired = playerInventory.AcquireItem(itemToBuy.Clone());

            if (isAcquired)
            {
                //돈차감
                ShowMessage($"{itemToBuy.itemName} 구매 완료");
            }
            else
            {
                coinPresenter.Model.AddCoins(itemToBuy.price);
                ShowMessage("인벤토리가 가득 찼습니다");
            }
        }
        else
        {
            //코인 부족 시 UI 피드백
            ShowMessage("코인이 부족합니다");
        }
    }

    // 화면 중앙에 메시지를 띄우고 2초 뒤에 사라지게 하는 코루틴
    private void ShowMessage(string msg)
    {
        if (systemMessageText == null) return;

        systemMessageText.text = msg;
        systemMessageText.gameObject.SetActive(true);

        StopAllCoroutines(); // 기존에 떠있던 메시지 타이머 초기화
        StartCoroutine(HideMessageRoutine());
    }

    private IEnumerator HideMessageRoutine()
    {
        yield return textOn;
        systemMessageText.gameObject.SetActive(false);
    }
}