using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FateGames.Core;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private FloatReference cooldown;
    [SerializeField] private Image cooldownLayer;
    public void PutInCooldown()
    {
        button.interactable = false;
        cooldownLayer.fillAmount = 1;
        DOTween.To(() => cooldownLayer.fillAmount, (x) => { cooldownLayer.fillAmount = x; }, 0, cooldown.Value).OnComplete(() =>
        {
            button.interactable = true;
        });
    }
}
