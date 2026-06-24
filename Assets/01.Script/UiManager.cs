using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] Slider hpSlider;

    [SerializeField] private Character playerCharacter;

    private void Start()
    {
        if (playerCharacter != null)
        {
            hpSlider.maxValue = playerCharacter.MaxHp;
            hpSlider.value = playerCharacter.CurrentHp;

            playerCharacter.OnHpChanged += UpdateHpUI;
        }
    }

    private void UpdateHpUI()
    {
        hpSlider.value = playerCharacter.CurrentHp;
    }
}
