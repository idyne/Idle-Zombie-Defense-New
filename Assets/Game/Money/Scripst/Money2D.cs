using FateGames.Core;
using FateGames.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money2D : FateMonoBehaviour, IPooledObject
{
    [SerializeField] protected SaveDataVariable saveData;
    [SerializeField] private Transform imageTransform;
    [SerializeField] private Vector3 target;
    public event Action OnRelease;

    public void OnObjectSpawn()
    {
        Activate();
    }

    public void Release()
    {
        Deactivate();
        OnRelease();
    }

    public void AnimateGoOnUI(Vector3 startPosiiton, int gain)
    {
        float duration = 0.6f;
        float startSize = 0.5f;
        float endSize = 1f;

        imageTransform.position = startPosiiton;
        imageTransform.localScale = Vector3.one * startSize;
        imageTransform.FaLocalScale(Vector3.one * endSize, duration);
        imageTransform.FaMove(target, duration).OnComplete(() =>
        {
            saveData.AddMoney(gain);
            Release();
        });
    }
}
