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
    private Tween timerTween = null;

    public void StartTimer()
    {
        CancelTimer();
        currentTime = time;
        timerTween = DOTween.To(() => currentTime, (float x) => currentTime = x, 0, time).OnUpdate(() =>
        {
            timeText.text = string.Format("{0:0.0}", currentTime).Replace(',', '.');
        }).OnComplete(() => { timerTween = null; });
    }
    public void CancelTimer()
    {
        if (timerTween != null)
        {
            timerTween.Kill();
            timerTween = null;
        }
    }
}
