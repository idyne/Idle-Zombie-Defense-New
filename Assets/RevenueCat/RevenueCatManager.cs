using com.adjust.sdk;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static Purchases;

[RequireComponent(typeof(Purchases))]
public class RevenueCatManager : Purchases.UpdatedCustomerInfoListener
{
    private static RevenueCatManager instance = null;
    public static RevenueCatManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<RevenueCatManager>();
            return instance;
        }
    }
    Purchases purchases;
    public Package RemoveAdsPackage { get; private set; }
    public CustomerInfo CustomerInfo { get; private set; }
    [SerializeField] private UnityEvent OnCustomerInfoUpdated = new();
    [SerializeField] private UnityEvent OnRemoveAdsPackageLoaded = new();


    private void Awake()
    {

        DontDestroyOnLoad(gameObject);

        purchases = GetComponent<Purchases>();



    }
    public void CollectData()
    {
        try
        {
            purchases.CollectDeviceIdentifiers();

        }
        catch (System.Exception)
        {

            Debug.LogError("CollectDeviceIdentifiers error");
        }
        try
        {
            string adid = Adjust.getAdid();
            Debug.Log(adid);
            purchases.SetAdjustID(adid);

        }
        catch (System.Exception)
        {

            Debug.LogError("SetAdjustID error");
        }
    }
    public void GetOfferings()
    {
        purchases.GetOfferings((offerings, error) =>
        {
            if (error != null)
            {
                Debug.LogError(error.Message);
            }
            else
            {
                Debug.Log("test2");
                Debug.Log(offerings.Current.AvailablePackages[0]);
                RemoveAdsPackage = offerings.Current.AvailablePackages[0];
                OnRemoveAdsPackageLoaded.Invoke();
                Debug.Log(RemoveAdsPackage.StoreProduct.PriceString);
                // show offering
            }
        });
    }

    public override void CustomerInfoReceived(Purchases.CustomerInfo customerInfo)
    {
        SetCustomerInfo(customerInfo);
    }

    public bool IsRemoveAdsPurchased()
    {
        if (CustomerInfo == null) return false;
        return CustomerInfo.Entitlements.Active.ContainsKey("remove_ads");
    }

    public void BeginPurchase(Purchases.Package package)
    {
        Debug.Log($"Begin purchasing package: {package}");
        purchases.PurchasePackage(package, (productIdentifier, customerInfo, userCancelled, error) =>
        {
            if (!userCancelled)
            {
                if (error != null)
                {
                    Debug.LogError(error.Message);
                }
                else
                {
                    Debug.Log(customerInfo);
                    SetCustomerInfo(customerInfo);
                }
            }
            else
            {
                Debug.Log("User cancelled the purchase");
            }
        });
    }

    private void SetCustomerInfo(CustomerInfo customerInfo)
    {
        CustomerInfo = customerInfo;
        OnCustomerInfoUpdated.Invoke();
    }

    public void RestoreClicked()
    {
        purchases.RestorePurchases((customerInfo, error) =>
        {
            if (error != null)
            {
                Debug.LogError($"Restore purchases error: {error.Message}");
            }
            else
            {
                SetCustomerInfo(customerInfo);
            }
        });
    }

}
