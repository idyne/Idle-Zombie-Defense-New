using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using TMPro;
using UnityEngine.UI;

public class PhaseButton : DynamicUIElement
{
    [SerializeField] protected PhaseUpgradeEntity phaseUpgrade;
    [SerializeField] protected GameObject infoField, costField;
    [SerializeField] protected TextMeshProUGUI infoText, costText, nameText;
    [SerializeField] protected Button button;
    [SerializeField] protected GameObject freeField;

    protected override void Awake()
    {
        base.Awake();
        phaseUpgrade.OnUpgrade.AddListener(UpdateElement);
        button.onClick.AddListener(phaseUpgrade.BuyUpgrade);
        UpdateElement();
    }

    public override void UpdateElement()
    {
        nameText.text = phaseUpgrade.UpgradeName;
        if (phaseUpgrade.MaxedOut)
        {
            SwitchToInfo("MAX");
        }
        else if (phaseUpgrade.Locked)
        {
            SwitchToInfo("Locked");
        }
        else
        {
            SwitchToCost(phaseUpgrade.Cost, phaseUpgrade.Affordable);
        }
    }

    public void SwitchToInfo(string message)
    {
        button.interactable = false;
        infoText.text = message;
        infoField.SetActive(true);
        costField.SetActive(false);
    }
    public virtual void SwitchToCost(int cost, bool affordable)
    {
        button.interactable = affordable;
        costText.text = MoneyField.numberFormat(cost);
        infoField.SetActive(false);
        costField.SetActive(true);
        freeField.SetActive(SDKManager.Instance.IsGraceTimePassed && !affordable);
    }
}
