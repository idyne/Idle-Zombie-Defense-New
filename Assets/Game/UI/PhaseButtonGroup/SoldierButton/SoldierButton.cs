using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.Events;

public class SoldierButton : PhaseButton
{
    [SerializeField] private SaveDataVariable saveData;
    [SerializeField] private BoolVariable isTowerFull;
    [SerializeField] private UnityEvent onSoldierCanBeBought;
    [SerializeField] private GameObject freeField;

    

    private void OnEnable()
    {
        saveData.OnMoneyChanged.AddListener(CheckSoldierCanBeBought);
        isTowerFull.OnValueChanged.AddListener(UpdateElement);
    }
    private void OnDisable()
    {
        saveData.OnMoneyChanged.RemoveListener(CheckSoldierCanBeBought);
        isTowerFull.OnValueChanged.RemoveListener(UpdateElement);
    }

    public void CheckSoldierCanBeBought(int previous, int current)
    {
        if (current >= phaseUpgrade.Cost && previous < phaseUpgrade.Cost) 
            onSoldierCanBeBought.Invoke();
    }

    public void UpdateElement(bool a, bool b)
    {
        UpdateElement();
    }
    public override void UpdateElement()
    {
        nameText.text = phaseUpgrade.UpgradeName;
        if (phaseUpgrade.MaxedOut || isTowerFull.Value)
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

    public override void SwitchToCost(int cost, bool affordable)
    {
        base.SwitchToCost(cost, affordable);
        //freeField.SetActive(!affordable && saveData.Value.SoldierBuyingTutorialPassed);
    }
}
