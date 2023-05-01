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
    [SerializeField] private SceneManager sceneManager;
    [SerializeField] private AdManager adManager;

    private void Start()
    {
        soundManager.PlaySound(sound);
        dayText.text = "DAY " + zoneManager.Day;
    }
    public void Continue()
    {
        IEnumerator routine()
        {
            zoneManager.ResetWaveLevelToDay();
            /*if (RemoteConfigValues.show_int_if_fail)
                yield return adManager.ShowInterstitial();*/
            sceneManager.LoadCurrentLevel();
            yield break;
        }
        StartCoroutine(routine());
    }
}
