using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.UI;

public class UIElement : FateMonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private GameObject backgroundObject = null;
    private int originalOrder = 0;

    public virtual void Hide() { canvas.enabled = false; canvas.gameObject.SetActive(false); }
    public virtual void Show() { canvas.enabled = true; canvas.gameObject.SetActive(true); }

    public void Highlight()
    {
        originalOrder = canvas.renderOrder;
        if (backgroundObject == null)
            backgroundObject = CreateHighlightBackground();
        backgroundObject.SetActive(true);
        OnHighlight();
    }
    public void Dehightlight()
    {
        if (backgroundObject)
            backgroundObject.SetActive(false);
        OnDehighlight();
    }
    public virtual void OnHighlight()
    {

    }
    public virtual void OnDehighlight()
    {

    }
    private GameObject CreateHighlightBackground()
    {
        GameObject backgroundObject = new("HighlightBackground", typeof(RectTransform));
        RectTransform backgroundTransform = backgroundObject.GetComponent<RectTransform>();
        backgroundTransform.SetParent(canvas.transform);
        backgroundTransform.SetAsFirstSibling();
        backgroundTransform.Stretch();
        Image backgroundImage = backgroundObject.AddComponent<Image>();
        backgroundImage.color = new Color(0, 0, 0, 0.7f);
        return backgroundObject;
    }
}
