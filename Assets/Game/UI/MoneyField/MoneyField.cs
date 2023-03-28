using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using TMPro;
using System;
using DG.Tweening;

public class MoneyField : FateMonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private SaveDataVariable saveData;
    [SerializeField] private Transform imageTransform;
    private bool animating = false;

    void Start()
    {
        SetMoney();
    }

    public void BounceMoney()
    {
        if (animating) return;
        animating = true;
        imageTransform.DOScale(1.2f, 0.05f).SetLoops(2, LoopType.Yoyo).OnComplete(() => { animating = false; });
    }

    public void SetMoney()
    {
        moneyText.text = numberFormat(saveData.Value.Money).Replace(",", ".");
    }

    public enum suffixes
    {
        p, // p is a placeholder if the value is under 1 thousand
        K, // Thousand
        M, // Million
        B, // Billion
        T, // Trillion
        Q, // Quadrillion
    }

    //Formats numbers in Millions, Billions, etc.
    public static string numberFormat(long money)
    {
        int decimals = 2; //How many decimals to round to
        string r = money.ToString(); //Get a default return value

        foreach (suffixes suffix in Enum.GetValues(typeof(suffixes))) //For each value in the suffixes enum
        {
            var currentVal = 1 * Math.Pow(10, (int)suffix * 3); //Assign the amount of digits to the base 10
            var suff = Enum.GetName(typeof(suffixes), (int)suffix); //Get the suffix value
            if ((int)suffix == 0) //If the suffix is the p placeholder
                suff = String.Empty; //set it to an empty string

            if (money >= currentVal)
                r = Math.Round((money / currentVal), decimals, MidpointRounding.ToEven).ToString() + suff; //Set the return value to a rounded value with suffix
            else
                return r; //If the value wont go anymore then return
        }
        return r; // Default Return
    }
}
