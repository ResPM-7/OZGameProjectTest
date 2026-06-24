using UnityEngine;

public class TestAttack : MonoBehaviour
{
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();

            if (player != null)
            {
                player.Hit(damage);
            }
        }
    }
}
