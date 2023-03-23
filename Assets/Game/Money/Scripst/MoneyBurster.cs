using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "CoinBurster")]
public class MoneyBurster: ScriptableObject
{
    [SerializeField] private ObjectPool money3DPool;

    public void Burst(int gain, int numberOfCoins, Vector3 position, Vector3 direction, float forceMultiplier = 7, bool time = true)
    {
        float delay;
        if (time)
            delay = 0.3f / numberOfCoins;
        else delay = 0;
        RoutineRunner.StartRoutine(BurstCoin(gain, numberOfCoins, delay, position, direction, forceMultiplier));
    }

    private IEnumerator BurstCoin(int gain, int count, float delay, Vector3 position, Vector3 direction, float forceMultiplier = 7)
    {
        float randomScaler = 0.3f;
        int currentGain = gain / count;
        gain -= currentGain;

        Money3D coin = money3DPool.Get<Money3D>(position, Quaternion.identity);
        coin.Gain = currentGain;

        Rigidbody coinRigidbody = coin.GetComponent<Rigidbody>();
        coinRigidbody.AddForce((direction + new Vector3(Random.Range(-randomScaler, randomScaler), 0, Random.Range(-randomScaler, randomScaler))) * forceMultiplier, ForceMode.Impulse);
        coinRigidbody.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)), ForceMode.Impulse);

        yield return new WaitForSeconds(delay);

        if (--count > 0) yield return BurstCoin(gain, count, delay, position, direction, forceMultiplier);
    }
}
