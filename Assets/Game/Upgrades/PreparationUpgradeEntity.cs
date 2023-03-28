using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class PreparationUpgradeEntity : UpgradeEntity
{
    [SerializeField] private UpgradeListEntityRuntimeSet runtimeSet;
    [SerializeField] private Sprite icon;
    [SerializeField] private string description;
    private UpgradeListEntityRuntimeSet previousRuntimeSet = null;
    public Sprite Icon { get => icon; }

    public string Description { get => description; }
    public UpgradeListEntityRuntimeSet RuntimeSet { get => runtimeSet; }
    public override bool Affordable => saveData.CanAffordTools(Cost);
    public virtual void Initialize() { }

    public override void BuyUpgrade()
    {
        if (!Affordable) return;
        saveData.SpendTools(Cost);
        Upgrade();
    }

    public void Awake()
    {
        AddToRuntimeSet();
    }
    public void OnDestroy()
    {
        RemoveFromRuntimeSet();
    }

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
        static System.Type _type = typeof(PreparationUpgradeEntity);

        // Limit to certain file endings only.
        static string _fileEnding = ".asset";

        public static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions _)
        {
            if (!path.EndsWith(_fileEnding))
                return AssetDeleteResult.DidNotDelete;

            var assetType = AssetDatabase.GetMainAssetTypeAtPath(path);
            if (assetType != null && (assetType == _type || assetType.IsSubclassOf(_type)))
            {
                var asset = AssetDatabase.LoadAssetAtPath<PreparationUpgradeEntity>(path);
                asset.OnDestroy();
            }

            return AssetDeleteResult.DidNotDelete;
        }
    }

    #endregion

}
#if UNITY_EDITOR
[CustomEditor(typeof(PreparationUpgradeEntity), true)]
public class UpgradeListEntityEditor : Editor
{
    private PreparationUpgradeEntity upgradeListEntity;
    private void OnEnable()
    {
        upgradeListEntity = target as PreparationUpgradeEntity;
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