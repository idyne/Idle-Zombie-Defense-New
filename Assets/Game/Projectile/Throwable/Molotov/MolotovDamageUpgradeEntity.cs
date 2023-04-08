using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Commander/Molotov/Molotov Damage Upgrade")]
public class MolotovDamageUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => Level * 100;

    public override int Level { get => saveData.Value.MolotovDamageLevel; protected set => saveData.Value.MolotovDamageLevel = value; }
}

public partial class SaveData
{
    public int MolotovDamageLevel = 1;
}