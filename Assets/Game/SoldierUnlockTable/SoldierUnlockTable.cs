using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Soldier Unlock Table")]
public class SoldierUnlockTable : ScriptableObject
{
    [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private SoldierUnlockEntity[] entities;
    public SoldierUnlockEntity this[int day]
    {
        get
        {
            for (int i = 0; i < entities.Length; i++)
            {
                SoldierUnlockEntity entity = entities[i];
                if (entity.Day == day)
                    return entity;
            }
            return null;
        }
    }
    [System.Serializable]
    public class SoldierUnlockEntity
    {
        public int Day;
        public int SoldierLevel;
    }

    public int GetLastUnlockedSoldierLevel()
    {

        for (int i = 0; i < entities.Length - 1; i++)
        {
            SoldierUnlockEntity entity = entities[i];
            SoldierUnlockEntity nextEntity = entities[i + 1];
            if (zoneManager.Day > entity.Day && zoneManager.Day <= nextEntity.Day)
            {
                return entity.SoldierLevel;
            }
        }
        return 0;
    }
}
