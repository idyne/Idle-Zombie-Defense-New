using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using FateGames.Core;

public class ZoneMapController : UIElement
{
    [SerializeField] private ZoneManager zoneManager;
    [SerializeField] private Route[] routes = null;
    [SerializeField] private Zone[] zones = null;
    [SerializeField] private Transform map = null;
    [SerializeField] private GameObject nextButton = null;
    [SerializeField] private GameEvent onClosedEvent = null;

    public void OpenMapForZonePass()
    {
        StartCoroutine(ZonePass());
    }

    private IEnumerator ZonePass()
    {
        int passedZoneIndex = zoneManager.Zone-1;

        OpenAndClean();
        SetupZonesAndPaths(passedZoneIndex);
        yield return new WaitForSeconds(1f);

        float focusDuration = 2f;
        FocusZone(passedZoneIndex, focusDuration);
        yield return new WaitForSeconds(focusDuration + 0.5f);

        float animatePassDuration = 2f;
        FindNextPathOnWorld(passedZoneIndex, animatePassDuration);
        PlayPath(passedZoneIndex, animatePassDuration);
        yield return new WaitForSeconds(animatePassDuration + 1);

        nextButton.SetActive(true);
    }

    private void OpenAndClean()
    {
        map.localScale = Vector3.one * 0.25f;
        map.localPosition = Vector3.zero;
        nextButton.SetActive(false);
        Show();
    }

    private void SetupZonesAndPaths(int lastAchivedZoneIndex)
    {
        for (int i = 0; i < lastAchivedZoneIndex; i++)
        {
            routes[i].InstantFill();
            zones[i].InstantExit();
        }
    }

    private void PlayPath(int fromIndex, float duration)
    {
        zones[fromIndex].ExitAnimation();

        if (fromIndex != zones.Length - 1)
        {
            routes[fromIndex].AnimateFill(duration);
            DOVirtual.DelayedCall(duration, () => zones[fromIndex + 1].EnterAnimation());
        }
    }

    private void FindNextPathOnWorld(int lastAchivedZoneIndex, float duration)
    {
        if (lastAchivedZoneIndex < zones.Length - 1)
        {
            Vector3 target = (zones[lastAchivedZoneIndex].transform.localPosition + zones[lastAchivedZoneIndex + 1].transform.localPosition) / 2;
            map.DOKill();
            map.DOLocalMove(-target, duration);
            map.DOScale(Vector3.one, duration);
        }
    }

    private void FocusZone(int zoneIndex, float duration)
    {
        if (zoneIndex < zones.Length)
        {
            map.DOKill();
            map.DOLocalMove(-zones[zoneIndex].transform.localPosition, duration).SetEase(Ease.InOutQuad);
            map.DOScale(Vector3.one, duration).SetEase(Ease.InOutQuad);
        }
    }

    private void Close()
    {
        Hide();
        onClosedEvent.Raise();
    }

}
