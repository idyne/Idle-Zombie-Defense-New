using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using FateGames.Tweening;
using UnityEngine.Events;

public class TimeLapseHeader : MonoBehaviour
{
    [SerializeField] private FloatReference transitionDuration;
    [SerializeField] private List<GameObject> timeHeaders = new List<GameObject>();
    [SerializeField] private UnityEvent onTimeLapseHeaderAnimationStarted, onTimeLapseHeaderAnimationCompleted;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void Animate(int targetIndex)
    {
        onTimeLapseHeaderAnimationStarted.Invoke();
        timeHeaders[targetIndex].SetActive(true);
        animator.SetTrigger("HeaderUp");
        FaTween.DelayedCall(transitionDuration.Value, () => timeHeaders[targetIndex].SetActive(false)).OnComplete(onTimeLapseHeaderAnimationCompleted.Invoke);
    }
}
