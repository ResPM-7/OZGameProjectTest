using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [Header("상점 UI 요소")]
    [SerializeField] private GameObject shopUI;

    // 플레이어 인벤토리 참조용
    [SerializeField] private PlayerInventory playerInventory;


    // 상점 열기/닫기
    public void ToggleShop()
    {
        bool isOpen = !shopUI.activeSelf;
        shopUI.SetActive(isOpen);

        Time.timeScale = isOpen ? 0f : 1f;
    }

    //아이템 구매 시도
    public void BuyItem(Item itemToBuy)
    {
        // 재화가 충분한가?
        if (CoinManager.instance.HasEnoughCoins(itemToBuy.price))
        {
            bool isAcquired = playerInventory.AcquireItem(itemToBuy.Clone()); //카피로 넘겨주기

            if (isAcquired)
            {
                // 인벤토리에 공간이 있어서 획득에 성공했다면 비로소 돈을 차감합니다.
                CoinManager.instance.UpdateCoins(itemToBuy.price);
                ShowSystemMessage($"{itemToBuy.itemName}을(를) 구매했습니다.");
            }
            else
            {
                // 돈은 있지만 인벤토리가 꽉 찬 경우
                ShowSystemMessage("인벤토리에 빈 공간이 없습니다.");
            }
        }
        else
        {
            //재화 부족 시 실패 피드백 제공
            ShowSystemMessage("코인이 부족합니다!");
        }
    }

    private void ShowSystemMessage(string message)
    {
        Debug.Log($"[상점 알림] {message}");
    }
}