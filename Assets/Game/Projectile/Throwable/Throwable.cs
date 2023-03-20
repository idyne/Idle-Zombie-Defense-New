using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using FateGames.Tweening;
using System;

public abstract class Throwable : FateMonoBehaviour
{
    [SerializeField] private float speed = 20;

    public void Throw(Vector3 to)
    {
        float time = 2;
        transform.FaProjectileMotion(to, time, gravity: -25).OnComplete(OnReached);
    }

    protected abstract void OnReached();
}
