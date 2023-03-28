using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Turret/Turret Damage")]
public class TurretDamageUpgradeEntity : UpgradeEntity
{
    public override int Cost => Level * 10;
    protected override int Level { get => saveData.Value.TurretDamageLevel; set => saveData.Value.TurretDamageLevel = value; }

}
namespace FateGames.Core
{
    public partial class SaveData
    {
        public int TurretDamageLevel = 0;
    }
}