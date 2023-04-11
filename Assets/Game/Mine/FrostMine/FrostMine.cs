using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostMine : Mine
{
    [SerializeField] private PreparationUpgradeEntity durationUpgrade;
    [SerializeField] private float baseDuration = 5;
    private float duration => baseDuration - 2f * durationUpgrade.Level;
    protected override void OnExplosion(Zombie zombie)
    {
        zombie.Freeze(duration);
    }
}
