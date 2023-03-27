using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using System.Linq;

public class UIUpgradeListView : FateMonoBehaviour
{
    [SerializeField] private GameObject upgradeItemPrefab;
    [SerializeField] private UpgradeListEntityRuntimeSet upgradeEntitySet;
    [SerializeField] private UIListView listView;

    private void Awake()
    {
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
        UpgradeEntity[] upgradeEntities = new UpgradeEntity[count];
        upgradeEntitySet.Items.CopyTo(upgradeEntities, 0);
        upgradeEntities = upgradeEntities.OrderBy((item) => item.Cost).ToArray();
        for (int i = 0; i < count; i++)
        {
            UpgradeEntity upgradeEntity = upgradeEntities[i];
            //if (upgradeEntity.Locked) break;
            UpgradeItem item = listView.AddItem<UpgradeItem>(upgradeItemPrefab);
            item.Set(upgradeEntity);
        }
    }

    public void Clear()
    {
        listView.Clear();
    }
}
