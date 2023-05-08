using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class FireRateBoostTimer : MonoBehaviour
{
    [SerializeField] private float time = 60;
    [SerializeField] private TextMeshProUGUI timeText;
    private float currentTime = 60;

    public void StartTimer()
    {
        currentTime = time;
        DOTween.To(() => currentTime, (float x) => currentTime = x, 0, time).OnUpdate(() =>
        {
            timeText.text = string.Format("{0:0.0}", currentTime).Replace(',', '.');
        });
    }
}
