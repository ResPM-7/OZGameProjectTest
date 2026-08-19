using TMPro;
using UnityEngine;

public class CoinManager : Singleton<CoinManager>
{
    [SerializeField] private int coins;
    [SerializeField] private TextMeshProUGUI coinText;

    public int CurrentCoins { get; private set; }

    private void Start()
    {
        UpdateCoins(0);
    }

    private void OnEnable()
    {
        //Enemy.OnEnemyDeadDropCoinEvent += UpdateCoins;
    }

    private void OnDisable()
    {
        //Enemy.OnEnemyDeadDropCoinEvent -= UpdateCoins;
    }

    public void UpdateCoins(int changeAmount)
    {
        coins += changeAmount;

        coinText.text = coins.ToString();
    }

    public bool HasEnoughCoins(int amount)
    {
        return coins >= amount;
    }

}
