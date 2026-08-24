using TMPro;
using UnityEngine;

public class CoinView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;

    // 프리젠터가 이 함수를 호출해서 화면을 바꿔줄 겁니다.
    public void UpdateCoinText(int currentCoins)
    {
        if (coinText != null)
        {
            coinText.text = currentCoins.ToString();
        }
    }
}