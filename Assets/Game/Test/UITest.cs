using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(EventSystem.current.IsPointerOverGameObject());
            Debug.Log(EventSystem.current.currentSelectedGameObject, EventSystem.current.currentSelectedGameObject);
        }
    }
}
