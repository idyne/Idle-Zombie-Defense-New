using FateGames.Core;
using FateGames.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money2D : UIElement, IPooledObject
{
    [SerializeField] private Money2DRuntimeSet runtimeSet;
    [SerializeField] private RectTransform imageTransform;
    [SerializeField] private Vector2 target;
    public event Action OnRelease;
    [HideInInspector] public Action OnFinish;
    [SerializeField] private BoolVariable developmentUIOn;

    public void OnObjectSpawn()
    {
        if (developmentUIOn.Value) Show();
        else Hide();
        Activate();
        runtimeSet.Add(this);
    }

    public void Release()
    {
        Deactivate();
        runtimeSet.Remove(this);
        OnRelease();
    }

    public void DirectGoToUI(Vector3 startPositon, Action finalAction)
    {
        OnFinish = finalAction;
        float duration = 0.6f;
        float startSize = 0.5f;
        float endSize = 1f;

        imageTransform.position = startPositon;
        imageTransform.localScale = Vector3.one * startSize;
        imageTransform.FaLocalScale(Vector3.one * endSize, duration);
        FaTween.To(() => imageTransform.anchoredPosition, (x) => imageTransform.anchoredPosition = x, target, duration).OnComplete(() =>
        {
            Finish();
        });
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
                Finish();
            });
        });
    }

    public void Finish()
    {
        OnFinish();
        Release();
    }

    private void OnDisable()
    {
        runtimeSet.Remove(this);
    }
}
