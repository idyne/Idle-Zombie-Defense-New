using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FateGames.Core;

public class DayBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private FloatVariable percent;

    public void SetPercent()
    {
        slider.value = percent;
    }
}
