using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NewSoldierAchivedScreenController : UIElement
{
    [SerializeField] private SoldierImageRendererController soldierImageRendererController = null;
    [SerializeField] private List<StringReference> soldierNames = new List<StringReference>();
    [SerializeField] private IntReference lastAchivedSoldierLevel;
    [SerializeField] private TextMeshProUGUI soldierName = null;
    [SerializeField] private UnityEvent onClosed = null;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SoundEntity sound;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AdManager adManager;
    [SerializeField] private UnityEvent onRewardGranted;

    public void Open()
    {
        gameManager.PauseGame();
        int soldierIndex = lastAchivedSoldierLevel.Value - 1;
        soldierImageRendererController.ShowSoldier(soldierIndex);
        soldierName.text = soldierNames[soldierIndex].Value;
        Show();
        soundManager.PlaySound(sound, true);
    }

    public void RewardedClaim()
    {
        SDKManager.Instance.ShowRewardedAd(() =>
       {
           Close();
       }, () =>
         {
           Close();
           onRewardGranted.Invoke();
       });
    }

    public void Close()
    {
        soldierImageRendererController.CloseShow();
        Hide();
        gameManager.ResumeGame();
        onClosed.Invoke();
    }
}
