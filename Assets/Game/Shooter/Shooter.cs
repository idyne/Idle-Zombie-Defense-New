using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using DG.Tweening;
using UnityEngine.Events;

public abstract class Shooter : FateMonoBehaviour
{
    [SerializeField] protected FloatVariable shootFrequencyMultiplier;
    [SerializeField] protected float range = 25;
    [SerializeField] protected float shootPeriod = 0.5f;
    [SerializeField] protected WaveStateVariable waveState;
    [SerializeField] protected ZombieSet targetableZombieSet;
    

    protected Gun gun;
    protected IEnumerator faceTargetRoutine;
    protected Zombie target;
    protected WaitUntil waitUntilReadyToShoot;
    protected WaitForSeconds waitForFaceTargetPeriod = new(0.3f);
    protected IEnumerator shootCoroutine;
    protected IEnumerator targetingRoutine;
    protected float lastShootTime = float.MinValue;
    protected float Cooldown => shootPeriod / shootFrequencyMultiplier.Value;
    protected bool InCooldown { get => Time.time < lastShootTime + Cooldown; }
    protected bool Shooting { get => shootCoroutine != null; }
    protected bool Targeting { get => targetingRoutine != null; }
    public UnityAction onTargetDied;

    protected virtual void Awake()
    {
        gun = GetComponentInChildren<Gun>();
        // Wait until  gun is not in cooldown and the shooter is faced to the target
        waitUntilReadyToShoot = new WaitUntil(() => !InCooldown && FacedTarget);
    }

    private void Update()
    {
        FaceTarget();
    }

    
    protected virtual bool FacedTarget
    {
        get
        {
            if (!target) return false;
            // Get projected forward of shooter
            Vector3 projectedForward = transform.forward;
            // Set y to 0 to project it to the ground
            projectedForward.y = 0;
            // Get projected difference between target and shooter
            Vector3 projectedDifference = target.transform.position - transform.position;
            // Set y to 0 to project it to the ground
            projectedDifference.y = 0;
            // Angle threshold for accepting the shooter is faced to the target
            float angleThreshold = 10;
            // Return whether the shooter is accepted as faced to the target
            return Vector3.Angle(projectedForward, projectedDifference) <= angleThreshold;
        }
    }

    // TODO Add listener to OnWaveStart event
    public void StartTargeting()
    {
        Log("StartTargeting", false);
        if (this.targetingRoutine != null)
        {
            Debug.LogError("Already targeting!", this);
        }
        float period = Random.Range(1.5f, 2f);
        WaitForSeconds waitForTargetingPeriod = new(period);
        IEnumerator targetingRoutine()
        {
            FindTarget();
            yield return waitForTargetingPeriod;
            this.targetingRoutine = targetingRoutine();
            yield return this.targetingRoutine;
        }
        this.targetingRoutine = targetingRoutine();
        StartCoroutine(this.targetingRoutine);
    }

    // TODO Add listener to OnWaveCleared and OnTowerDestroyed events
    public void StopTargeting()
    {
        Log("StopTargeting", false);
        if (targetingRoutine == null)
        {
            Debug.LogError("Was not targeting!", this);
            return;
        }
        StopCoroutine(targetingRoutine);
        targetingRoutine = null;
    }

    public void OnWaveCleared()
    {
        Log("OnWaveCleared", false);
        if (Targeting)
            StopTargeting();
    }

    protected Zombie FindNearestZombieInRange()
    {
        Log("FindNearestZombieInRange", false);
        // Find the nearest zombie in range
        return FindNearestZombie(range);
    }

    protected Zombie FindNearestZombie(float range = float.MaxValue)
    {
        Log("FindNearestZombie", false);
        // Find the nearest zombie in range
        Zombie nearestZombie = null;
        float minDistance = float.MaxValue;
        for (int i = 0; i < targetableZombieSet.Items.Count; i++)
        {
            Zombie zombie = targetableZombieSet.Items[i];
            float distance = Vector3.Distance(transform.position, zombie.transform.position);
            if (distance < minDistance && distance <= range)
            {
                nearestZombie = zombie;
                minDistance = distance;
            }
        }
        return nearestZombie;
    }

    public void FindTarget()
    {
        Log("FindTarget", false);
        if (target != null)
        {
            Debug.LogError("Already has a target!", this);
            return;
        }
        Zombie nearestZombieInRange = FindNearestZombieInRange();
        if (!nearestZombieInRange) return;
        StopTargeting();
        SetTarget(nearestZombieInRange);
        StartShooting();
    }

    protected void SetTarget(Zombie zombie)
    {
        Log("SetTarget", false);
        SetTarget(zombie, OnTargetDied);
    }

    protected void SetTarget(Zombie zombie, UnityAction onTargetDied)
    {
        Log("SetTarget", false);
        if (target != null)
        {
            Debug.LogError("Already has a target!", this);
            return;
        }
        target = zombie;
        this.onTargetDied = onTargetDied;
        target.OnDied.AddListener(this.onTargetDied);
    }

    public abstract void Face(Vector3 to);

    public virtual void FaceTarget()
    {
        // Called in Update()
        if (!target) return;
        Face(target.ShotPoint.position);
    }
    public virtual void StartShooting()
    {
        Log("StartShooting", false);
        if (!target)
        {
            Debug.LogError("Does not have a target!", this);
            return;
        }
        IEnumerator shootRoutine()
        {
            if (!target)
            {
                Debug.LogError("There is no target!", this);
                yield break;
            }
            yield return waitUntilReadyToShoot;
            Shoot();
            shootCoroutine = shootRoutine();
            yield return shootCoroutine;
        }
        shootCoroutine = shootRoutine();
        StartCoroutine(shootCoroutine);

    }

    public virtual void StopShooting()
    {
        Log("StopShooting", false);
        if (shootCoroutine == null)
        {
            Debug.LogError("Was not shooting!", this);
            return;
        }
        StopCoroutine(shootCoroutine);
        shootCoroutine = null;
    }

    public void RemoveTarget()
    {
        Log("RemoveTarget", false);
        if (!target)
        {
            Debug.LogError("There is no target!", this);
            return;
        }
        if (Shooting)
            StopShooting();
        target.OnDied.RemoveListener(onTargetDied);
        target = null;
    }

    public void OnTargetDied()
    {
        Log("OnTargetDied", false);
        RemoveTarget();
        if (waveState.Value == WaveController.WaveState.STARTED)
            StartTargeting();
    }


    public virtual void Shoot()
    {
        Log("Shoot", false);
        if (!target)
        {
            Debug.LogError("There is no target!", this);
            return;
        }
        gun.Shoot(target);
        lastShootTime = Time.time;
    }
}
