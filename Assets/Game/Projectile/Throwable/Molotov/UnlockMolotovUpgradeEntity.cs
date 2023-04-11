using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Commander/Molotov/Unlock Molotov Upgrade")]
public class UnlockMolotovUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => baseCost;

    public override int Level { get => saveData.Value.MolotovUnlocked ? 1 : 0; protected set => saveData.Value.MolotovUnlocked = value > 0; }
}

public partial class SaveData
{
    public bool MolotovUnlocked = false;
}