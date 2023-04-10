using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

[CreateAssetMenu(menuName = "Ad Manager")]
public class AdManager : ScriptableObject
{
    [SerializeField] private GameManager gameManager;
    public IEnumerator ShowInterstitial()
    {
        yield break;
        if (AdvertisementManager.IsCanShowInterstital && AdvertisementManager.IsInterstitialdAdReady())
        {
            bool isAdDone = false;
            void ContinueAfterAd() { isAdDone = true; gameManager.ResumeGame(); }
            AdvertisementManager.ShowInterstitial(OnStartAdEvent: gameManager.PauseGame, OnFinishAdEvent: ContinueAfterAd, OnFailedAdEvent: ContinueAfterAd);
            yield return new WaitUntil(() => isAdDone);
        }
    }
    public IEnumerator ShowRewardedAd(System.Action OnSuccess, System.Action OnFail)
    {
        if (AdvertisementManager.IsRewardedAdReady())
        {
            bool isAdDone = false;
            void ContinueAfterAd() { isAdDone = true; gameManager.ResumeGame(); Debug.Log("ContinueAfterAd"); }
            bool isSuccess = false;
            AdvertisementManager.ShowRewardedAd(OnStartAdEvent: gameManager.PauseGame, OnFinishAdEvent: ContinueAfterAd, OnFailedAdEvent: ContinueAfterAd, OnFinishRewardedVideoWithSuccessEvent: () => { isSuccess = true; OnSuccess(); });
            yield return new WaitUntil(() => isAdDone);
            if (!isSuccess) OnFail();
        }
        else
        {
            Debug.Log("Not Ready");
            OnFail();
        }
    }
    public void ShowBannerAd()
    {
        return;
        AdvertisementManager.ShowBanner();
    }
    public void HideBannerAd()
    {
        return;
        AdvertisementManager.HideBanner();
    }
}
