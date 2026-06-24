using System.Collections;
using UnityEngine;

public class FloorEffect : MonoBehaviour
{
    [SerializeField] private bool isHeal = true;
    [SerializeField] private int damage = 5;
    [SerializeField] private int healing = 5;
    [SerializeField] private float tickRate = 1.0f;

    private WaitForSeconds tickRateDelay;

    private Coroutine currentEffect;
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
                currentEffect = StartCoroutine(ApplyEffect(player));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentEffect != null)
            {
                StopCoroutine(currentEffect);
                currentEffect = null;
            }
        }
    }

    private IEnumerator ApplyEffect(PlayerCharacter player)
    {
        while (player.IsAlive())
        {
            if (isHeal)
            {
                player.Heal(healing);
            }
            else
            {
                player.Hit(damage);
            }

            yield return tickRateDelay;
        }
    }
}
