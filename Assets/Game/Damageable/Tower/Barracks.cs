using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.Events;

public partial class Tower
{
    [SerializeField] private List<SoldierSet> soldierTable;
    [SerializeField] private List<ObjectPool> soldierPools;
    [SerializeField] private UnityEvent OnNewSoldier;
    private readonly static int mergeSize = 3;
    public int NumberOfSoldiers { get; private set; } = 0;

    public void Merge()
    {
        // Check if there are mergeSize soldiers of same level
        if (!CanMerge(out int level)) return;
        //TODO change with real cost
        int cost = 5;
        // Try to spend money, if cannot afford return
        if (!saveData.SpendMoney(cost)) return;
        // Remove the mergeSize soldiers of the returned level from CanMerge function
        for (int i = 0; i < mergeSize; i++)
            RemoveSoldier(level);
        // Add 1 higher level soldier
        AddSoldier(level + 1);
        PlaceSoldiers();
    }
    public bool CanMerge(out int level)
    {
        // Index starts with 1 because soldier levels starts with 1
        int i = 1;
        // Assign default value
        bool canMerge = false;
        // Cannot merge the maximum level soldiers
        int limitLevel = soldierTable.Count - 1;
        // Iterate through all soldier levels
        while (i < limitLevel)
        {
            // If there are more than or equal to merge size soldiers in a set, that level can be merged 
            if (soldierTable[i].Items.Count >= mergeSize)
            {
                canMerge = true;
                break;
            }
            // Else go next level
            i++;
        }
        level = i;
        return canMerge;
    }

    public void AddSoldier()
    {
        AddSoldier(saveData.Value.SoldierBuyingLevel);
    }

    public void AddSoldier(int level, bool save = true)
    {
        if (NumberOfSoldiers >= points.Count)
        {
            Debug.LogError("Cannot add soldier. Max number of soldiers reached!");
            return;
        }
        Vector3 position = GetPoint(NumberOfSoldiers).position;
        soldierPools[level].Get<Soldier>(position, Quaternion.identity);
        NumberOfSoldiers++;
        isTowerFull.Value = NumberOfSoldiers >= points.Count;
        if (save)
        {
            saveData.Value.SoldierTable[level]++;
        }
        OnNewSoldier.Invoke();
        if (CanMerge(out _))
        {
            print("merge");
            OnMergeAvailable.Invoke();
        }
    }

    public void RemoveSoldier(int level)
    {
        soldierTable[level].Items[^1].Release();
        NumberOfSoldiers--;
        isTowerFull.Value = NumberOfSoldiers >= points.Count;
        saveData.Value.SoldierTable[level]--;
    }

    public List<Soldier> GetSortedSoldierList()
    {
        List<Soldier> sortedSoldiers = new();
        sortedSoldiers.AddRange(soldierTable[0].Items);
        int limitLevel = soldierTable.Count - 1;
        int i = limitLevel;
        while (i >= 1)
        {
            sortedSoldiers.AddRange(soldierTable[i].Items);
            i--;
        }
        return sortedSoldiers;
    }

}
