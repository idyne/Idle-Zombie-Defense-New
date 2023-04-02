using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewSoldierUnlockedScreenController : MonoBehaviour
{
    [SerializeField] private SoldierImageRendererController soldierImageRendererController = null;
    [SerializeField] private List<StringReference> soldierNames = new List<StringReference>();
    [SerializeField] private IntReference lastUnlockedSoldierLevel;
    [SerializeField] private Canvas rootCanvas = null;
    [SerializeField] private TextMeshProUGUI soldierName = null;

    public void Open()
    {
        int soldierIndex = lastUnlockedSoldierLevel.Value - 1;
        soldierImageRendererController.ShowSoldier(soldierIndex);
        soldierName.text = soldierNames[soldierIndex].Value;
        rootCanvas.enabled = true;
    }

    public void Close()
    {
        soldierImageRendererController.CloseShow();
        rootCanvas.enabled = false;
    }
}
