using System;

public class CoinModel
{
    // 순수 데이터
    public int CurrentCoins { get; private set; }

    // 데이터 변경 방송 채널
    public event Action<int> OnCoinChanged;

    public CoinModel(int initialCoins = 0)
    {
        CurrentCoins = initialCoins;
    }

    public void AddCoins(int amount)
    {
        CurrentCoins += amount;
        OnCoinChanged?.Invoke(CurrentCoins);
    }

    public bool HasEnoughCoins(int amount)
    {
        return CurrentCoins >= amount;
    }

    //결제 로직을 모델 안으로 캡슐화 (상점이 더 편해짐)
    public bool TryConsumeCoins(int amount)
    {
        if (HasEnoughCoins(amount))
        {
            CurrentCoins -= amount;
            OnCoinChanged?.Invoke(CurrentCoins);
            return true;
        }
        return false;
    }
}