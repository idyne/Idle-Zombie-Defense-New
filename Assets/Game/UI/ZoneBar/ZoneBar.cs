using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.UI;

public class ZoneBar : UIElement
{
    [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private Transform barContainer;
    [Header("Colors")]
    [SerializeField] private Color pastColor, presentColor, futureColor;
    private Image[] bars;
    private int Day { get => zoneManager.NormalizedDay; }
    private int ZoneLength { get => zoneManager.ZoneLength; }

    protected override void Awake()
    {
        base.Awake();
        bars = barContainer.GetComponentsInChildren<Image>();
    }
    private void Start()
    {
        SetDay();
    }
    public void SetDay()
    {
        int presentBarIndex = Mathf.CeilToInt(Day / (float)ZoneLength * bars.Length) - 1;
        for (int i = 0; i < bars.Length; i++)
        {
            Image bar = bars[i];
            if (i < presentBarIndex)
                bar.color = pastColor;
            else if (i == presentBarIndex)
                bar.color = presentColor;
            else
                bar.color = futureColor;
        }
    }

}
