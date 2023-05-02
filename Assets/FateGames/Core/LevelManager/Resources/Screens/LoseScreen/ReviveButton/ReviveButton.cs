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
        SDKManager.Instance.ShowRewardedAd(Fail, Success);
    }

    public void Prepare()
    {
        IEnumerator routine()
        {
            button.interactable = false;
            adIcon.SetActive(false);
            loading.SetActive(true);
            yield return new WaitUntil(() => SDKManager.Instance.IsRewardedAdReady());
            button.interactable = true;
            adIcon.SetActive(true);
            loading.SetActive(false);
        }
        StartCoroutine(routine());
    }
}
