using FateGames.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : Throwable, IPooledObject
{
    [SerializeField] private PreparationUpgradeEntity damageUpgrade;
    [SerializeField] private float duration = 6;
    [SerializeField] private float hitPeriod = 0.5f;
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private int baseDamagePerSecond = 5;
    [SerializeField] private LayerMask damageableLayerMask;
    [SerializeField] private GameObject meshObject;
    [SerializeField] protected ObjectPool effectPool;
    private WaitForSeconds waitForHitPeriod;
    private IEnumerator burnRoutine = null;
    private bool levelFinished = false;
    public event Action OnRelease;
    [SerializeField] private SoundEntity crackSound, burningSound;
    [SerializeField] private SoundManager soundManager;
    private SoundWorker burningSoundWorker;
    private Vector3 offset = new (0, 1, 0);
    public int damagePerSecond => baseDamagePerSecond * damageUpgrade.Level;

    private void Awake()
    {
        waitForHitPeriod = new(hitPeriod);
    }
    protected override void OnReached()
    {
        if (levelFinished) return;
        // TODO change here when implemented the pooled effects
        DeactivateMesh();
        if (effectPool)
            effectPool.Get<Transform>(transform.position + offset, Quaternion.identity);
        StartBurning();
    }

    public void OnLevelFinished()
    {
        levelFinished = true;
        Release();
    }

    private void StartBurning()
    {

        soundManager.PlaySoundOneShot(crackSound);
        burningSoundWorker = soundManager.PlaySound(burningSound);
        burnRoutine = Burn(duration);
        StartCoroutine(burnRoutine);
    }

    private void StopBurningSound()
    {
        if (burningSoundWorker == null) return;
        burningSoundWorker.Stop();
        burningSoundWorker = null;
    }

    public void StopBurning()
    {
        if (burnRoutine == null) return;
        StopBurningSound();
        StopCoroutine(burnRoutine);
        burnRoutine = null;
    }

    private IEnumerator Burn(float duration)
    {
        if (duration <= 0)
        {
            Release();
            yield break;
        }
        Burn();
        yield return waitForHitPeriod;
        burnRoutine = Burn(duration - hitPeriod);
        yield return burnRoutine;
    }

    private void Burn()
    {
        int maxColliders = 30;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders, damageableLayerMask);
        if (numColliders > 0)
        {
            for (int i = 0; i < numColliders; i++)
            {
                Damageable damageable = hitColliders[i].GetComponent<Damageable>();
                Hit(damageable);
            }
        }
    }

    private void Hit(Damageable damageable)
    {
        damageable.Hit(damagePerSecond);
    }

    private void ActivateMesh() => meshObject.SetActive(true);
    private void DeactivateMesh() => meshObject.SetActive(false);

    public void OnObjectSpawn()
    {
        levelFinished = false;
        ActivateMesh();
        Activate();
    }

    public void Release()
    {
        StopBurningSound();
        Deactivate();
        OnRelease.Invoke();
    }
}
