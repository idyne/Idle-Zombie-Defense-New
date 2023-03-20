using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using FateGames.Tweening;
using UnityEngine.UI;

public class DayOpeningScreen : UIElement
{
    [SerializeField] private TextMeshProUGUI tmpro;
    [SerializeField] private IntReference day;

    private void Start()
    {
        tmpro.text = "DAY " + day.Value.ToString();
    }
}
