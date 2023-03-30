using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FateGames.Core;

public class SkillButton : UIElement
{
    [SerializeField] private Button button;
    [SerializeField] private FloatVariable remainingCooldownPercent;
    [SerializeField] private Image cooldownLayer;
    [SerializeField] private SaveDataVariable saveData;

    private void Start()
    {
        Hide();
    }

    private void OnEnable()
    {
        remainingCooldownPercent.OnValueChanged.AddListener(UpdateCooldownLayer);
    }
    private void OnDisable()
    {
        remainingCooldownPercent.OnValueChanged.RemoveListener(UpdateCooldownLayer);
    }

    public override void Show()
    {
        if (saveData.Value.MolotovUnlocked)
            base.Show();
    }

    private void UpdateCooldownLayer(float previousRemainingCooldownPercent, float currentRemainingCooldownPercent)
    {
        cooldownLayer.fillAmount = currentRemainingCooldownPercent;
        button.interactable = currentRemainingCooldownPercent <= 0;
    }

}
