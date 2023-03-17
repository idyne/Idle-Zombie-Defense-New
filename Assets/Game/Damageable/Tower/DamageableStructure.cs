using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageableStructure : Damageable
{
    private void OnTriggerEnter(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable)
            damageable.OnTriggerEnterNotification();
    }
    private void OnTriggerExit(Collider other)
    {
        Damageable damageable = other.GetComponent<Damageable>();
        if (damageable)
            damageable.OnTriggerExitNotification();
    }

}
