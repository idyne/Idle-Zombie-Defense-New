using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public abstract class Weapon : FateMonoBehaviour
{
    [SerializeField] protected float cooldown = 1;
    protected WaitForSeconds waitForCooldown;
    protected IEnumerator useRoutine = null;
    public bool InCooldown { get; protected set; }

    protected virtual void OnEnable()
    {
        ResetCooldown();
        waitForCooldown = new WaitForSeconds(cooldown);
    }
    public abstract IEnumerator Use(Damageable target);

    public void ResetCooldown()
    {
        InCooldown = false;
    }

    public IEnumerator Cooldown()
    {
        InCooldown = true;
        yield return waitForCooldown;
        InCooldown = false;
    }

    public void Stop()
    {
        if(useRoutine == null)
        {
            Debug.LogWarning("Routine is null", this);
            return;
        }
        StopCoroutine(useRoutine);
    }
}
