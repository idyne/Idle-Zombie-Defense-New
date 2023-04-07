using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHighlighter : UIElement
{
    static public TutorialHighlighter Instance;
    [SerializeField] private RectTransform darkLayer = null;

    private Transform lastHighlightedObject = null;
    private Transform lastHighilghtedObjectParent = null;

    private void Awake()
    {
        Instance = this;
    }

    public bool Highlight(Transform UIObject)
    {
        if (lastHighlightedObject != null)
        {
            Debug.Log("There is already an highlighted object. But removed");
            Dehighlight(lastHighlightedObject);
        }

        Show();
        lastHighilghtedObjectParent = UIObject.parent;
        lastHighlightedObject = UIObject;
        UIObject.SetParent(darkLayer);
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
