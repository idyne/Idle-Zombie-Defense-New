using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class NewSoldierUnlockedScreenController : UIElement
{
    [SerializeField] private SoldierImageRendererController soldierImageRendererController = null;
    [SerializeField] private List<StringReference> soldierNames = new List<StringReference>();
    [SerializeField] private IntReference lastUnlockedSoldierLevel;
    [SerializeField] private TextMeshProUGUI soldierName = null;
    [SerializeField] private UnityEvent onClosed = null;

    public void Open()
    {
        int soldierIndex = lastUnlockedSoldierLevel.Value - 1;
        soldierImageRendererController.ShowSoldier(soldierIndex);
        soldierName.text = soldierNames[soldierIndex].Value;
        Show();
    }

    public void Close()
    {
        soldierImageRendererController.CloseShow();
        Hide();
        onClosed.Invoke();
    }
}
