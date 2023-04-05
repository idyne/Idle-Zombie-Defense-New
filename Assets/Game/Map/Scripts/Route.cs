using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    [SerializeField] private bool clockwise = true;
    [SerializeField] private Transform filler = null;

    int direction = -1;

    private void Awake()
    {
        if (clockwise) direction = 1;
        Clear();
    }

    public void AnimateFill(float duration)
    {
        Clear();
        filler.DOLocalRotate(Vector3.zero, duration);
    }

    public void InstantFill()
    {
        filler.localEulerAngles = Vector3.zero;
    }

    private void Clear()
    {
        filler.localEulerAngles = Vector3.forward * 90 * direction;
    }
}
