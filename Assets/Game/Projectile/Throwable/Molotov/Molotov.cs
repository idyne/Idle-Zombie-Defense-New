using FateGames.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : Throwable, IPooledObject
{
    [SerializeField] private float duration = 6;
    [SerializeField] private float hitPeriod = 0.5f;
    [SerializeField] private float radius = 2.5f;
    [SerializeField] private int damagePerSecond = 5;
    [SerializeField] private LayerMask damageableLayerMask;
    [SerializeField] private GameObject meshObject;
    [SerializeField] protected ObjectPool effectPool;
    private WaitForSeconds waitForHitPeriod;
    private IEnumerator burnRoutine = null;
    private bool towerDied = false;
    public event Action OnRelease;

    private void Awake()
    {
        waitForHitPeriod = new(hitPeriod);
    }
    protected override void OnReached()
    {
        // TODO change here when implemented the pooled effects
        DeactivateMesh();
        if (effectPool)
            effectPool.Get<Transform>(transform.position, Quaternion.identity);
        if (!towerDied)
            StartBurning();
    }

    public void OnTowerDied()
    {
        StopBurning();
        towerDied = true;
    }

    private void StartBurning()
    {
        burnRoutine = Burn(duration);
        StartCoroutine(burnRoutine);
    }

    public void StopBurning()
    {
        if (burnRoutine == null) return;
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
        towerDied = false;
        ActivateMesh();
        Activate();
    }

    public void Release()
    {
        Deactivate();
        OnRelease.Invoke();
    }
}
