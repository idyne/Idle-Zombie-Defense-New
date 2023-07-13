using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAdsButton : MonoBehaviour
{
    [SerializeField] UIElement uiElement;


    public void CheckEntitlement()
    {
        if (RevenueCatManager.Instance.IsRemoveAdsPurchased())
        {
            Debug.Log("IsRemoveAdsPurchased: true");
            uiElement.Hide();
        }
        else
        {
            Debug.Log("IsRemoveAdsPurchased: false");
            uiElement.Show();
        }
    }
    
}
