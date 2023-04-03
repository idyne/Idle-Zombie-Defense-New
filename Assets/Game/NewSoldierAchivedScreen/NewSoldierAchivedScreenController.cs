using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewSoldierAchivedScreenController : MonoBehaviour
{
    [SerializeField] private SoldierImageRendererController soldierImageRendererController = null;
    [SerializeField] private List<StringReference> soldierNames = new List<StringReference>();
    [SerializeField] private IntReference lastAchivedSoldierLevel;
    [SerializeField] private Canvas rootCanvas = null;
    [SerializeField] private TextMeshProUGUI soldierName = null;
    [SerializeField] private GameEvent onClosedEvent = null;

    public void Open()
    {
        int soldierIndex = lastAchivedSoldierLevel.Value - 1;
        soldierImageRendererController.ShowSoldier(soldierIndex);
        soldierName.text = soldierNames[soldierIndex].Value;
        rootCanvas.enabled = true;
    }

    public void RevardedClaim() 
    { 
        Close();
        // revarded video
    }

    public void Close()
    {
        soldierImageRendererController.CloseShow();
        rootCanvas.enabled = false;
        onClosedEvent.Raise();
    }
}
