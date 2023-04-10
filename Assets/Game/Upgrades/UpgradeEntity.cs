using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FateGames.Core;
using UnityEngine.Events;

public abstract class UpgradeEntity : ScriptableObject
{
    [System.Serializable]
    public class Prerequisite
    {
        public UpgradeEntity Entity;
        public int Level;
        public bool Met => Entity.Level >= Level;
    }
    [SerializeField] protected int baseCost = 10;
    [SerializeField] protected int increasePerLevel = 10;
    [SerializeField] protected Prerequisite[] prerequisites = new Prerequisite[0];
    [SerializeField] protected SaveDataVariable saveData;
    [SerializeField] private string upgradeName;
    public UnityEvent OnUpgrade = new();
    public abstract int Level { get; protected set; }
    [SerializeField] protected int limit = -1;
    [SerializeField] protected DayLimit[] dayLimits = new DayLimit[0];
    public string UpgradeName { get => upgradeName; }
    public abstract bool Affordable { get; }
    public bool MaxedOut
    {
        get => limit >= 0 && Level >= limit;
    }
    public bool Locked
    {
        get
        {
            for (int i = 0; i < prerequisites.Length; i++)
            {
                Prerequisite prerequisite = prerequisites[i];
                if (!prerequisite.Met)
                    return true;
            }
            int Day = 1;
            bool result = false;
            for (int i = 0; i < dayLimits.Length; i++)
            {
                DayLimit dayLimit = dayLimits[i];
                if (Day <= dayLimit.Day)
                {
                    result = Level >= dayLimit.Limit;
                    break;
                }
                else if (i == dayLimits.Length - 1)
                {
                    result = Level >= dayLimit.Limit;
                }
            }
            return result;
        }
    }
    public bool Upgradeable => Affordable && !MaxedOut && !Locked;

    [System.Serializable]
    public class DayLimit
    {
        public int Day;
        public int Limit;
    }

    public abstract int Cost { get; }
    public int Limit { get => limit; }

    public abstract void BuyUpgrade();
    public virtual void Upgrade()
    {
        Level++;
        OnUpgrade.Invoke();
    }
}

