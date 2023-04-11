using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
[CreateAssetMenu(menuName = "Zone Manager")]
public class ZoneManager : ScriptableObject
{
    [SerializeField] private SaveDataVariable saveData;
    [SerializeField] private int[] zoneLengths;
    public int Day = 0;
    public int Zone = 0;
    public int NormalizedDay = 0;
    public int WaveLevel { get => saveData.Value.WaveLevel; }
    public int ZoneLength { get => zoneLengths[Zone - 1]; }
    public int NumberOfDays = 0;

    public bool IsNight { get => WaveLevel % 4 == 0; }
    public bool IsLastDayOfZone()
    {
        int total = 0;
        for (int i = 0; i < zoneLengths.Length; i++)
        {
            int zoneLength = zoneLengths[i];
            total += zoneLength;
            if (Day == total) return true;
            if (Day < total) break;
        }
        return false;
    }
    public bool IsLastDayOfGame()
    {
        int total = 0;
        for (int i = 0; i < zoneLengths.Length; i++)
        {
            int zoneLength = zoneLengths[i];
            total += zoneLength;
        }
        return Day == total;
    }

    public void ResetGame()
    {
        saveData.Value = new SaveData();
    }

    public void Initialize()
    {
        SetNumberOfDays();
        SetDay();
        SetZone();
        SetNormalizedDay();
    }
    private void SetDay()
    {
        Day = (WaveLevel - 1) / 4 + 1;
    }
    public void SetZone()
    {
        int total = 0;
        for (int i = 0; i < zoneLengths.Length; i++)
        {
            int zoneLength = zoneLengths[i];
            total += zoneLength;
            if (Day <= total)
            {
                Zone = i + 1;
                break;
            }
        }
    }

    public void SetNumberOfDays()
    {
        NumberOfDays = 0;
        for (int i = 0; i < zoneLengths.Length; i++)
        {
            NumberOfDays += zoneLengths[i];
        }
    }

    private void SetNormalizedDay()
    {
        int total = 0;
        for (int i = 0; i < zoneLengths.Length; i++)
        {
            int zoneLength = zoneLengths[i];
            if (Day <= total + zoneLength)
            {
                NormalizedDay = Day - total;
                break;
            }
            total += zoneLength;
        }
    }

    public void IncrementWaveLevel()
    {
        saveData.Value.WaveLevel++;
    }

    public void ResetWaveLevelToDay()
    {
        saveData.Value.WaveLevel = Day * 4 - 3;
    }

}
