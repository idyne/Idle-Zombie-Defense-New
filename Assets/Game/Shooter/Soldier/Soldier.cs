using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using System;
using DG.Tweening;

public class Soldier : Shooter, IPooledObject
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected SoldierSet soldierSet;


    public event Action OnRelease;

    protected override void OnEnable()
    {
        base.OnEnable();
        soldierSet.Add(this);
    }

    protected virtual void OnDisable()
    {
#if DEBUG
        logs.Add("OnDisable");
#endif
        soldierSet.Remove(this);
    }

    public void SetPosition(Vector3 position)
    {
#if DEBUG
        logs.Add("SetPosition");
#endif
        transform.position = position;
    }
    public void OnObjectSpawn()
    {
#if DEBUG
        logs.Add("SetPosition");
#endif
        Activate();
        StartTargeting();
    }

    public void Release()
    {
#if DEBUG
        logs.Add("Release");
#endif
        RemoveTarget();
        StopShooting();
        CancelInvoke();
        Deactivate();
        OnRelease.Invoke();
    }


    public override IEnumerator Shoot()
    {
#if DEBUG
        logs.Add("Shoot");
#endif
        if (!target) yield break;
        animator.SetTrigger("Shoot");
        yield return gun.Use(target);
    }

    public override IEnumerator FaceTargetRoutine()
    {
#if DEBUG
        logs.Add("FaceTarget");
#endif
        Face(target.ShotPoint.position, 0.2f);
        yield return waitForFaceTargetPeriod;
        faceTargetRoutine = FaceTargetRoutine();
        yield return faceTargetRoutine;
    }

    public void Face(Vector3 to, float time)
    {
        Vector3 direction = to - transform.position;
        direction.y = 0;
        transform.DORotateQuaternion(Quaternion.LookRotation(direction), time);
    }

    public virtual void Die()
    {
        StopTargeting();
        StopShooting();
    }
}
