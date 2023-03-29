using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

[CreateAssetMenu(menuName = "Upgrades/Upgrade List Entity Runtime Set")]
public class UpgradeListEntityRuntimeSet : RuntimeSet<PreparationUpgradeEntity>
{
    private void OnEnable()
    {
        foreach (PreparationUpgradeEntity entity in Resources.FindObjectsOfTypeAll<PreparationUpgradeEntity>())
        {
            if (entity.RuntimeSet == this)
                Add(entity);
        }
    }
    private void OnDisable()
    {
        Items.Clear();
    }
    public void InitializeItems()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            Items[i].Initialize();
        }
    }
}
