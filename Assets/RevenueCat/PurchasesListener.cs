using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Purchases))]
public class PurchasesListener : Purchases.UpdatedCustomerInfoListener
{
    Purchases purchases;
    private void Awake()
    {
        purchases = GetComponent<Purchases>();
    }
    public override void CustomerInfoReceived(Purchases.CustomerInfo customerInfo)
    {
        // display new CustomerInfo
    }

    private void Start()
    {
        purchases.GetOfferings((offerings, error) =>
        {
            if (error != null)
            {
                Debug.LogError(error.Message);
            }
            else
            {
                Debug.Log(offerings.All.Count);
                BeginPurchase(offerings.All["test"].AvailablePackages[0]);
                // show offering
            }
        });
    }

    public void BeginPurchase(Purchases.Package package)
    {
        purchases.PurchasePackage(package, (productIdentifier, customerInfo, userCancelled, error) =>
        {
            if (!userCancelled)
            {
                if (error != null)
                {
                    // show error
                }
                else
                {
                    // show updated Customer Info
                }
            }
            else
            {
                // user cancelled, don't show an error
            }
        });
    }

    void RestoreClicked()
    {
        purchases.RestorePurchases((customerInfo, error) =>
        {
            if (error != null)
            {
                // show error
            }
            else
            {
                // show updated Customer Info
            }
        });
    }
}