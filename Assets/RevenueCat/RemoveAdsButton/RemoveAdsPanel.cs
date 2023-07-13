using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAdsPanel : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI priceText;
    [SerializeField] UIElement uiElement;
    public void UpdatePrice()
    {
        priceText.text = RevenueCatManager.Instance.RemoveAdsPackage.StoreProduct.PriceString;
    }
    public void BeginRemoveAdsPurchase()
    {
        RevenueCatManager.Instance.BeginPurchase(RevenueCatManager.Instance.RemoveAdsPackage);
    }
    public void CheckEntitlement()
    {
        if (RevenueCatManager.Instance.IsRemoveAdsPurchased())
        {
            uiElement.Hide();
        }
    }
}
