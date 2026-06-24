
using System;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField] private Weapon playerWeapon;

    private Collider weaponCollider;
    private void Start()
    {
        weaponCollider = playerWeapon.GetComponent<Collider>();
        weaponCollider.enabled = false;

        playerWeapon.damage = this.damage;
    }

    public override void Attack()
    {

    }

    public void Heal(int healAmount)
    {
        currentHp += healAmount;

        if(currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }

    public void EnableWeapon()
    {
        weaponCollider.enabled = true;
    }
    public void DisableWeapon()
    {
        weaponCollider.enabled = false;
    }
}
