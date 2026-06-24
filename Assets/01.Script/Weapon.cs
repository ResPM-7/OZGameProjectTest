using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Character hitCharacter = other.GetComponent<Character>();

            if (hitCharacter != null && hitCharacter.IsAlive())
            {
                hitCharacter.Hit(damage);
            }

        }
    }
}
