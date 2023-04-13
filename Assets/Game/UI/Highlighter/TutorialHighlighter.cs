using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHighlighter : UIElement
{
    static public TutorialHighlighter Instance;
    [SerializeField] private RectTransform darkLayer = null;
    [SerializeField] private RectTransform container = null;
    [SerializeField] private RectTransform centerContainer = null;

    private Transform lastHighlightedObject = null;
    private Transform lastHighilghtedObjectParent = null;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public bool Highlight(RectTransform UIObject, bool anchorCenter = false)
    {
        if (lastHighlightedObject != null)
        {
            Debug.Log("There is already an highlighted object. But removed");
            Dehighlight(lastHighlightedObject);
        }
        Show();
        Vector3 anchoredPosition = UIObject.anchoredPosition;
        Vector3 scale = UIObject.localScale;
        lastHighilghtedObjectParent = UIObject.parent;
        lastHighlightedObject = UIObject;
        UIObject.SetParent(anchorCenter ? centerContainer : container);
        UIObject.localScale = scale;
        UIObject.anchoredPosition = anchoredPosition;
        return true;
    }

    public bool Dehighlight(Transform UIObject)
    {
        if (lastHighlightedObject == null)
        {
            Debug.Log("There is no highlighted object.");
            return false;
        }

        if (lastHighlightedObject != UIObject)
        {
            Debug.Log("This not the highlighted object.");
            return false;
        }

        Hide();
        lastHighlightedObject.SetParent(lastHighilghtedObjectParent);
        lastHighilghtedObjectParent = null;
        lastHighlightedObject = null;
        return true;
    }
}
