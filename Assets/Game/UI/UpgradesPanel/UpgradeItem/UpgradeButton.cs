using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : UIElement
{
    [SerializeField] private SaveDataVariable saveData;
    [SerializeField] private Button button;
    [SerializeField] private GameObject costField, infoField;
    [SerializeField] private TextMeshProUGUI moneyText, infoText;
    public Button.ButtonClickedEvent OnClick { get => button.onClick; }

    public void SwitchToInfo(string message)
    {
        button.interactable = false;
        infoText.text = message;
        infoField.SetActive(true);
        costField.SetActive(false);
    }
    public void SwitchToCost(int cost)
    {
        button.interactable = saveData.CanAffordTools(cost);
        moneyText.text = cost.ToString();
        infoField.SetActive(false);
        costField.SetActive(true);
    }
}
