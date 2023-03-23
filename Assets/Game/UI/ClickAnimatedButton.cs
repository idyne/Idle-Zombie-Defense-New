using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickAnimatedButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Button button;
    [SerializeField] private GameObject container, pressedContainer;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.interactable) return;
        container.SetActive(false);
        pressedContainer.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        container.SetActive(true);
        pressedContainer.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            button.interactable = !button.interactable;
    }

}
