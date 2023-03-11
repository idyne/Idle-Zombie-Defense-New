using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using System;

public class Soldier : Shooter, IPooledObject
{
    [SerializeField] protected Animator animator;

    public event Action OnRelease;

    public void OnObjectSpawn()
    {
        StartTargeting();
    }

    public void Release()
    {
        StopShooting();
        CancelInvoke();
        Deactivate();
    }

    public override void Shoot()
    {
#if DEBUG
        logs.Add("Shoot");
#endif
        if (!target) return;
        animator.SetTrigger("Shoot");
        gun.Use(target);
    }
}
