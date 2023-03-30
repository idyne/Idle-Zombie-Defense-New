using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Commander : Soldier
{
    [SerializeField] protected ObjectPool throwableWeaponPool;
    [SerializeField] protected FloatVariable remainingCooldownPercent;
    [SerializeField] protected Transform throwableContainer;
    [SerializeField] protected SaveDataVariable saveData;
    [SerializeField] protected SkillButton skillButton;
    private Tween cooldownTween = null;
    private bool usingSkill = false;
    private Vector3 lastKnownTargetPosition = Vector3.zero;

    public float SkillCooldown => 10f / saveData.Value.MolotovCooldownLevel;

    public void UseSkill()
    {
        Log("UseSkill", false);
        if (target)
            RemoveTarget();
        else if (Targeting)
            StopTargeting();
        StartCooldown();
        ThrowWeapon();
    }

    public override void FaceTarget()
    {
        if (usingSkill && !target)
            Face(lastKnownTargetPosition);
        else
            base.FaceTarget();
    }
    private void StartCooldown()
    {
        Log("StartCooldown", false);
        remainingCooldownPercent.Value = 1;
        if (cooldownTween != null) cooldownTween.Kill();
        FateGames.Tweening.FaTween.To(() => remainingCooldownPercent.Value, (x) => remainingCooldownPercent.Value = x, 0, SkillCooldown);
    }
    public void ThrowWeapon()
    {
        Log("ThrowWeapon", false);

        IEnumerator throwWeapon()
        {
            usingSkill = true;
            animator.SetTrigger("Throw");
            gun.Deactivate();
            Zombie nearestZombie = FindNearestZombie();
            lastKnownTargetPosition = nearestZombie.transform.position;
            yield return new WaitForSeconds(0.3f);
            Throwable throwableWeapon = throwableWeaponPool.Get<Throwable>(throwableContainer.position, throwableContainer.rotation);
            throwableWeapon.transform.SetParent(throwableContainer);
            void onTargetDied()
            {
                lastKnownTargetPosition = nearestZombie.transform.position;
                RemoveTarget();
                //OnTargetDied();
            }
            SetTarget(nearestZombie, onTargetDied);
            yield return new WaitForSeconds(0.63f);
            Log("Throw", true);
            throwableWeapon.transform.SetParent(null);
            throwableWeapon.Throw(lastKnownTargetPosition);
            yield return new WaitForSeconds(0.28f);
            gun.Activate();
            yield return new WaitForSeconds(0.39f);
            usingSkill = false;
            if (target)
            {
                RemoveTarget();
                StartTargeting();
            }
        }
        StartCoroutine(throwWeapon());
    }

}
