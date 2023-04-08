using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.Events;
using DG.Tweening;
public partial class Tower
{
    [SerializeField] private List<SoldierSet> soldierTable;
    [SerializeField] private List<ObjectPool> soldierPools;
    [SerializeField] private ObjectPool poofEffectPool;
    [SerializeField] private ObjectPool magicBuffEffectPool;

    [SerializeField] private UnityEvent OnNewSoldier, OnNewSoldierAchieved;
    [SerializeField] private IntVariable lastAchievedSoldierLevel;
    [SerializeField] private SoldierUnlockTable soldierUnlockTable;
    private readonly static int mergeSize = 3;
    private readonly float mergeAnimationDuration = 0.25f;
    private List<Soldier> sortedSoldiers = new();
    public int NumberOfSoldiers { get; private set; } = 0;
    public override void Repair()
    {
        SetHealth(maxHealth);
    }
    public void Merge()
    {
        // Check if there are mergeSize soldiers of same level
        if (!CanMerge(out int level)) return;
        //TODO change with real cost
        int cost = 5;
        // Try to spend money, if cannot afford return
        if (!saveData.SpendMoney(cost)) return;
        /*// Remove the mergeSize soldiers of the returned level from CanMerge function
        for (int i = 0; i < mergeSize; i++)
            RemoveSoldier(level);
        // Add 1 higher level soldier
        AddSoldier(level + 1);
        PlaceSoldiers();*/
        IEnumerator mergeRoutine()
        {
            int count = mergeSize;
            for (int i = 0; i < mergeSize; i++)
            {
                SoldierSet set = soldierTable[level];
                Soldier soldier = set.Items[set.Items.Count - 1 - i];
                soldier.transform.DOMove(mergePoint.position, mergeAnimationDuration).OnComplete(() =>
                {
                    RemoveSoldier(level);
                    count--;
                });
            }
            yield return new WaitUntil(() => count == 0);
            bool newSoldierAchieved = true;
            for (int i = level + 1; i < soldierTable.Count; i++)
            {
                if (soldierTable[i].Items.Count > 0)
                {
                    newSoldierAchieved = false;
                    break;
                }
            }
            PlaceSoldiers();
            AddSoldier(level + 1, mergePoint.position);
            poofEffectPool.Get<Transform>(mergePoint.position, Quaternion.identity);
            if (newSoldierAchieved)
            {
                lastAchievedSoldierLevel.Value = level + 1;
                OnNewSoldierAchieved.Invoke();
            }
        }
        StartCoroutine(mergeRoutine());
    }
    public bool CanMerge(out int level)
    {
        // Index starts with 1 because soldier levels starts with 1
        int i = 1;
        // Assign default value
        bool canMerge = false;
        // Cannot merge the maximum level soldiers
        int limitLevel = soldierUnlockTable.GetLastUnlockedSoldierLevel();
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
    public void AddSoldier(int level, Vector3 spawnPoint, bool save = true)
    {
        if (NumberOfSoldiers >= points.Count)
        {
            Debug.LogError("Cannot add soldier. Max number of soldiers reached!");
            return;
        }
        Vector3 position = GetPoint(NumberOfSoldiers).position;
        Soldier soldier = soldierPools[level].Get<Soldier>(spawnPoint, Quaternion.identity);
        soldier.transform.DOMove(position, mergeAnimationDuration);
        NumberOfSoldiers++;
        isTowerFull.Value = NumberOfSoldiers >= points.Count;
        if (save)
        {
            saveData.Value.SoldierTable[level]++;
        }
        OnNewSoldier.Invoke();
        if (CanMerge(out _))
        {
            OnMergeAvailable.Invoke();

        }
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
            OnMergeAvailable.Invoke();
        }
        magicBuffEffectPool.Get<Transform>(position, Quaternion.identity);
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
        sortedSoldiers.Clear();
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
