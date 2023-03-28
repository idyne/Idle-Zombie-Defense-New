using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.UI;
using TMPro;
public class UpgradeItem : DynamicUIElement
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText, description, costText;
    [SerializeField] private UpgradeButton upgradeButton;
    private UpgradeEntity upgradeEntity = null;

    public void Set(UpgradeEntity entity)
    {
        Unset();
        upgradeEntity = entity;
        upgradeEntity.OnUpgrade.AddListener(UpdateElement);
        upgradeButton.OnClick.AddListener(upgradeEntity.BuyUpgrade);
        UpdateElement();
    }

    public void Unset()
    {
        if (upgradeEntity)
        {
            upgradeEntity.OnUpgrade.RemoveListener(UpdateElement);
            upgradeButton.OnClick.RemoveListener(upgradeEntity.BuyUpgrade);
        }
    }

    public override void UpdateElement()
    {
        Debug.Log("UpdateElement", this);
        icon.sprite = upgradeEntity.Icon;
        nameText.text = upgradeEntity.UpgradeName;
        description.text = upgradeEntity.Description;

        if (upgradeEntity.MaxedOut)
        {
            upgradeButton.SwitchToInfo("MAX");
        }
        else if (upgradeEntity.Locked)
        {
            upgradeButton.SwitchToInfo("Locked");
        }
        else
        {
            upgradeButton.SwitchToCost(upgradeEntity.Cost);
        }
    }
}
