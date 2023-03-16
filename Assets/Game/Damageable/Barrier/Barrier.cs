using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Damageable
{
    public override void Die()
    {
        Deactivate();
    }

}
