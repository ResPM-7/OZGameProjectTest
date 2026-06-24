using UnityEngine;

public class HealItem : MonoBehaviour
{
    [SerializeField] private int healAmount = 20;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();

            if (player != null && player.IsAlive())
            {
                player.Heal(healAmount);
                gameObject.SetActive(false);
            }
        }
    }
}
