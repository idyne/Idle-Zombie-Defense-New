using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostMine : Mine
{
    [SerializeField] private PreparationUpgradeEntity durationUpgrade;
    [SerializeField] private float baseDuration = 5;
    [SerializeField] private float durationIncrease = 1;
    private float duration => baseDuration + durationIncrease * durationUpgrade.Level;
    protected override void OnExplosion(Zombie zombie, int numberOfAffectedZombies)
    {
        zombie.Freeze(duration);
    }
}
