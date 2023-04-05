using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using System;
using DG.Tweening;

public class PooledEffect : FateMonoBehaviour, IPooledObject
{
#pragma warning disable CS0108 
    [SerializeField] private ParticleSystem particleSystem;
#pragma warning restore CS0108 
    public event Action OnRelease;

    public void OnObjectSpawn()
    {
        Activate();
        particleSystem.Play();
        DOVirtual.DelayedCall(particleSystem.main.duration, Release);
    }

    public void Release()
    {
        Deactivate();
        OnRelease.Invoke();
    }

}
