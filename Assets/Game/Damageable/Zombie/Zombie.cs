using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using System;
using UnityEngine.Events;

public class Zombie : Damageable, IPooledObject
{
    [SerializeField] protected ZombieSet zombieSet;
    public event Action OnRelease;


    protected override void OnEnable()
    {
        base.OnEnable();

    }

    private void OnDisable()
    {
#if DEBUG
        logs.Add("OnDisable");
#endif
        zombieSet.Remove(this);
    }

    public override void AnnounceFutureDeath()
    {
        zombieSet.Remove(this);
        base.AnnounceFutureDeath();
    }


    public override void Die()
    {
#if DEBUG
        logs.Add("Die");
#endif
        Release();
        OnDied.Invoke();
    }

    public void OnObjectSpawn()
    {
#if DEBUG
        logs.Add("OnObjectSpawn");
#endif
        zombieSet.Add(this);
    }

    public void Release()
    {
        Deactivate();
#if DEBUG
        logs.Add("Release");
#endif
    }

}
