using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Tweening;
using DG.Tweening;
using TMPro;

public class AreaClearHandler : MonoBehaviour
{
    [SerializeField] private SceneManager sceneManager;
    [SerializeField] private GameObject waveClearEffect;
    [SerializeField] private GameObject dayClearScreen;
    [SerializeField] private GameObject claimButton;
    [SerializeField] private ObjectPool moneyPool;
    [SerializeField] private ObjectPool toolPool;
    [SerializeField] private RectTransform defaultSpawnPosition;
    [SerializeField] private RectTransform moneySpawnPosition;
    [SerializeField] private RectTransform toolSpawnPosition;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI toolText;
    [SerializeField] private TextMeshProUGUI dayText;

    private bool zoneEnd = false;
    private int collectableMoneyAmount = 0;
    private int collectableToolsAmount = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            WaveClear(52, 10);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            DayClear(50,5);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ZoneClear(50,5);
        }
    }

    public void Claim()
    {
        claimButton.SetActive(false);
        SpreadMoney(moneySpawnPosition.position, collectableMoneyAmount);
        SpreadTool(toolSpawnPosition.position, collectableToolsAmount);

        FaTween.DelayedCall(3f, () =>
        {
            if (zoneEnd) { } // zone end ekraný açýlacak
            else sceneManager.LoadCurrentLevel();
        });
    }

    public void WaveClear(int moneyAmount, int toolAmount)
    {
        waveClearEffect.SetActive(true);
        FaTween.DelayedCall(0.5f, () =>
        {
            SpreadMoney(defaultSpawnPosition.position, moneyAmount);
            SpreadTool(defaultSpawnPosition.position, toolAmount);
        });

        FaTween.DelayedCall(3f, () =>
        {
            waveClearEffect.SetActive(false);
        });
    }

    public void DayClear(int moneyAmount, int toolAmount)
    {
        waveClearEffect.SetActive(true);
        FaTween.DelayedCall(2f, () =>
        {
            waveClearEffect.SetActive(false);
            dayClearScreen.SetActive(true);

            collectableMoneyAmount = moneyAmount;
            collectableToolsAmount= toolAmount;
        });
    }

    public void ZoneClear(int moneyAmount, int toolAmount)
    {
        zoneEnd = true;
        DayClear(moneyAmount, toolAmount);
    }

    private void SpreadMoney(Vector2 spawnPosition, int amount)
    {
        int valueOfSingleMoneyImage = 5;
        int count = amount / valueOfSingleMoneyImage;
        int remainder = amount % valueOfSingleMoneyImage;
        if (remainder > 0) count++;

        for (int i = 0; i < count; i++)
        {
            Money2D money = moneyPool.Get<Money2D>(Vector3.zero, Quaternion.identity);

            int gain = valueOfSingleMoneyImage;
            if (i == count - 1 && remainder > 0) gain = remainder;

            float radius = Screen.width / 3f;
            Vector2 midPosition = spawnPosition + new Vector2(Random.Range(-radius, radius), Random.Range(-radius, radius));

            money.GoUIWithBurstMove(spawnPosition, midPosition, gain);
        }
    }

    private void SpreadTool(Vector2 spawnPosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Tool2D tool = toolPool.Get<Tool2D>(Vector3.zero, Quaternion.identity);

            float radius = Screen.width / 3f;
            Vector2 midPosition = spawnPosition + new Vector2(Random.Range(-radius, radius), Random.Range(-radius, radius));

            tool.GoUIWithBurstMove(spawnPosition, midPosition, 1);
        }
    }
}
