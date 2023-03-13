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
        ThrowWeapon();
    }
    public void ThrowWeapon()
    {
#if DEBUG
        logs.Add("ThrowWeapon");
#endif
        if (!target) return;
        IEnumerator throwWeapon()
        {
            animator.SetTrigger("Throw");
            ThrowableProjectile throwableWeapon = throwableWeaponPool.Get<ThrowableProjectile>(throwableContainer.position, throwableContainer.rotation);
            yield return null;
            //yield return throwableWeapon.Use(target);
        }
        StartCoroutine(throwWeapon());
    }

}
