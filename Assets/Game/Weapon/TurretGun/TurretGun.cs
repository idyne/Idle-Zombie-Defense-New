using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretGun : Gun
{
    [SerializeField] private PreparationUpgradeEntity damageUpgrade;
    public override int Damage => base.Damage * damageUpgrade.Level;
}
