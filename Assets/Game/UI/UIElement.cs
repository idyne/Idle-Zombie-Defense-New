using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.UI;

public class UIElement : FateMonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private bool deactivateOnHide = true;

    public Canvas Canvas { get => canvas; }

    public virtual void Hide() { canvas.enabled = false; if (deactivateOnHide) canvas.gameObject.SetActive(false); }
    public virtual void Show() { canvas.enabled = true; canvas.gameObject.SetActive(true); }
}
