using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.UI;

public class UIElement : FateMonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private List<UIElement> children = new();
    private Dictionary<UIElement, bool> childrenStatesBeforeHide = new();
    private UIElement parentUIElement = null;
    [SerializeField] private bool hideOnStart = false;
    public bool Hidden => !canvas.enabled;

    protected virtual void Awake()
    {
        BindParentUIElement();
        if (!canvas.gameObject.activeSelf)
            Debug.LogError("Not Active", this);
    }

    private void Start()
    {
        if (hideOnStart) Hide();
    }

    private void BindParentUIElement()
    {
        Transform parent = transform.parent;
        while (parent != null)
        {
            parentUIElement = parent.GetComponent<UIElement>();
            if (parentUIElement != null)
                break;
            parent = parent.parent;
        }
        if (parentUIElement != null)
            parentUIElement.AddChild(this);
    }
    public void AddChild(UIElement child)
    {
        children.Add(child);
    }
    public void RemoveChild(UIElement child)
    {
        children.Remove(child);
        childrenStatesBeforeHide.Remove(child);
    }

    public Canvas Canvas { get => canvas; }

    public virtual void Hide()
    {
        if (Hidden) return;
        //Debug.Log("Hide " + name, this);
        canvas.enabled = false;
        childrenStatesBeforeHide.Clear();
        foreach (UIElement child in children)
        {
            childrenStatesBeforeHide.Add(child, !child.Hidden);
            child.Hide();
        }
    }
    public virtual void Show()
    {
        if (!Hidden) return;
        //Debug.Log("Show " + name, this);
        if (parentUIElement && parentUIElement.Hidden) return;
        canvas.enabled = true;
        foreach (UIElement child in childrenStatesBeforeHide.Keys)
        {
            if (childrenStatesBeforeHide[child]) child.Show();
        }
        childrenStatesBeforeHide.Clear();
    }

    private void OnDestroy()
    {
        if (parentUIElement)
            parentUIElement.RemoveChild(this);
        parentUIElement = null;
    }
}
