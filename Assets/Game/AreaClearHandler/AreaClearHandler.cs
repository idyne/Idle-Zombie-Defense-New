using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Tweening;
using DG.Tweening;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class AreaClearHandler : UIElement
{
    [SerializeField] private int toolPerWave = 2;
    [SerializeField] private int toolPerDay = 5;
    [SerializeField] private int toolPerZone = 10;
    [SerializeField] private int moneyPerWave = 50;
    [SerializeField] private int moneyPerDay = 200;
    [SerializeField] private int moneyPerZone = 1000;
    [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private SceneManager sceneManager;
    [SerializeField] private SaveDataVariable saveData;
    [SerializeField] private GameObject waveClearEffect;
    [SerializeField] private Transform waveClearText;
    [SerializeField] private GameObject dayClearScreen;
    [SerializeField] private Button claimButton;
    [SerializeField] private ObjectPool moneyPool;
    [SerializeField] private ObjectPool toolPool;
    [SerializeField] private RectTransform defaultSpawnPosition;
    [SerializeField] private RectTransform moneySpawnPosition;
    [SerializeField] private RectTransform toolSpawnPosition;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI toolText;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private UnityEvent onZoneFinished;
    [SerializeField] private UnityEvent onWaveClearEffectFinished;
    [SerializeField] private SoldierUnlockTable soldierUnlockTable;
    [SerializeField] private UnityEvent onNewSoldierUnlocked;
    [SerializeField] private IntVariable lastUnlockedSoldierLevel;
    [SerializeField] private SoundEntity waveClearSound, areaClearSound;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private AdManager adManager;

    private int collectableMoneyAmount = 0;
    private int collectableToolsAmount = 0;
    private bool mapClosed = false;
    private bool soldierUnlockedScreenClosed = false;

    public void ShowCorrectScreen()
    {
        if (zoneManager.IsNight)
        {
            if (zoneManager.IsLastDayOfZone())
                ZoneClear(moneyPerZone, toolPerZone);
            else
                DayClear(moneyPerDay, toolPerDay);
        }
        else
            WaveClear(moneyPerWave, toolPerWave);
    }

    public void Claim()
    {
        claimButton.interactable = false;
        SpreadMoney(moneySpawnPosition.position, collectableMoneyAmount);
        SpreadTool(toolSpawnPosition.position, collectableToolsAmount);

        FaTween.DelayedCall(3f, () =>
        {
            Hide();
            IEnumerator routine()
            {
                if (zoneManager.IsLastDayOfGame() && zoneManager.IsNight)
                {
                    zoneManager.ResetGame();
                    sceneManager.LoadCurrentLevel();
                    yield break;
                }
                if (zoneManager.IsLastDayOfZone())
                {
                    onZoneFinished.Invoke();
                    yield return new WaitUntil(() => mapClosed);
                }
                if (CheckSoldierUnlocked())
                    yield return new WaitUntil(() => soldierUnlockedScreenClosed);
                zoneManager.IncrementWaveLevel();
                yield return adManager.ShowInterstitial();
                sceneManager.LoadCurrentLevel();
            }


            StartCoroutine(routine());
        });
    }

    public bool CheckSoldierUnlocked()
    {
        SoldierUnlockTable.SoldierUnlockEntity entity = soldierUnlockTable[zoneManager.Day];
        if (entity != null)
        {
            lastUnlockedSoldierLevel.Value = entity.SoldierLevel;
            onNewSoldierUnlocked.Invoke();
            return true;
        }
        return false;
    }

    public void OnSoldierUnlockedScreenClosed()
    {
        soldierUnlockedScreenClosed = true;
    }

    public void OnMapClosed()
    {
        mapClosed = true;
    }

    public void WaveClear(int moneyAmount, int toolAmount)
    {
        WaveClearEffect(2);
        SpreadMoney(defaultSpawnPosition.position, moneyAmount);
        SpreadTool(defaultSpawnPosition.position, toolAmount);
        FaTween.DelayedCall(2f, onWaveClearEffectFinished.Invoke);
    }

    public void DayClear(int moneyAmount, int toolAmount)
    {
        WaveClearEffect(2);
        FaTween.DelayedCall(2f, () =>
        {
            soundManager.PlaySound(areaClearSound);
            waveClearEffect.SetActive(false);
            dayClearScreen.SetActive(true);

            collectableMoneyAmount = moneyAmount;
            collectableToolsAmount = toolAmount;
            moneyText.text = moneyAmount.ToString();
            toolText.text = toolAmount.ToString();

            string dayCompleteText = "DAY " + zoneManager.Day + "\nCOMPLETED";
            if (zoneManager.IsLastDayOfZone()) dayCompleteText = "ZONE " + zoneManager.Zone + "\nCOMPLETED";
            dayText.text = dayCompleteText;
        });
    }

    public void ZoneClear(int moneyAmount, int toolAmount)
    {
        DayClear(moneyAmount, toolAmount);
    }

    private void WaveClearEffect(float duration)
    {
        soundManager.PlaySound(waveClearSound);
        Debug.Log("playwavesound", this);
        waveClearEffect.SetActive(true);
        waveClearText.localScale = Vector3.one * 0f;
        waveClearText.FaLocalScale(Vector3.one * 1f, duration / 3).SetEaseFunction(FaEaseFunctions.EaseMode.OutQuad);
        FaTween.DelayedCall(duration * 2 / 3, () =>
        {
            waveClearText.FaLocalScale(Vector3.one * 0f, duration / 3).SetEaseFunction(FaEaseFunctions.EaseMode.InQuad).OnComplete(() =>
            {
                waveClearEffect.SetActive(false);
            });
        });
    }

    private void SpreadMoney(Vector2 spawnPosition, int amount)
    {
        int valueOfSingleMoneyImage = 10;
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

            money.GoUIWithBurstMove(spawnPosition, midPosition, () => saveData.AddMoney(gain));
        }
    }

    private void SpreadTool(Vector2 spawnPosition, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Tool2D tool = toolPool.Get<Tool2D>(Vector3.zero, Quaternion.identity);

            float radius = Screen.width / 3f;
            Vector2 midPosition = spawnPosition + new Vector2(Random.Range(-radius, radius), Random.Range(-radius, radius));

            tool.GoUIWithBurstMove(spawnPosition, midPosition, () => saveData.AddTools(1));
        }
    }
}
