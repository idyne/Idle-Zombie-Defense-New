using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonOnClickOverrider : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Button.ButtonClickedEvent onClick;
    public virtual void OnEnable()
    {
        button.onClick = onClick;
        //onClick.AddListener(() => { Debug.Log(name + " clicked!", this); });
    }
    public virtual void OnDisable()
    {

    }
}
