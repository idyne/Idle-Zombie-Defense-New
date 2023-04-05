using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Zone : MonoBehaviour
{
    [SerializeField] private Transform root = null;
    [SerializeField] private Transform cleanSign = null;
    [SerializeField] private Transform cleaningWave = null;
    [SerializeField] private Image enemyDetails = null;

    public void EnterAnimation()
    {
        Hop();
    }

    public void ExitAnimation()
    {
        Hop();
        Wave();
        CleanEnemyDetails();
        DOVirtual.DelayedCall(1f, () => { ApplyClearedMark(); });
    }

    public void InstantExit()
    {
        InstantApplyClearedMark();
        InstantCleanEnemyDetails();
    }

    private void Hop()
    {
        root.DOKill(true);
        root.DOScale(Vector3.one * 1.5f, 0.2f).SetLoops(2, LoopType.Yoyo);
    }

    private void Wave()
    {
        cleaningWave.DOScale(Vector3.one * 5, 1f);
        cleaningWave.GetComponent<Image>().DOFade(0, 1f);
    }

    private void ApplyClearedMark()
    {
        cleanSign.DOScale(Vector3.one, 0.5f);
        cleanSign.GetComponent<Image>().DOFade(1, 0.5f);
        cleanSign.GetChild(0).GetComponent<Image>().DOFade(1, 0.5f);
    }

    private void InstantApplyClearedMark()
    {
        cleanSign.localScale = Vector3.one;
        Color tempColor = cleanSign.GetComponent<Image>().color;
        tempColor.a = 1;
        cleanSign.GetComponent<Image>().color = tempColor;
        cleanSign.GetChild(0).GetComponent<Image>().color = tempColor;
    }

    private void CleanEnemyDetails()
    {
        enemyDetails.DOFade(0, 0.5f);
    }

    private void InstantCleanEnemyDetails()
    {
        Color tempColor = enemyDetails.color;
        tempColor.a = 0;
        enemyDetails.color = tempColor;
    }
}
