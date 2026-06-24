using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected int maxHp;
    [SerializeField] protected int currentHp;
    [SerializeField] protected int damage;

    private void Start()
    {
        currentHp = maxHp;
    }

    public virtual void Hit(int damage)
    {
        currentHp -= damage;

        if(currentHp < 0 ) currentHp = 0;
    }
    public abstract void Attack();

    public bool IsAlive()
    {
        return currentHp > 0;
    }

    public virtual void Die()
    {
        gameObject.SetActive(false);
    }
}
