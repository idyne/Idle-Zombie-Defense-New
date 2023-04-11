using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FateGames.Core;
public class HealthBar : UIElement
{
    private Slider slider;
    private Tween tween = null;
    private Tween showTween = null;
    private static Camera mainCamera;
    private static Transform container;
    [SerializeField] private RectTransform sliderContainer;
    [SerializeField] private HealthBarSet set;
    public float Percent { get => slider.value; }
    protected override void Awake()
    {
        base.Awake();
        slider = GetComponentInChildren<Slider>();
        slider.minValue = 0;
        if (mainCamera == null) mainCamera = Camera.main;
        if (container == null) container = new GameObject("Health Bar Container").transform;
    }

    private void OnEnable()
    {
        set.Add(this);
    }

    private void OnDisable()
    {
        set.Remove(this);
    }

    private void Start()
    {
        Hide();
        //transform.SetParent(container);
        /*transform.position = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;*/
    }

    public void SetPercent(float percent)
    {
        CancelTween();
        float time = Mathf.Abs(slider.value - percent) / 2f; // 0 to 1 in 2 seconds;
        tween = DOTween.To(() => slider.value, (x) => slider.value = x, percent, time).OnComplete(() => { tween = null; });
    }

    // Call on every frame
    public void SnapToTarget()
    {
        //sliderContainer.position = mainCamera.WorldToScreenPoint(target.position);
    }

    private void CancelTween()
    {
        if (tween == null) return;
        tween.Kill();
        tween = null;
    }

    public override void Hide()
    {
        tween?.Complete();
        CancelShowTween();
        base.Hide();
        Deactivate();
    }

    public void Show(float duration = -1)
    {
        tween?.Complete();
        CancelShowTween();
        base.Show();
        if (duration > 0)
            showTween = DOVirtual.DelayedCall(duration, Hide, false);
        Activate();
    }

    private void CancelShowTween()
    {
        if (showTween == null) return;
        showTween.Kill();
        showTween = null;
    }
}
