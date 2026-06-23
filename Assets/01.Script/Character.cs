using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected int hp;
    [SerializeField] protected int damage;

    public virtual void Hit(int damage)
    {
        hp -= damage;

        if(hp < 0 ) hp = 0;
    }
    public abstract void Attack();

    public bool IsAlive()
    {
        return hp > 0;
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
    }
}
