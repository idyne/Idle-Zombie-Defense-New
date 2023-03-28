using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.UI;

public class ZoneBar : UIElement
{
    [SerializeField] private Transform barContainer;
    [Header("Colors")]
    [SerializeField] private Color pastColor, presentColor, futureColor;
    private Image[] bars;
    //TODO
    private int Day { get => 6; }
    private int ZoneLength { get => 7; }

    private void Awake()
    {
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
