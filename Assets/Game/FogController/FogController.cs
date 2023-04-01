using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Tweening;

[CreateAssetMenu(menuName = "FogController")]
public class FogController : ScriptableObject
{
    [SerializeField] private FloatReference cameraDistance;
    [SerializeField] private List<Color> fogColors = new List<Color>();
    [SerializeField] private List<Vector2> fogDistanceOffsets = new List<Vector2>();
    [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private FloatReference duration;

    private float currentStartDistanceOffset;
    private float currentEndDistanceOffset;

    //private int timeIndex = 0;

    public void Init()
    {
        int timeIndex = (zoneManager.WaveLevel - 1) % 4;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogColor = fogColors[timeIndex];
        currentStartDistanceOffset = fogDistanceOffsets[timeIndex].x;
        currentEndDistanceOffset = fogDistanceOffsets[timeIndex].y;
        //this.timeIndex = timeIndex;
    }

    public void OnDistanceChanged()
    {
        RenderSettings.fogStartDistance = cameraDistance.Value + currentStartDistanceOffset;
        RenderSettings.fogEndDistance = cameraDistance.Value + currentEndDistanceOffset;

        /*RenderSettings.fogStartDistance = cameraDistance.Value + fogDistanceOffsets[timeIndex].x;
        RenderSettings.fogEndDistance = cameraDistance.Value + fogDistanceOffsets[timeIndex].y;
        RenderSettings.fogColor = fogColors[timeIndex];*/
    }

    public void SetFogToTime()
    {
        int newTimeIndex = zoneManager.WaveLevel % 4;
        FaTween.To(() => RenderSettings.fogColor, (Color x) => RenderSettings.fogColor = x, fogColors[newTimeIndex], duration.Value);
        FaTween.To(() => currentStartDistanceOffset, (float x) => currentStartDistanceOffset = x, fogDistanceOffsets[newTimeIndex].x, duration.Value);
        FaTween.To(() => currentEndDistanceOffset, (float x) => currentEndDistanceOffset = x, fogDistanceOffsets[newTimeIndex].y, duration.Value);
        OnDistanceChanged();
        //timeIndex = newTimeIndex;
    }
}
