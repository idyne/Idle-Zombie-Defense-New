using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FateGames.Tweening;
using UnityEngine.UI;

public class DayOpeningScreen : UIElement
{
    [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private TextMeshProUGUI tmpro;

    private void Start()
    {
        tmpro.text = "DAY " + zoneManager.Day;
    }
}
