using System.Collections;
using UnityEngine;

public class PoisonSwamp : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    [SerializeField] private float tickRate = 1.0f;

    private WaitForSeconds tickRateDelay;
    private void Start()
    {
        tickRateDelay = new WaitForSeconds(tickRate);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();
            if (player != null)
            {
                StartCoroutine(ApplyEffect(player));
            }
        }
    }

    IEnumerator ApplyEffect(PlayerCharacter player)
    {
        while (player.IsAlive())
        {
            player.Hit(damage);
            yield return tickRateDelay;
        }
    }
}
