using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using DG.Tweening;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Reward Manager")]
public class RewardManager : ScriptableObject
{
    [SerializeField] private SoldierBuyUpgradeEntity soldierBuyUpgradeEntity;
    [SerializeField] private FloatVariable boostMultiplier;
    [SerializeField] private UnityEvent onFireRateBoostStarted, onFireRateBoostFinished;
    private Tween fireRateBoostTween;
    public void FreeSoldier()
    {
        SDKManager.Instance.ShowRewardedAd(() => { }, () =>
        {
            for (int i = 0; i < 3; i++)
            {
                soldierBuyUpgradeEntity.Upgrade();
            }
        });
    }

    public void BoostFireRate()
    {
        SDKManager.Instance.ShowRewardedAd(() => { }, () =>
        {
            CancelFireRateBoost();
            boostMultiplier.Value = 2;
            onFireRateBoostStarted.Invoke();
            fireRateBoostTween = DOVirtual.DelayedCall(60, () =>
            {
                boostMultiplier.Value = 1;
                onFireRateBoostFinished.Invoke();
                fireRateBoostTween = null;
            }).OnKill(() =>
            {
                boostMultiplier.Value = 1;
                onFireRateBoostFinished.Invoke();
                fireRateBoostTween = null;
            });
        });
    }

    public void CancelFireRateBoost()
    {
        if (fireRateBoostTween == null) return;
        fireRateBoostTween.Kill();
        fireRateBoostTween = null;
    }
}
