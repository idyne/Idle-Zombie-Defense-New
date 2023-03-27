using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Commander/Damage Upgrade")]
public class CommanderDamageUpgradeEntity : UpgradeEntity
{
    protected override int Level { get => saveData.Value.CommanderDamageLevel; set => saveData.Value.CommanderDamageLevel = value; }

}
namespace FateGames.Core
{
    public partial class SaveData
    {
        public int CommanderDamageLevel = 1;
    }
}