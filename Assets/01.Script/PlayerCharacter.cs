
using System;
using System.Collections;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField] private Weapon playerWeapon;

    private Collider weaponCollider;
    private PlayerMovement movement;


    private void Start()
    {
        weaponCollider = playerWeapon.GetComponent<Collider>();
        movement = GetComponent<PlayerMovement>();
        weaponCollider.enabled = false;

        playerWeapon.damage = this.damage;
    }

    public override void Attack()
    {

    }

    public override void Hit(int damage)
    {
        base.Hit(damage);
        if (movement != null)
        {
            movement.TakeHit();
        }
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
