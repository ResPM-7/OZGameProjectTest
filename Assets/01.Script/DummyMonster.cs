using UnityEngine;
using TMPro;

public class DummyMonster : MonoBehaviour
{
    private TextMeshPro currentHpText;
    private EnemyCharacter enemy;


    private int currentHp;
    private void Start()
    {
        currentHpText = GetComponentInChildren<TextMeshPro>();
        enemy = GetComponent<EnemyCharacter>();

        if (enemy != null)
        {
            enemy.OnHpChanged += UpdateHpText;

            UpdateHpText();
        }
    }


    private void UpdateHpText()
    {
        if (currentHpText != null && enemy != null)
        {
            currentHpText.text = enemy.CurrentHp.ToString();
        }
    }

    private void OnDisable()
    {
        if (enemy != null)
        {
            enemy.OnHpChanged -= UpdateHpText;
        }
    }
}