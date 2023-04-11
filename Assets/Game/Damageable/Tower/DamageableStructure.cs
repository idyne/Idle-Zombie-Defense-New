using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageableStructure : Damageable
{
    [SerializeField] protected PreparationUpgradeEntity healthUpgrade;
    protected override int maxHealth => base.maxHealth + healthUpgrade.Level * 100;
    public abstract void Repair();
    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable)
            damageable.OnTriggerEnterNotification();
    }
    private void OnTriggerExit(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable)
            damageable.OnTriggerExitNotification();
    }

}
