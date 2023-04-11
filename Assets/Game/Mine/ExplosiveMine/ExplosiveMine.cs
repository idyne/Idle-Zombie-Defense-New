using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMine : Mine
{
    [SerializeField] private PreparationUpgradeEntity damageUpgrade;
    [SerializeField] private int baseDamage = 20;
    private int damage => baseDamage * damageUpgrade.Level;
    protected void Hit(Damageable damageable)
    {
        damageable.Hit(damage);
    }

    protected override void OnExplosion(Zombie zombie)
    {
        Hit(zombie);
    }
}
