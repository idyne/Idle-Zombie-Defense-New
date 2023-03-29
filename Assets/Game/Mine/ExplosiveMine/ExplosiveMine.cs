using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMine : Mine
{
    [SerializeField] private int damage = 20;
    
    protected void Hit(Damageable damageable)
    {
        damageable.Hit(damage);
    }

    protected override void OnExplosion(Zombie zombie)
    {
        Hit(zombie);
    }
}
