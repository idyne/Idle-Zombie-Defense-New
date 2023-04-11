using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
public class UIListView : FateMonoBehaviour
{
    [SerializeField] private float padding = 20;
    private List<RectTransform> items = new();
    //private float pointer = 0;

    private void Start()
    {
        RearrangeItems();
    }
    public void GoToUp()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 position = rectTransform.anchoredPosition;
        Debug.Log(position, this);
        position.y = 0;
        rectTransform.anchoredPosition = position;
        Debug.Log(rectTransform.anchoredPosition, this);
    }
    public void AddItem(RectTransform rectTransform)
    {
        rectTransform.SetParent(transform);
        rectTransform.SetAsLastSibling();
        items.Add(rectTransform);
        RearrangeItems();
    }

    public T AddItem<T>(GameObject prefab) where T : Component
    {
        T item = Instantiate(prefab, transform).GetComponent<T>();
        items.Add(item.GetComponent<RectTransform>());
        RearrangeItems();
        return item;
    }

    public void RemoveItem(RectTransform rectTransform)
    {
        if (items.Remove(rectTransform))
        {
            Destroy(rectTransform.gameObject);
            RearrangeItems();
        }
    }

    public void RemoveItem(GameObject gameObject)
    {
        if (items.Remove(gameObject.GetComponent<RectTransform>()))
        {
            Destroy(gameObject);
            RearrangeItems();
        }
    }

    public void Clear()
    {
        while (items.Count > 0)
        {
            RemoveItem(items[^1]);
        }
    }

    public void RearrangeItems()
    {
        float current = 0;
        for (int i = 0; i < items.Count; i++)
        {
            RectTransform item = items[i];
            Vector3 anchoredPosition = item.anchoredPosition;
            anchoredPosition.y = -current;
            item.anchoredPosition = anchoredPosition;
            current += item.sizeDelta.y + padding;
        }
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 sizeDelta = rectTransform.sizeDelta;
        sizeDelta.y = current;
        rectTransform.sizeDelta = sizeDelta;
    }


}
