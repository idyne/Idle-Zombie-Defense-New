using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using System;
using UnityEngine.Events;

public class Zombie : Damageable, IPooledObject
{
    [SerializeField] protected SaveDataVariable saveData;
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
        DropMoney();
        Release();
        OnDied.Invoke();
    }

    public void DropMoney()
    {
        // TODO change to real money
        int money = 50001;
        saveData.AddMoney(money);
    }

    public void OnObjectSpawn()
    {
#if DEBUG
        logs.Add("OnObjectSpawn");
#endif
        Activate();
        zombieSet.Add(this);
    }

    public void Release()
    {
#if DEBUG
        logs.Add("Release");
#endif
        Deactivate();
        OnRelease.Invoke();
    }

}
