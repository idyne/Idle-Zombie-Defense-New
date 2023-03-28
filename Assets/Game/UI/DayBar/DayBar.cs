using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FateGames.Core;
using TMPro;

public class DayBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private FloatVariable percent;
    [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private TextMeshProUGUI dayText;

    private void Start()
    {
        SetDay();
    }

    public void SetDay()
    {
        dayText.text = "DAY " + zoneManager.Day;
    }

    public void SetPercent()
    {
        slider.value = percent;
    }
}
