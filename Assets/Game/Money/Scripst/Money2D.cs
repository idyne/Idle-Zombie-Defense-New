using FateGames.Core;
using FateGames.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money2D : FateMonoBehaviour, IPooledObject
{
    [SerializeField] protected SaveDataVariable saveData;
    [SerializeField] private RectTransform imageTransform;
    [SerializeField] private Vector2 target;
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

    public void DirectGoToUI(Vector3 startPositon, int gain)
    {
        float duration = 0.6f;
        float startSize = 0.5f;
        float endSize = 1f;

        imageTransform.position = startPositon;
        imageTransform.localScale = Vector3.one * startSize;
        imageTransform.FaLocalScale(Vector3.one * endSize, duration);
        FaTween.To(() => imageTransform.anchoredPosition, (x) => imageTransform.anchoredPosition = x, target, duration).OnComplete(() =>
        {
            saveData.AddMoney(gain);
            Release();
        });
    }

    public void GoUIWithBurstMove(Vector2 startPositon, Vector2 midPosition, int gain)
    {
        imageTransform.position = startPositon;
        imageTransform.localScale = Vector3.one;
        imageTransform.FaMove(midPosition, 0.5f).SetEaseFunction(FaEaseFunctions.EaseMode.OutQuint).OnComplete(() =>
        {
            float randomTimeRange = 1.5f + UnityEngine.Random.Range(-0.3f, 0.3f);
            FaTween.To(() => imageTransform.anchoredPosition, (x) => imageTransform.anchoredPosition = x, target, randomTimeRange).SetEaseFunction(FaEaseFunctions.EaseMode.InQuint).OnComplete(() =>
            {
                saveData.AddMoney(gain);
                Release();
            });
        });
    }
}
