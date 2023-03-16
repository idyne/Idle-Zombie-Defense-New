using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using System;
using DG.Tweening;

public class PooledEffect : FateMonoBehaviour, IPooledObject
{
    [SerializeField] private ParticleSystem particleSystem;
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
