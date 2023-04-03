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
        bool colorSet = false;
        bool startSet = false;
        bool endSet = false;
        FaTween.To(() => RenderSettings.fogColor, (Color x) => RenderSettings.fogColor = x, fogColors[newTimeIndex], duration.Value).OnComplete(() => { colorSet = true; });
        FaTween.To(() => currentStartDistanceOffset, (float x) => currentStartDistanceOffset = x, fogDistanceOffsets[newTimeIndex].x, duration.Value).OnComplete(() => { startSet = true; });
        FaTween.To(() => currentEndDistanceOffset, (float x) => currentEndDistanceOffset = x, fogDistanceOffsets[newTimeIndex].y, duration.Value).OnComplete(() => { endSet = true; });
        IEnumerator fogRoutine()
        {
            yield return new WaitUntil(() =>
            {
                OnDistanceChanged();
                return colorSet && startSet && endSet;
            });
        }
        RoutineRunner.StartRoutine(fogRoutine());
        //timeIndex = newTimeIndex;
    }
}
