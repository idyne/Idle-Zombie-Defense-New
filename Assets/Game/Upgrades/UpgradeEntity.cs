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
    [SerializeField] protected Prerequisite[] prerequisites;
    [SerializeField] protected SaveDataVariable saveData;
    [SerializeField] private string upgradeName;
    public UnityEvent OnUpgrade = new();
    protected abstract int Level { get; set; }
    [SerializeField] protected int Limit = -1;
    [SerializeField] protected DayLimit[] dayLimits;
    public string UpgradeName { get => upgradeName; }
    public abstract bool Affordable { get; }
    public bool MaxedOut
    {
        get => Limit >= 0 && Level >= Limit;
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


    public abstract void BuyUpgrade();
    public virtual void Upgrade()
    {
        Level++;
        OnUpgrade.Invoke();
    }
}

