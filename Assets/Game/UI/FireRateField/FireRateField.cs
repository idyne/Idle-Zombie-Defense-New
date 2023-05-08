using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
public class FireRateField : MonoBehaviour
{
    [SerializeField] private FireRateUpgradeEntity fireRateUpgrade;
    [SerializeField] private TMPro.TextMeshProUGUI fireRateText;
    [SerializeField] protected FloatVariable shootFrequencyMultiplier, boostMultiplier;

    private void OnEnable()
    {
        shootFrequencyMultiplier.OnValueChanged.AddListener(OnShootFrequencyMultiplierChanged);
        boostMultiplier.OnValueChanged.AddListener(OnShootFrequencyMultiplierChanged);
    }
    private void OnDisable()
    {
        shootFrequencyMultiplier.OnValueChanged.RemoveListener(OnShootFrequencyMultiplierChanged);
        boostMultiplier.OnValueChanged.RemoveListener(OnShootFrequencyMultiplierChanged);
    }
    private void Start()
    {
        UpdateField();
    }
    private void OnShootFrequencyMultiplierChanged(float previous, float current)
    {
        UpdateField();
    }
    public void UpdateField()
    {
        float fireRate = ((float)fireRateUpgrade.Level / fireRateUpgrade.Limit * 3 + 1) * shootFrequencyMultiplier.Value * boostMultiplier.Value;
        fireRateText.text = "x" + string.Format("{0:0.00}", fireRate);
    }
}
