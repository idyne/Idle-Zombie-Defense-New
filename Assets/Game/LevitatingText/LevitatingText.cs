using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using System;
using TMPro;
using DG.Tweening;
public class LevitatingText : UIElement, IPooledObject
{
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI text;
    public event Action OnRelease;

    public void OnObjectSpawn()
    {
        Activate();
        Levitate();
    }

    public void Release()
    {
        Deactivate();
        OnRelease.Invoke();
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
    public void Levitate()
    {
        animator.SetTrigger("Levitate");
    }
}
