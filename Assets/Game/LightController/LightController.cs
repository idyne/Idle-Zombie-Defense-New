using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using DG.Tweening;
using FateGames.Tweening;

public class LightController : FateMonoBehaviour, IInitializable
{
    [SerializeField] private ZoneManager zoneManager = null;
    [SerializeField] private List<Color> lightColors = new List<Color>();
    [SerializeField] private List<Vector3> lightRotations = new List<Vector3>();
    [SerializeField] private FloatReference transitionDuration;

    private Light directionalLight;

    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        directionalLight = GetComponent<Light>();
        int timeIndex = (zoneManager.WaveLevel - 1) % 4;

        directionalLight.color = lightColors[timeIndex];
        transform.rotation = Quaternion.Euler(lightRotations[timeIndex]);
    }

    public void AnimateToTargetTime()
    {
        int timeIndex = (zoneManager.WaveLevel - 1) % 4;
        FaTween.To(() => directionalLight.color, (Color x) => directionalLight.color = x, lightColors[timeIndex], transitionDuration.Value);
        transform.DORotateQuaternion(Quaternion.Euler(lightRotations[timeIndex]), transitionDuration.Value);
    }

}
