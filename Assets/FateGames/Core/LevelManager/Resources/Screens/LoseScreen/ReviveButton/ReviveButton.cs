using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReviveButton : UIElement
{
    [SerializeField] private AdManager adManager;
    [SerializeField] private Button button;
    [SerializeField] private GameObject adIcon, loading;
    [SerializeField] private UnityEvent onRewardGranted, onRewardFailed;

    private void Start()
    {
        Prepare();
    }
    public void OnClick()
    {
        void Success()
        {
            onRewardGranted.Invoke();
        }
        void Fail()
        {
            onRewardFailed.Invoke();
        }

        StartCoroutine(adManager.ShowRewardedAd(Success, Fail));
    }

    public void Prepare()
    {
        IEnumerator routine()
        {
            button.interactable = false;
            adIcon.SetActive(false);
            loading.SetActive(true);
            //yield return new WaitUntil(AdvertisementManager.IsRewardedAdReady);
            button.interactable = true;
            adIcon.SetActive(true);
            loading.SetActive(false);
            yield break;
        }
        StartCoroutine(routine());
    }
}
