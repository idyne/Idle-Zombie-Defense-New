using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.Events;

public class FireRateBoostPanel : MonoBehaviour
{
    [SerializeField] private UIElement uiElement;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Animator animator;
    [SerializeField] private UnityEvent onHidden, onShowed;
    public static bool isShown = false;
    public void Show()
    {
        if (!SDKManager.Instance.IsGraceTimePassed || isShown) return;
        gameManager.PauseGame();
        isShown = true;
        uiElement.Show();
        animator.SetTrigger("Show");
        onShowed.Invoke();
    }
    public void Hide()
    {
        gameManager.ResumeGame();
        uiElement.Hide();
        onHidden.Invoke();
    }
}
