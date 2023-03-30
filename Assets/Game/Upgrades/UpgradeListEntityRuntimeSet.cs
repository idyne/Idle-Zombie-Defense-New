using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

[CreateAssetMenu(menuName = "Upgrades/Upgrade List Entity Runtime Set")]
public class UpgradeListEntityRuntimeSet : RuntimeSet<PreparationUpgradeEntity>
{
    private void OnEnable()
    {
        Items.Clear();
        foreach (PreparationUpgradeEntity entity in Resources.LoadAll("Upgrades", typeof(PreparationUpgradeEntity)))
        {
            if (entity.RuntimeSet == this)
                Add(entity);
        }
    }

    public void InitializeItems()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].Initialize();
        }
    }
}
