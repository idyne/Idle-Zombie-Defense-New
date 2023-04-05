using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewSoldierAchivedScreenController : UIElement
{
    [SerializeField] private SoldierImageRendererController soldierImageRendererController = null;
    [SerializeField] private List<StringReference> soldierNames = new List<StringReference>();
    [SerializeField] private IntReference lastAchivedSoldierLevel;
    [SerializeField] private TextMeshProUGUI soldierName = null;
    [SerializeField] private GameEvent onClosedEvent = null;
    [SerializeField] private GameManager gameManager;

    public void Open()
    {
        gameManager.PauseGame();
        int soldierIndex = lastAchivedSoldierLevel.Value - 1;
        soldierImageRendererController.ShowSoldier(soldierIndex);
        soldierName.text = soldierNames[soldierIndex].Value;
        Show();
    }

    public void RevardedClaim() 
    { 
        Close();
        // revarded video
    }

    public void Close()
    {
        soldierImageRendererController.CloseShow();
        Hide();
        gameManager.ResumeGame();
        onClosedEvent.Raise();
    }
}
