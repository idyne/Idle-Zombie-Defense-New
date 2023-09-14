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

    public void OnSDKsInitialized()
    {
        Configure();
        CollectData();
        GetOfferings();
    }

    public void RestorePurchases()
    {
        purchases.RestorePurchases((info, error) =>
        {
            SetCustomerInfo(info);
        });
    }

    public void CollectData()
    {
        purchases.CollectDeviceIdentifiers();
        string adid = Adjust.getAdid();
        Debug.Log($"adjust adid: {adid}");
        purchases.SetAdjustID(adid);

    }

    public void Configure()
    {
        purchases.SetLogLevel(LogLevel.Debug);
        Purchases.PurchasesConfiguration.Builder builder = Purchases.PurchasesConfiguration.Builder.Init("appl_vuOKWotExaxxXzSEfVWSmCUvkkl");
        Purchases.PurchasesConfiguration purchasesConfiguration =
            builder
                .Build();
        purchases.Configure(purchasesConfiguration);
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

                RemoveAdsPackage = offerings.Current.AvailablePackages[0];
                OnRemoveAdsPackageLoaded.Invoke();
            }
        });
    }

    public override void CustomerInfoReceived(Purchases.CustomerInfo customerInfo)
    {
        SetCustomerInfo(customerInfo);
    }

    public bool IsRemoveAdsPurchased()
    {
        if (CustomerInfo == null || RemoveAdsPackage == null) return false;
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
                    purchases.SyncPurchases();
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
