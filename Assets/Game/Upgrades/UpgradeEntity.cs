using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FateGames.Core;
using UnityEngine.Events;

public abstract class UpgradeEntity : ScriptableObject
{
    [SerializeField] protected SaveDataVariable saveData;
    [SerializeField] private UpgradeListEntityRuntimeSet runtimeSet;
    [SerializeField] private Sprite icon;
    [SerializeField] private string upgradeName, description;
    public UnityEvent OnUpgrade = new();
    private UpgradeListEntityRuntimeSet previousRuntimeSet = null;
    protected abstract int Level { get; set; }
    [SerializeField] protected int Limit = -1;
    [SerializeField] protected DayLimit[] dayLimits;
    public bool MaxedOut
    {
        get => Limit >= 0 && Level >= Limit;
    }
    public bool Locked
    {
        get
        {
            int Day = 1;
            bool result = false;
            for (int i = 0; i < dayLimits.Length; i++)
            {
                DayLimit dayLimit = dayLimits[i];
                if (Day <= dayLimit.Day)
                {
                    result = Level >= dayLimit.Limit;
                    break;
                }
            }
            return result;
        }
    }
    [System.Serializable]
    public class DayLimit
    {
        public int Day;
        public int Limit;
    }
    public Sprite Icon { get => icon; }
    public string UpgradeName { get => upgradeName; }
    public string Description { get => description; }
    public int Cost { get => Level * 10; }
    public UpgradeListEntityRuntimeSet RuntimeSet { get => runtimeSet; }

    public void Awake()
    {
        AddToRuntimeSet();
    }
    public void OnDestroy()
    {
        RemoveFromRuntimeSet();
    }
    public void BuyUpgrade()
    {
        if (!saveData.CanAffordTools(Cost)) return;
        saveData.SpendTools(Cost);
        Upgrade();
    }
    public virtual void Upgrade()
    {
        Level++;
        OnUpgrade.Invoke();
    }

    public virtual void Initialize() { }

    public void AddToRuntimeSet()
    {
        RemoveFromPreviousRuntimeSet();
        if (runtimeSet)
        {
            runtimeSet.Add(this);
            previousRuntimeSet = runtimeSet;
        }
    }
    public void RemoveFromPreviousRuntimeSet()
    {
        if (previousRuntimeSet)
            previousRuntimeSet.Remove(this);
    }
    public void RemoveFromRuntimeSet()
    {
        if (runtimeSet)
            runtimeSet.Remove(this);
    }

    #region Reliable In-Editor OnDestroy

    // Sadly OnDestroy is not being called reliably by the editor. So we need this.
    // Thanks to: https://forum.unity.com/threads/ondestroy-and-ondisable-are-not-called-when-deleting-a-scriptableobject-file.1129220/#post-7259671
    class OnDestroyProcessor : UnityEditor.AssetModificationProcessor
    {
        // Cache the type for reuse.
        static System.Type _type = typeof(UpgradeEntity);

        // Limit to certain file endings only.
        static string _fileEnding = ".asset";

        public static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions _)
        {
            if (!path.EndsWith(_fileEnding))
                return AssetDeleteResult.DidNotDelete;

            var assetType = AssetDatabase.GetMainAssetTypeAtPath(path);
            if (assetType != null && (assetType == _type || assetType.IsSubclassOf(_type)))
            {
                var asset = AssetDatabase.LoadAssetAtPath<UpgradeEntity>(path);
                asset.OnDestroy();
            }

            return AssetDeleteResult.DidNotDelete;
        }
    }

    #endregion

}

#if UNITY_EDITOR
[CustomEditor(typeof(UpgradeEntity), true)]
public class UpgradeListEntityEditor : Editor
{
    private UpgradeEntity upgradeListEntity;
    private void OnEnable()
    {
        upgradeListEntity = target as UpgradeEntity;
    }
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();
        if (EditorGUI.EndChangeCheck())
        {
            upgradeListEntity.OnDestroy();
            upgradeListEntity.Awake();
        }
    }
}
#endif