using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using DG.Tweening;

public class TapToSpeedUp : UIElement
{

    [SerializeField] private Animator animator;
    [SerializeField] private FloatReference targetVariable;
    [SerializeField] GameEvent onTappedToSpeedUp;
    [SerializeField] private float impactDuration = 1.5f;
    [SerializeField] private float waitingDuration = 4;
    [SerializeField] private float multiplier = 2;

    private bool isGameStarted = false;

    private Tween speedUpTween = null, countdownTween = null;


    private void Start()
    {
        Init();
    }

    public void OnGameStarted()
    {
        Debug.Log("OnGameStarted", this);
        isGameStarted = true;
        StartCountdown();
    }
    public void OnGameEnded()
    {
        isGameStarted = false;
        CancelCountdown();
    }

    private void Init()
    {
        targetVariable.Value = 1;
        void TapCheck() { if (isGameStarted && (!EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject == null)) Tap(); }
        InputManager.GetKeyDownEvent(KeyCode.Mouse0).AddListener(TapCheck);
    }

    public void Tap()
    {
        onTappedToSpeedUp.Raise();
        Hide();
        SpeedUp();
        StartCountdown();
    }
    public void SpeedUp()
    {
        CancelSpeedUp();
        targetVariable.Value = 2;
        speedUpTween = DOTween.To(() => targetVariable.Value, (float x) => targetVariable.Value = x, 1, impactDuration).OnComplete(() => speedUpTween = null);
    }
    public void CancelSpeedUp()
    {
        if (speedUpTween == null) return;
        speedUpTween.Kill(true);
        speedUpTween = null;
    }
    public void StartCountdown()
    {
        CancelCountdown();
        countdownTween = DOVirtual.DelayedCall(waitingDuration, Show).OnComplete(() => countdownTween = null);
    }
    public void CancelCountdown()
    {
        if (countdownTween == null) return;
        countdownTween.Kill(true);
        countdownTween = null;
    }
    public override void Show()
    {
        base.Show();
        Animate();
    }
    public void Animate()
    {
        animator.SetTrigger("Bounce");
    }

}
