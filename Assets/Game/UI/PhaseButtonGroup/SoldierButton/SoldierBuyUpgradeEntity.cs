using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Phase Upgrades/Soldier Buying Upgrade")]
public class SoldierBuyUpgradeEntity : PhaseUpgradeEntity
{
    [SerializeField] private SoundEntity sound;
    [SerializeField] private SoundManager soundManager;
    public override int Cost => Level * 10;

    protected override int Level { get => saveData.Value.SoldierBuyingCount; set => saveData.Value.SoldierBuyingCount = value; }

    public override void Upgrade()
    {
        base.Upgrade();
        soundManager.PlaySoundOneShot(sound);
    }
}

public partial class SaveData
{
    public int SoldierBuyingCount = 0;
}