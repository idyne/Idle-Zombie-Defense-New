using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Commander/Molotov/Unlock Molotov Upgrade")]
public class UnlockMolotovUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => 100;

    protected override int Level { get => saveData.Value.MolotovUnlocked ? 1 : 0; set => saveData.Value.MolotovUnlocked = value > 0; }
}

public partial class SaveData
{
    public bool MolotovUnlocked = false;
}