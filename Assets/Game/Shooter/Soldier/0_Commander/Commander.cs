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
        else
            StopTargeting();
        ThrowWeapon();
    }
    public void ThrowWeapon()
    {
#if DEBUG
        logs.Add("ThrowWeapon");
#endif
        IEnumerator throwWeapon()
        {
            animator.SetTrigger("Throw");
            gun.Deactivate();
            yield return new WaitForSeconds(0.3f);
            ThrowableProjectile throwableWeapon = throwableWeaponPool.Get<ThrowableProjectile>(throwableContainer.position, throwableContainer.rotation);
            throwableWeapon.transform.SetParent(throwableContainer);
            Zombie nearestZombie = FindNearestZombie();
            Vector3 position = nearestZombie.transform.position;
            yield return new WaitForSeconds(0.63f);
            
            Face(position, 0.2f);
            throwableWeapon.transform.SetParent(null);
            throwableWeapon.Shoot(position);
            yield return new WaitForSeconds(0.28f);
            gun.Activate();
            yield return new WaitForSeconds(0.39f);
            StartTargeting();
        }
        StartCoroutine(throwWeapon());
    }

    public void Test()
    {
        Debug.Log("Test worked!", this);
    }

}
