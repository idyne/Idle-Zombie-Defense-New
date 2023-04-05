using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FateGames.Core;
public class DOTweenSequenceTest : FateMonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private FloatVariable targetVariable;
    [SerializeField] private float impactDuration = 1.5f;
    Tween delayedCall, speedUpTween = null;
    void Start()
    {
       


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            delayedCall.Kill(true);
        if (Input.GetKeyDown(KeyCode.S))
            CancelSpeedUp();
        if (Input.GetKeyDown(KeyCode.L))
        {
            SpeedUp();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            delayedCall = transform.DOMoveX(10, 1);
        }
    }
    public void SpeedUp()
    {
        CancelSpeedUp();
        targetVariable.Value = 2;
        speedUpTween = transform.DOMoveX(10, 1);
    }
    public void CancelSpeedUp()
    {
        if (speedUpTween == null) return;
        print("Kill");
        speedUpTween.Kill(true);
        speedUpTween = null;
    }
}
