using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Tweening;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using FateGames.Core;
public class TimeLapseController : FateMonoBehaviour, IInitializable
{
    [SerializeField] private FogController fogController = null;
    [SerializeField] private ZoneManager zoneManager = null;
    [SerializeField] private FloatReference transitionDuration;
    [SerializeField] private TimeLapseHeader timeLapseHeader;

    [SerializeField] private UnityEvent onTimeLapseStarted, onTimeLapseCompleted = new();

    private void Start()
    {
        Initialize();
    }
    public void Animate()
    {
        onTimeLapseStarted.Invoke();
        int targetIndex = zoneManager.WaveLevel % 4;
        AnimateHeader(targetIndex);
        fogController.SetFogToTime();
        FaTween.DelayedCall(transitionDuration.Value, () =>
        {
            zoneManager.IncrementWaveLevel();
            onTimeLapseCompleted.Invoke();
        });
    }

    private void AnimateHeader(int targetIndex)
    {
        timeLapseHeader.Animate(targetIndex);
    }

    public void Initialize()
    {
        int timeIndex = (zoneManager.WaveLevel - 1) % 4;
        AnimateHeader(timeIndex);
    }
}
