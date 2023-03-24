using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
public class UIListView : FateMonoBehaviour
{
    [SerializeField] private float padding = 20;
    private void Start()
    {
        RearrangeItems();
    }
    public void AddItem(RectTransform rectTransform)
    {
        rectTransform.SetParent(transform);
        rectTransform.SetAsLastSibling();
        RearrangeItems();
    }

    public void RearrangeItems()
    {
        float current = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
            Vector3 anchoredPosition = child.anchoredPosition;
            anchoredPosition.y = -current;
            child.anchoredPosition = anchoredPosition;
            current += child.sizeDelta.y + padding;
        }
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = current;
        rectTransform.sizeDelta = sizeDelta;
    }
}
