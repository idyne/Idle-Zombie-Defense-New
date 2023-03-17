using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Commander : Soldier
{
    [SerializeField] protected ObjectPool throwableWeaponPool;
    [SerializeField] protected FloatReference cooldown;
    [SerializeField] protected Transform throwableContainer;

    public void UseSkill()
    {
        if (target)
            RemoveTarget();
        else if (Targeting)
            StopTargeting();
        ThrowWeapon();
    }
    public void ThrowWeapon()
    {
        Log("ThrowWeapon", false);

        IEnumerator throwWeapon()
        {
            animator.SetTrigger("Throw");
            gun.Deactivate();
            yield return new WaitForSeconds(0.3f);
            Throwable throwableWeapon = throwableWeaponPool.Get<Throwable>(throwableContainer.position, throwableContainer.rotation);
            throwableWeapon.transform.SetParent(throwableContainer);
            Zombie nearestZombie = FindNearestZombie();
            Vector3 lastKnownPosition = nearestZombie.transform.position;
            void onTargetDied()
            {
                lastKnownPosition = nearestZombie.transform.position;
                OnTargetDied();
            }
            SetTarget(nearestZombie, onTargetDied);
            yield return new WaitForSeconds(0.63f);


            throwableWeapon.transform.SetParent(null);
            throwableWeapon.Throw(lastKnownPosition);
            yield return new WaitForSeconds(0.28f);
            gun.Activate();
            yield return new WaitForSeconds(0.39f);
            if (target)
                RemoveTarget();
            StartTargeting();
        }
        StartCoroutine(throwWeapon());
    }

}
