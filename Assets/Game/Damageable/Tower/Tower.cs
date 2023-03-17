using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.Events;
using DG.Tweening;

public partial class Tower : DamageableStructure
{
    [SerializeField] private Transform pointContainer;
    private readonly List<Transform> points = new();
    [SerializeField] private SaveDataVariable saveData;
    [SerializeField] private UnityEvent OnMergeAvailable;
    [SerializeField] private LevelManager levelManager;

    private void Start()
    {
        AddSoldier(0);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        SetPoints();
    }

    public void BuySoldier()
    {
        if (NumberOfSoldiers >= points.Count)
        {
            Debug.LogError("Cannot buy soldier. Max number of soldiers reached!");
            return;
        }
        // TODO change to real cost
        int cost = 0;
        saveData.SpendMoney(cost);
        AddSoldier(saveData.Value.SoldierBuyingLevel);
    }



    private Transform GetPoint(int index)
    {
        return points[index];
    }

    private void SetPoints()
    {
        for (int i = 0; i < pointContainer.childCount; i++)
            points.Add(pointContainer.GetChild(i));
    }

    public void PlaceSoldiers()
    {
        List<Soldier> soldiers = GetSortedSoldierList();

        for (int i = 0; i < soldiers.Count; i++)
        {
            Soldier soldier = soldiers[i];
            soldier.SetPosition(points[i].position);
        }
    }

    public override void Die()
    {
        Debug.Log("Die", this);
        OnDied.Invoke();
        IEnumerator finishLevelAfterSeconds(float t)
        {
            yield return new WaitForSeconds(t);
            levelManager.FinishLevel(false);
        }
        StartCoroutine(finishLevelAfterSeconds(3));
    }
}
