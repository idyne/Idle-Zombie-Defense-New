using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using DG.Tweening;
public class Shooter : FateMonoBehaviour
{
    [SerializeField] protected float range = 10;
    [SerializeField] protected ZombieSet zombieSet;
    [SerializeField] protected WaveStateVariable waveState;
    [SerializeField] protected Gun gun;
    protected Zombie target;
    protected WaitUntil waitUntilReadyToShoot;
    protected IEnumerator shootCoroutine;
#if DEBUG
    public List<string> logs = new();
#endif
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

    private void OnEnable()
    {
#if DEBUG
        logs.Add("OnEnable");
#endif
        // Wait until  gun is not in cooldown and the shooter is faced to the target
        waitUntilReadyToShoot = new WaitUntil(() => !gun.InCooldown && FacedTarget);
    }

    // TODO Add listener to OnWaveStart event
    public void StartTargeting()
    {
#if DEBUG
        logs.Add("StartTargeting");
#endif
        if (waveState.Value != Wave.WaveState.STARTED) return;
        float period = Random.Range(1.5f, 2f);
        InvokeRepeating(nameof(SetTarget), 0, period);
    }

    // TODO Add listener to OnWaveCleared and OnTowerDestroyed events
    public void StopTargeting()
    {
#if DEBUG
        logs.Add("StopTargeting");
#endif
        CancelInvoke(nameof(SetTarget));
    }

    protected Zombie FindNearestZombieInRange()
    {
#if DEBUG
        logs.Add("FindNearestZombieInRange");
#endif
        // Find the nearest zombie in range
        Zombie nearestZombie = null;
        float minDistance = float.MaxValue;
        for (int i = 0; i < zombieSet.Items.Count; i++)
        {
            Zombie zombie = zombieSet.Items[i];
            float distance = Vector3.Distance(transform.position, zombie.transform.position);
            if (distance < minDistance && distance <= range)
            {
                nearestZombie = zombie;
                minDistance = distance;
            }
        }
        return nearestZombie;
    }



    public void SetTarget()
    {
#if DEBUG
        logs.Add("SetTarget");
#endif
        // Set target to the nearest zombie
        target = FindNearestZombieInRange();
        // Return if there is no zombie in range
        if (!target) return;
        StopTargeting();
        target.OnDied.AddListener(OnTargetDied);
        target.OnGoingToDie.AddListener(OnTargetGoingToDie);
        // TODO face target
        FaceTarget();
        // Start shooting
        StartShooting();
    }

    public void FaceTarget()
    {
        Vector3 direction = target.ShotPoint.position - transform.position;
        direction.y = 0;
        transform.DORotateQuaternion(Quaternion.LookRotation(direction), 0.2f);
    }

    public void StartShooting()
    {
#if DEBUG
        logs.Add("StartShooting");
#endif
        if (target)
        {
            shootCoroutine = ShootRoutine();
            StartCoroutine(shootCoroutine);
        }
    }

    public void StopShooting()
    {
#if DEBUG
        logs.Add("StopShooting");
#endif
        StopCoroutine(shootCoroutine);
    }

    public void RemoveTarget()
    {
#if DEBUG
        logs.Add("RemoveTarget");
#endif
        if (!target) return;
        StopShooting();
        target.OnDied.RemoveListener(OnTargetDied);
        target.OnGoingToDie.RemoveListener(OnTargetGoingToDie);
        target = null;
    }

    public void OnTargetDied()
    {
#if DEBUG
        logs.Add("OnTargetDied");
#endif
        RemoveTarget();
        StartTargeting();
    }

    public void OnTargetGoingToDie()
    {
#if DEBUG
        logs.Add("OnTargetGoingToDie");
#endif
        RemoveTarget();
        StartTargeting();
    }

    public virtual void Shoot()
    {
#if DEBUG
        logs.Add("Shoot");
#endif
        if (!target) return;
        gun.Use(target);
    }

    private IEnumerator ShootRoutine()
    {
#if DEBUG
        logs.Add("ShootRoutine");
#endif
        if (!target)
            Debug.LogError("There is no target!", this);
        yield return waitUntilReadyToShoot;
        Shoot();
        shootCoroutine = ShootRoutine();
        yield return shootCoroutine;
    }
}
