using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.Events;
using UnityEngine.AI;
using System.Linq;

public class WaveController : MonoBehaviour
{
    [SerializeField] private int baseWavePower = 10;
    [SerializeField] private int powerIncreasePerDay = 3;
    [SerializeField] private float baseSpawnPeriod = 0.1f;
    [SerializeField] private float morningMultiplier = 1, noonMultiplier = 1, eveningMultiplier = 1, nightMultiplier = 1;
    [SerializeField] private float morningSpawnSpeed = 1, noonSpawnSpeed = 1, eveningSpawnSpeed = 1, nightSpawnSpeed = 1;
    [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private ObjectPool standardZombiePool, dayBossPool, zoneBossPool;
    [SerializeField] private WaveStateVariable waveState;
    [SerializeField] private UnityEvent onWaveStarted = new();
    [SerializeField] private UnityEvent onWaveCleared = new();
    [SerializeField] private UnityEvent onWaveClearPercentChanged = new();
    [SerializeField] private Transform spawnPointContainer;
    [SerializeField] private float spawnCircleRadius = 30;
    [SerializeField] private float spawnCircleWidth = 5;
    private List<Transform> spawnPoints = new();
    private IEnumerator zombieSpawningCoroutine = null;
    private int numberOfZombiesInWave = 0;
    private int numberOfDeadZombiesInWave = 0;
    private int remainingZombiesToSpawn = 0;
    private List<int> zombieTable = new();
    private int baseMaxHealth = 10;
    [SerializeField] private FloatVariable waveCleanPercentage;
    [SerializeField] private TowerDPS towerDPS;
    public int WavePower
    {
        get
        {
            float power = baseWavePower + (zoneManager.Day - 1) * powerIncreasePerDay;
            switch (zoneManager.WaveLevel % 4)
            {
                case 0:
                    power *= nightMultiplier;
                    break;
                case 1:
                    power *= morningMultiplier;
                    break;
                case 2:
                    power *= noonMultiplier;
                    break;
                case 3:
                    power *= eveningMultiplier;
                    break;
            }
            return Mathf.CeilToInt(power);
        }
    }


    private bool spawnOnRandomPoint { get => spawnPointContainer == null || spawnPointContainer.childCount == 0; }

    private void Awake()
    {
        waveState.Value = WaveState.NONE;
        InitializeSpawnPoints();
        waveCleanPercentage.Value = 0;
    }

    /*
    private List<int> _GenerateZombieTable(int power, int maxZombieLevel, int maxNumberOfZombies, out int numberOfZombies)
    {
        power = Mathf.Clamp(power, 1, maxNumberOfZombies * maxZombieLevel);
        int groupPower = power / maxZombieLevel;
        List<int> zombieTable = new() { 0 };
        int total = 0;
        for (int i = 1; i <= maxZombieLevel; i++)
        {
            int number = groupPower / i;
            total += number;
            zombieTable.Add(number);
        }

        int currentPower = 0;

        for (int i = 1; i < zombieTable.Count; i++)
        {
            currentPower += zombieTable[i] * i;
        }

        zombieTable[1] += power - currentPower;

        int count = 0;
        while (zombieTable.Sum() > maxNumberOfZombies && count++ < 500)
        {
            int maxIndex = zombieTable.IndexOfMax(1, maxZombieLevel - 1);
            int minIndex = zombieTable.IndexOfMin(maxIndex + 1, maxZombieLevel);
            if (maxIndex > minIndex)
            {
                maxIndex = zombieTable.IndexOfMax(1, maxZombieLevel);
                minIndex = zombieTable.IndexOfMin(1, maxZombieLevel);
            }
            int[] numbers = new int[] { minIndex, maxIndex };
            int lcm = test(numbers);
            zombieTable[minIndex] += lcm / minIndex;
            zombieTable[maxIndex] -= lcm / maxIndex;
        }


        numberOfZombies = zombieTable.Sum();
        List<int> result = new();
        for (int i = 1; i < zombieTable.Count; i++)
        {
            int zombieCount = zombieTable[i];
            for (int j = 0; j < zombieCount; j++)
            {
                result.Add(i);
            }
        }
        result.Shuffle();
        for (int i = 1; i < zombieTable.Count; i++)
        {
            print(i + ": " + zombieTable[i]);
        }
        print(zombieTable.Sum());
        return result;

    }*/

    static int gcd(int n1, int n2)
    {
        if (n2 == 0)
        {
            return n1;
        }
        else
        {
            return gcd(n2, n1 % n2);
        }
    }

    public static int test(int[] numbers)
    {
        return numbers.Aggregate((S, val) => S * val / gcd(S, val));
    }

    private void InitializeSpawnPoints()
    {
        if (!spawnOnRandomPoint)
        {
            for (int i = 0; i < spawnPointContainer.childCount; i++)
                spawnPoints.Add(spawnPointContainer.GetChild(i));
        }
    }

    public void StartWave()
    {
        waveState.Value = WaveState.STARTED;
        onWaveStarted.Invoke();
        SpawnZombies();
    }

    public List<int> GenerateZombieTable(int power, int minLevel, int maxLevel, out int numberOfZombies)
    {
        int budget = power;
        zombieTable.Clear();
        while (budget >= minLevel)
        {
            int level = Random.Range(minLevel, Mathf.Clamp(maxLevel, minLevel, budget) + 1);
            zombieTable.Add(level);
            budget -= level;
        }
        if (budget > 0)
            zombieTable.Add(budget);
        numberOfZombies = zombieTable.Count;
        return zombieTable;
    }

    public void SpawnZombies()
    {
        numberOfDeadZombiesInWave = 0;
        numberOfZombiesInWave = 0;
        waveCleanPercentage.Value = 0;
        onWaveClearPercentChanged.Invoke();
        //List<int> zombieTable = _GenerateZombieTable(WavePower, 6, WaveSize, out int numberOfZombies);
        print("numberofdays " + zoneManager.NumberOfDays);
        List<int> zombieTable = GenerateZombieTable(WavePower, Mathf.Clamp(zoneManager.Day - 15, 1, zoneManager.NumberOfDays), Mathf.Clamp(zoneManager.Day + 4, 1, zoneManager.NumberOfDays), out int numberOfZombies);
        remainingZombiesToSpawn = numberOfZombies;
        if (zoneManager.IsNight) remainingZombiesToSpawn++;
        // Spawns zombies from the zombie table in random order
        float spawnPeriod = baseSpawnPeriod;
        int wavePower = WavePower;
        switch (zoneManager.WaveLevel % 4)
        {
            case 0:
                spawnPeriod /= nightSpawnSpeed;
                if (zoneManager.IsLastDayOfZone())
                    wavePower += Mathf.Clamp(zoneManager.Day + 5, 1, zoneManager.NumberOfDays) * 15;
                else
                    wavePower += Mathf.Clamp(zoneManager.Day + 5, 1, zoneManager.NumberOfDays) * 10;
                break;
            case 1:
                spawnPeriod /= morningSpawnSpeed;
                break;
            case 2:
                spawnPeriod /= noonSpawnSpeed;
                break;
            case 3:
                spawnPeriod /= eveningSpawnSpeed;
                break;
        }
        int seconds = Mathf.CeilToInt((wavePower - 100) * 140 / 2000f + 40);
        baseMaxHealth = Mathf.CeilToInt(towerDPS.CalculateDamage(60) / (float)wavePower);
        print("Wave Size: " + numberOfZombies);
        print("Damage: " + towerDPS.CalculateDamage(60));
        print("WavePower: " + WavePower);
        print("BaseHealth: " + baseMaxHealth);
        WaitForSeconds waitForSeconds = new(spawnPeriod);
        IEnumerator spawnZombies(int count)
        {
            if (count <= 0)
            {
                if (zoneManager.IsNight)
                {
                    if (zoneManager.IsLastDayOfZone())
                    {
                        SpawnZombie(Mathf.Clamp(zoneManager.Day + 5, 1, zoneManager.NumberOfDays), ZombieType.ZONE_BOSS);
                    }
                    else
                    {
                        SpawnZombie(Mathf.Clamp(zoneManager.Day + 5, 1, zoneManager.NumberOfDays), ZombieType.DAY_BOSS);
                    }
                }
                yield break;
            }
            int level = zombieTable[^1];
            zombieTable.RemoveAt(zombieTable.Count - 1);
            SpawnZombie(level, ZombieType.STANDARD);
            yield return waitForSeconds;
            zombieSpawningCoroutine = spawnZombies(count - 1);
            yield return zombieSpawningCoroutine;
        }
        zombieSpawningCoroutine = spawnZombies(numberOfZombies);
        StartCoroutine(zombieSpawningCoroutine);
        // Checks if the phase is night, if so spawn boss
        // TODO implement here
    }

    public void StopSpawning()
    {
        if (zombieSpawningCoroutine == null) return;
        StopCoroutine(zombieSpawningCoroutine);
        zombieSpawningCoroutine = null;
        remainingZombiesToSpawn = 0;
    }

    public void SpawnZombie(int level, ZombieType type)
    {
        //Debug.Log("SpawnZombie " + level, this);
        Vector3 position = GetSpawnPosition();
        Zombie zombie;
        int baseMaxHealth = this.baseMaxHealth;
        switch (type)
        {
            case ZombieType.DAY_BOSS:
                zombie = dayBossPool.Get<Zombie>(position, Quaternion.identity);
                baseMaxHealth *= 10;
                break;
            case ZombieType.ZONE_BOSS:
                zombie = zoneBossPool.Get<Zombie>(position, Quaternion.identity);
                baseMaxHealth *= 15;
                break;
            default:
                zombie = standardZombiePool.Get<Zombie>(position, Quaternion.identity);
                break;
        }

        zombie.SetBaseMaxHealth(baseMaxHealth);
        zombie.SetLevel(level);
        numberOfZombiesInWave++;
        remainingZombiesToSpawn--;
        zombie.OnDied.AddListener(OnSpawnedZombieDied);
    }

    private bool IsWaveCleared { get => numberOfZombiesInWave == 0 && remainingZombiesToSpawn == 0; }

    private void OnSpawnedZombieDied()
    {
        numberOfDeadZombiesInWave++;
        numberOfZombiesInWave--;
        /*Debug.Log("numberOfZombiesInWave: " + numberOfZombiesInWave +
            "\nremainingZombiesToSpawn: " + remainingZombiesToSpawn +
            "\nnumberOfDeadZombiesInWave: " + numberOfDeadZombiesInWave + 
            "\ntotal: " + (numberOfZombiesInWave + remainingZombiesToSpawn + numberOfDeadZombiesInWave));*/
        waveCleanPercentage.Value = 1f - (numberOfZombiesInWave + remainingZombiesToSpawn) / (float)(numberOfZombiesInWave + remainingZombiesToSpawn + numberOfDeadZombiesInWave);
        onWaveClearPercentChanged.Invoke();
        if (IsWaveCleared)
            OnWaveCleared();
    }
    /*
    private List<int> GenerateZombieTable(int power, int maxZombieLevel, out int numberOfZombies)
    {
        //Debug.Log("GenerateZombieTable", this);
        numberOfZombies = 0;
        List<int> zombieTable = new() { -1, power / maxZombieLevel };
        for (int i = 2; i < maxZombieLevel + 1; i++)
            zombieTable.Add(zombieTable[1] / i);
        int currentPower = 0;
        for (int i = 1; i < zombieTable.Count; i++)
            currentPower += i * zombieTable[i];
        zombieTable[1] += power - currentPower;
        for (int i = 1; i < zombieTable.Count; i++)
            numberOfZombies += zombieTable[i];
        List<int> result = new();
        for (int i = 1; i < zombieTable.Count; i++)
        {
            int zombieCount = zombieTable[i];
            for (int j = 0; j < zombieCount; j++)
            {
                result.Add(i);
            }
        }
        result.Shuffle();
        return result;
    }
    */
    public void OnWaveCleared()
    {
        waveState.Value = WaveState.CLEARED;
        onWaveCleared.Invoke();
    }

    public enum ZombieType
    {
        STANDARD,
        DAY_BOSS,
        ZONE_BOSS
    }

    public Vector3 GetSpawnPosition()
    {
        if (spawnOnRandomPoint)
            return GenerateSpawnPosition();
        else
            return spawnPoints[Random.Range(0, spawnPoints.Count)].position;
    }

    private Vector3 GenerateSpawnPosition()
    {
        Vector3 position;
        int count = 0;
        do
        {
            if (count > 0)
                print(count);
            position = Quaternion.Euler(0, Random.Range(0f, 360f), 0) * Vector3.right * spawnCircleRadius;
            position += position.normalized * Random.Range(0, spawnCircleWidth);
        } while (count++ < 100 && !NavMesh.SamplePosition(position, out NavMeshHit hit, 0.5f, NavMesh.AllAreas));
        if (count == 100) Debug.LogError("Could not generate a random spawn position on given circle", this);
        return position;
    }

    public enum WaveState { NONE, STARTED, CLEARED }


}



public partial class SaveData
{
    public int WaveLevel = 1;
}
