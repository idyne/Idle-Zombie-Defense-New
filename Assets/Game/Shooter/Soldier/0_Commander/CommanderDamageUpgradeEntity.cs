using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Commander/Damage Upgrade")]
public class CommanderDamageUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => Level * increasePerLevel + baseCost;

    public override int Level { get => saveData.Value.CommanderDamageLevel; protected set => saveData.Value.CommanderDamageLevel = value; }

}

public partial class SaveData
{
    public int CommanderDamageLevel = 0;
}
