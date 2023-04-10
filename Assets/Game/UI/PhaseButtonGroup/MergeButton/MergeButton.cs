using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using TMPro;
using UnityEngine.UI;

public class MergeButton : DynamicUIElement
{
    [SerializeField] protected PhaseUpgradeEntity phaseUpgrade;
    [SerializeField] private SaveDataVariable saveData;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Button button;

    protected override void Awake()
    {
        base.Awake();
        phaseUpgrade.OnUpgrade.AddListener(UpdateElement);
        UpdateElement();
    }

    private void OnEnable()
    {
        Hide();
    }

    public override void Show()
    {
        UpdateElement();
        base.Show();
    }

    public override void UpdateElement()
    {
        button.interactable = saveData.CanAffordMoney(phaseUpgrade.Cost);
        moneyText.text = MoneyField.numberFormat(phaseUpgrade.Cost);
    }


}

