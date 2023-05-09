using DG.Tweening;
using FateGames.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DataEditController : MonoBehaviour
{
    [SerializeField] private DevelopmentEditScreen editScreen;
    [SerializeField] private TextMeshProUGUI moneyText = null;
    [SerializeField] private TextMeshProUGUI toolsText = null;
    [SerializeField] private TextMeshProUGUI dayText = null;
    [SerializeField] private SaveDataVariable saveData;
    [SerializeField] private GameObject UIParent = null;
    [SerializeField] private BoolVariable UIOn;
    [SerializeField] private SceneManager sceneManager;
    [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private UnityEvent onUITurnedOn, onUITurnedOff;

    private void Start()
    {
        UIOn.Value = true;
    }

    public void SetMoney()
    {
        Int32.TryParse(moneyText.text[..^1], out int result);
        saveData.SetMoney(result);
    }
    public void SetTools()
    {
        Int32.TryParse(toolsText.text[..^1], out int result);
        saveData.SetTools(result);
    }
    public void SetDay()
    {
        Int32.TryParse(dayText.text[..^1], out int result);
        result = Mathf.Clamp(result, 1, zoneManager.NumberOfDays);
        saveData.Value.WaveLevel = result * 4 - 3;
        editScreen.ClosePanel();
        sceneManager.LoadCurrentLevel();
    }

    public void ToggleUI()
    {
        UIParent.SetActive(!UIParent.activeSelf);
        if (UIParent.activeSelf)
            onUITurnedOn.Invoke();
        else
            onUITurnedOff.Invoke();
        UIOn.Value = UIParent.activeSelf;

    }

}
