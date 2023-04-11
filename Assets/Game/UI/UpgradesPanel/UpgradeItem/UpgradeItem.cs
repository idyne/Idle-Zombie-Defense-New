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
    [SerializeField] private PreparationUpgradeEntity upgradeEntity = null;

    protected override void Awake()
    {
        base.Awake();
        if (upgradeEntity)
            Set(upgradeEntity);
    }

    public void Set(PreparationUpgradeEntity entity)
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
        icon.sprite = upgradeEntity.Icon;
        nameText.text = upgradeEntity.UpgradeName;
        description.text = upgradeEntity.Description;

        if (upgradeEntity.MaxedOut)
        {
            upgradeButton.SwitchToInfo("MAX");
        }
        else if (upgradeEntity.Locked)
        {
            Debug.Log("Locked " + upgradeEntity.UpgradeName, this);
            upgradeButton.SwitchToInfo("Locked");
        }
        else
        {
            upgradeButton.SwitchToCost(upgradeEntity.Cost, upgradeEntity.Affordable);
        }
    }
}
