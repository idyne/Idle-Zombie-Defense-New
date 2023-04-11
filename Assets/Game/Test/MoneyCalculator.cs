using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCalculator : MonoBehaviour
{
    public int N, BaseWavePower, PowerIncrease, BaseMoney, BaseMoneyPerDay, MoneyIncreasePerDay, BaseMoneyPerWave, MoneyIncreasePerWave;
    [Header("Result")]
    public int Result = 0;
    public void Calculate()
    {
        Result = 0;
        for (int d = 1; d <= N; d++)
        {
            Result += Mathf.CeilToInt((BaseWavePower + (d - 1) * PowerIncrease) * 3.05f * BaseMoney + BaseMoneyPerDay + (MoneyIncreasePerDay * (d - 1)) + (BaseMoneyPerWave + MoneyIncreasePerWave * (d * 4 - 3)) * 3);
        }
    }

    private void OnValidate()
    {
        Calculate();
    }
}
