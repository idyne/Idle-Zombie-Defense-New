using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using System.Linq;

public class UIUpgradeListView : UIElement
{
    [SerializeField] private GameObject upgradeItemPrefab;
    [SerializeField] private UpgradeListEntityRuntimeSet upgradeEntitySet;
    [SerializeField] private UIListView listView;

    protected override void Awake()
    {
        base.Awake();
        upgradeEntitySet.OnChange.AddListener(Rebuild);
    }

    private void Start()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        Clear();
        int count = upgradeEntitySet.Items.Count;
        PreparationUpgradeEntity[] upgradeEntities = new PreparationUpgradeEntity[count];
        upgradeEntitySet.Items.CopyTo(upgradeEntities, 0);
        upgradeEntities = upgradeEntities.OrderBy((item) => item.Locked).ThenBy((item) => item.MaxedOut).ThenBy((item) => item.Cost).ToArray();
        for (int i = 0; i < count; i++)
        {
            PreparationUpgradeEntity upgradeEntity = upgradeEntities[i];
            //if (upgradeEntity.Locked) break;
            UpgradeItem item = listView.AddItem<UpgradeItem>(upgradeItemPrefab);
            item.Set(upgradeEntity);
        }
        listView.GoToUp();
    }

    public void Clear()
    {
        listView.Clear();
    }
}
