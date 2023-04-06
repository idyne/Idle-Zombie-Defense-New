using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FateGames.Core;

public class LoseScreen : UIElement
{
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private SoundEntity sound;
    [SerializeField] private SoundManager soundManager;

    private void Start()
    {
        soundManager.PlaySound(sound);
        dayText.text = "DAY " + zoneManager.Day;
    }
}
