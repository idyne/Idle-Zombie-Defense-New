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
    [SerializeField] private IntVariable baseMaxHealth;
    [SerializeField] private FloatVariable waveCleanPercentage;
    [SerializeField] private TowerDPS towerDPS;
    private int numberOfZombieLevels = 10;
    private int anchorLevel
    {
        get
        {
            float normalizedDay = (float)zoneManager.Day / zoneManager.NumberOfDays;
            int anchorLevel = Mathf.CeilToInt(numberOfZombieLevels * normalizedDay);
            print(anchorLevel);
            return anchorLevel;
        }
    }
    private int dayBossLevel => anchorLevel * 20;
    private int zoneBossLevel => anchorLevel * 30;
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
        List<int> zombieTable = GenerateZombieTable(WavePower, Mathf.Clamp(anchorLevel - 3, 1, numberOfZombieLevels), Mathf.Clamp(anchorLevel + 1, 1, numberOfZombieLevels), out int numberOfZombies);
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
                    wavePower += zoneBossLevel;
                else
                    wavePower += dayBossLevel;
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
        int seconds = Mathf.CeilToInt(((wavePower - 100) * 110 / 2000f + 40) * 1f);
        int totalDamage = towerDPS.CalculateDamage(seconds);
        //print("Seconds " + seconds);
        baseMaxHealth.Value = Mathf.CeilToInt(totalDamage / (float)wavePower);
        //print("Wave Size: " + numberOfZombies);
        //print("Damage: " + totalDamage);
        //print("WavePower: " + wavePower);
        //print("BaseHealth: " + baseMaxHealth.Value);
        WaitForSeconds waitForSeconds = new(spawnPeriod);
        IEnumerator spawnZombies(int count)
        {
            if (count <= 0)
            {
                if (zoneManager.IsNight)
                {
                    if (zoneManager.IsLastDayOfZone())
                    {
                        SpawnZombie(zoneBossLevel, ZombieType.ZONE_BOSS);
                    }
                    else
                    {
                        SpawnZombie(dayBossLevel, ZombieType.DAY_BOSS);
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
        switch (type)
        {
            case ZombieType.DAY_BOSS:
                print("DayBossLevel: " + level);
                zombie = dayBossPool.Get<Zombie>(position, Quaternion.identity);
                break;
            case ZombieType.ZONE_BOSS:
                zombie = zoneBossPool.Get<Zombie>(position, Quaternion.identity);
                break;
            default:
                zombie = standardZombiePool.Get<Zombie>(position, Quaternion.identity);
                break;
        }

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
        waveCleanPercentage.Value = 1f - (numberOfZombiesInWave + remainingZombiesToSpawn) / (float)(numberOfZombiesInWave + remainingZombiesToSpawn + numberOfDeadZombiesInWave);
        onWaveClearPercentChanged.Invoke();
        if (IsWaveCleared)
            OnWaveCleared();
    }

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
