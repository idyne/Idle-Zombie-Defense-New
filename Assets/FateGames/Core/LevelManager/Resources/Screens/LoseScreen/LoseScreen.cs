using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoseScreen : UIElement
{
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private ZoneManager zoneManager;

    private void Start()
    {
        dayText.text = "DAY " + zoneManager.Day;
    }
}
