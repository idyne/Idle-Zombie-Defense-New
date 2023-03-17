using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : DamageableStructure
{
    public override void Die()
    {
        Deactivate();
    }

}
