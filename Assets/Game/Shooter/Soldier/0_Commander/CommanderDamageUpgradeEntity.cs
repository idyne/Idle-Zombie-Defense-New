using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Preparation/Commander/Damage Upgrade")]
public class CommanderDamageUpgradeEntity : PreparationUpgradeEntity
{
    public override int Cost => Level * 10;

    protected override int Level { get => saveData.Value.CommanderDamageLevel; set => saveData.Value.CommanderDamageLevel = value; }

}

public partial class SaveData
{
    public int CommanderDamageLevel = 1;
}
