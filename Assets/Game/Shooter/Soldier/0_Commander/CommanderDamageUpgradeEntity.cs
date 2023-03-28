using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Commander/Damage Upgrade")]
public class CommanderDamageUpgradeEntity : UpgradeEntity
{
    public override int Cost => Level * 10;

    protected override int Level { get => saveData.Value.CommanderDamageLevel; set => saveData.Value.CommanderDamageLevel = value; }

}
namespace FateGames.Core
{
    public partial class SaveData
    {
        public int CommanderDamageLevel = 1;
    }
}