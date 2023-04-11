using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "CoinBurster")]
public class MoneyBurster : ScriptableObject
{
    [SerializeField] private int totalLimit = 30;
    [SerializeField] private int quotaOn2D = 10;
    [SerializeField] private SaveDataVariable saveData;
    [SerializeField] private ObjectPool money3DPool;
    [SerializeField] private Money2DRuntimeSet money2DRuntimeSet;
    [SerializeField] private Money3DRuntimeSet money3DRuntimeSet;

    public void Burst(int gain, Vector3 position, bool time = true)
    {
        int numberOfCoins = 3;
        int needIn2D = quotaOn2D - money2DRuntimeSet.Items.Count;
        int remain = totalLimit - money2DRuntimeSet.Items.Count - money3DRuntimeSet.Items.Count;

        int need = numberOfCoins - remain;

        for (int i = 0; i < need; i++)
        {
            money3DRuntimeSet.Items[needIn2D].Finish();
        }

        float delay;
        if (time)
            delay = 0.3f / numberOfCoins;
        else delay = 0;
        RoutineRunner.StartRoutine(BurstCoin(gain, numberOfCoins, delay, position));
    }

    private IEnumerator BurstCoin(int gain, int count, float delay, Vector3 position)
    {

        int currentGain = gain / count;
        gain -= currentGain;

        Money3D coin = money3DPool.Get<Money3D>(position, Quaternion.identity);
        coin.OnFinish = () => saveData.AddMoney(currentGain);

        Rigidbody coinRigidbody = coin.GetComponent<Rigidbody>();


        yield return new WaitForSeconds(delay);

        if (--count > 0) yield return BurstCoin(gain, count, delay, position);
    }
}
