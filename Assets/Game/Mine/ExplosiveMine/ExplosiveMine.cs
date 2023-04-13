using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveMine : Mine, IDamageObject
{
    [SerializeField] private TowerDPS towerDPS;
    [SerializeField] private PreparationUpgradeEntity damageUpgrade;
    [SerializeField] private int baseDamage = 200;
    [SerializeField] private float damageIncreaseRate = 1.3f;
    private int damage => Mathf.CeilToInt(baseDamage * Mathf.Pow(damageIncreaseRate, damageUpgrade.Level));

    private void OnEnable()
    {
        towerDPS.Register(this);
    }
    private void OnDisable()
    {
        towerDPS.Unregister(this);
    }

    public int GetDamage()
    {
        return damage;
    }

    protected override void OnExplosion(Zombie zombie, int numberOfAffectedZombies)
    {
        zombie.Hit(damage / numberOfAffectedZombies);
    }
}
