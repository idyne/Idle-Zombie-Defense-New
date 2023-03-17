using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.Events;
using UnityEngine.AI;

public class WaveController : MonoBehaviour
{
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
    [SerializeField] private FloatVariable waveCleanPercentage;

    private bool spawnOnRandomPoint { get => spawnPointContainer == null || spawnPointContainer.childCount == 0; }

    private void Awake()
    {
        waveState.Value = WaveState.NONE;
        InitializeSpawnPoints();
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
        Debug.Log("StartWave", this);
        onWaveStarted.Invoke();
        waveState.Value = WaveState.STARTED;
        SpawnZombies();
    }

    public void SpawnZombies()
    {
        numberOfDeadZombiesInWave = 0;
        numberOfZombiesInWave = 0;
        waveCleanPercentage.Value = 0;
        onWaveClearPercentChanged.Invoke();
        Debug.Log("SpawnZombies", this);
        List<int> zombieTable = GenerateZombieTable(2000, 6, out int numberOfZombies);
        remainingZombiesToSpawn = numberOfZombies;
        // Spawns zombies from the zombie table in random order
        float spawnPeriod = 0.01f;
        WaitForSeconds waitForSeconds = new(spawnPeriod);
        IEnumerator spawnZombies(int count)
        {
            if (count <= 0) yield break;
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
        switch (type)
        {
            case ZombieType.DAY_BOSS:
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
        waveCleanPercentage.Value = 1f - numberOfZombiesInWave / (float)(numberOfZombiesInWave + remainingZombiesToSpawn + numberOfDeadZombiesInWave);
        onWaveClearPercentChanged.Invoke();
        if (IsWaveCleared)
            OnWaveCleared();
    }

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(Vector3.zero, spawnCircleRadius);
        Gizmos.DrawSphere(Vector3.zero, spawnCircleRadius + spawnCircleWidth);
    }
}
