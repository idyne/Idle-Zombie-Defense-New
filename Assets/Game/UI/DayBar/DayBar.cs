using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FateGames.Core;
using TMPro;
using DG.Tweening;

public class DayBar : UIElement, IInitializable
{
    [SerializeField] private Slider slider;
    [SerializeField] private FloatVariable percent;
    [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private RectTransform container;

    private void Start()
    {
        Initialize();
    }

    public void SetDay()
    {
        dayText.text = "DAY " + zoneManager.Day;
    }

    public void SetPercent()
    {

        slider.value = ((zoneManager.WaveLevel - 1) % 4) * 0.25f + percent / 4f;
    }

    public void Initialize()
    {
        SetDay();
        SetPercent();
    }

    public void GoUp()
    {
        Vector2 position = container.anchoredPosition;
        position.y += 125;
        container.DOAnchorPos(position, 0.75f);
    }

    public void ScaleUp()
    {
        container.DOScale(Vector3.one * 1.1f, 0.75f);
    }
    public void ScaleDown()
    {
        container.DOScale(Vector3.one * 1.0f, 0.75f);
    }

    public void GoDown()
    {
        Vector2 position = container.anchoredPosition;
        position.y -= 125;
        container.DOAnchorPos(position, 0.5f);
    }
}
