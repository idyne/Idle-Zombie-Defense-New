using FateGames.Core;
using FateGames.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool2D : FateMonoBehaviour, IPooledObject
{
    [SerializeField] private RectTransform imageTransform;
    [SerializeField] private Vector2 target;
    public event Action OnRelease;
    [HideInInspector] public Action OnFinish;

    public void OnObjectSpawn()
    {
        Activate();
    }

    public void Release()
    {
        Deactivate();
        OnRelease();
    }

    public void GoUIWithBurstMove(Vector2 startPositon, Vector2 midPosition, Action finalAction)
    {
        OnFinish = finalAction;
        imageTransform.position = startPositon;
        imageTransform.localScale = Vector3.one;
        imageTransform.FaMove(midPosition, 0.5f).SetEaseFunction(FaEaseFunctions.EaseMode.OutQuint).OnComplete(() =>
        {
            float randomTimeRange = 1.5f + UnityEngine.Random.Range(-0.3f, 0.3f);
            FaTween.To(() => imageTransform.anchoredPosition, (x) => imageTransform.anchoredPosition = x, target, randomTimeRange).SetEaseFunction(FaEaseFunctions.EaseMode.InQuint).OnComplete(() =>
            {
                OnFinish();
                Release();
            });
        });
    }
}
