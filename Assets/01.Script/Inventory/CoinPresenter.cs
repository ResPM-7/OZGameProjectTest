using UnityEngine;

public class CoinPresenter : MonoBehaviour
{
    [SerializeField] private CoinView coinView; // 인스펙터에서 View 연결
    [SerializeField] private int startCoin;

    // 외부에서 데이터를 읽고 조작할 수 있도록 모델을 열어둠
    public CoinModel Model { get; private set; }

    private void Awake()
    {
        // 1. 모델 초기화
        Model = new CoinModel(startCoin);
    }

    private void OnEnable()
    {
        // 2. 모델의 방송(OnCoinChanged)을 뷰(UpdateCoinText)에 다이렉트로 꽂아버림 (구독)
        Model.OnCoinChanged += coinView.UpdateCoinText;

        // 3. 시작할 때 초기 코인 화면에 띄우기
        coinView.UpdateCoinText(Model.CurrentCoins);
    }

    private void OnDisable()
    {
        Model.OnCoinChanged -= coinView.UpdateCoinText;
    }
}