using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
public class UIElement : FateMonoBehaviour
{
    [SerializeField] private Canvas canvas;

    public virtual void Hide() => canvas.enabled = false;
    public virtual void Show() => canvas.enabled = true;
}
