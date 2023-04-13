using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using DG.Tweening;

public class Turret : Shooter
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform head;
    [SerializeField] private TurretDamageUpgradeEntity damageUpgrade;
    [SerializeField] private float damageIncreaseRate = 1.2f;

    protected override bool FacedTarget
    {
        get
        {
            if (!target) return false;
            // Get projected forward of shooter
            Vector3 projectedForward = head.forward;
            // Set y to 0 to project it to the ground
            projectedForward.y = 0;
            // Get projected difference between target and shooter
            Vector3 projectedDifference = target.transform.position - head.position;
            // Set y to 0 to project it to the ground
            projectedDifference.y = 0;
            // Angle threshold for accepting the shooter is faced to the target
            float angleThreshold = 20;
            // Return whether the shooter is accepted as faced to the target
            return Vector3.Angle(projectedForward, projectedDifference) <= angleThreshold;
        }
    }

    private void Start()
    {
        UpdateGunDamage();
    }

    public void UpdateGunDamage()
    {
        gun.BaseDamage = Mathf.CeilToInt(dps * Cooldown / dpsDivider * Mathf.Pow(damageIncreaseRate, damageUpgrade.Level));
    }

    public override float GetDPS()
    {
        return dps * Mathf.Pow(damageIncreaseRate, damageUpgrade.Level);
    }

    public void RegisterSelfToTowerDPS()
    {
        towerDPS.Register(this);
    }
    public void UnregisterSelfToTowerDPS()
    {
        towerDPS.Unregister(this);
    }


    public override void StartShooting()
    {
        base.StartShooting();
        DOTween.Kill(transform);
        DOTween.To(() => animator.GetFloat("Speed"), (x) => animator.SetFloat("Speed", x), 20, 0.1f);
    }

    public override void StopShooting()
    {
        base.StopShooting();
        DOTween.Kill(transform);
        DOTween.To(() => animator.GetFloat("Speed"), (x) => animator.SetFloat("Speed", x), 1, 0.1f);
    }

    public override void Face(Vector3 to)
    {
        // Called in Update()
        Vector3 direction = to - head.position;
        head.rotation = Quaternion.Lerp(head.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 3f * shootFrequencyMultiplier);
    }
    public virtual void Die()
    {
        if (Targeting)
            StopTargeting();
        if (target)
            RemoveTarget();
    }
}
