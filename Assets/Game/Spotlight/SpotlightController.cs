using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class SpotlightController : FateMonoBehaviour, IInitializable
{
    [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private Light spotlight;
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        spotlight.enabled = zoneManager.IsNight;
    }

    public void OnTimeLapseCompleted()
    {
        spotlight.enabled = zoneManager.IsNight;
    }

}
