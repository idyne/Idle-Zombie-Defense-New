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
    //[SerializeField] private bool deactivateOnRelease = true;
    public event Action OnRelease;

    private void Awake()
    {
        ParticleSystem.MainModule main = particleSystem.main;
        main.playOnAwake = false;
    }

    public void OnObjectSpawn()
    {
        Activate();
        particleSystem.Stop();
        particleSystem.Play();
        Release();
    }

    public void Release()
    {
        /*if (deactivateOnRelease)
            Deactivate();*/
        OnRelease.Invoke();
    }

}
