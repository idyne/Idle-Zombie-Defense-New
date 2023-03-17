using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using System;
using DG.Tweening;

public class Soldier : Shooter, IPooledObject
{
    [SerializeField] protected SoldierSet soldierSet;
    [SerializeField] protected Animator animator;


    public event Action OnRelease;

    protected void OnEnable()
    {
        Log("OnEnable", false);
        soldierSet.Add(this);
    }

    protected virtual void OnDisable()
    {
        Log("OnDisable", false);
        soldierSet.Remove(this);
    }

    public void SetPosition(Vector3 position)
    {
        Log("SetPosition", false);
        transform.position = position;
    }
    public void OnObjectSpawn()
    {
        Log("OnObjectSpawn", false);
        Activate();
        if (waveState.Value == WaveController.WaveState.STARTED)
            StartTargeting();
    }

    public void Release()
    {
        Log("Release", false);
        if (Targeting)
            StopTargeting();
        if (target)
            RemoveTarget();
        Deactivate();
        OnRelease.Invoke();
    }


    public override void Shoot()
    {
        animator.SetTrigger("Shoot");
        base.Shoot();
    }

    public override void Face(Vector3 to)
    {
        // Called in Update()
        Vector3 direction = to - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 3f);
    }

    public virtual void Die()
    {
        if (Targeting)
            StopTargeting();
        if (target)
            RemoveTarget();
    }
}
