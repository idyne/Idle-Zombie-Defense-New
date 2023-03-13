using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : UnguidedProjectile
{
    [SerializeField] private Transform trailEffect;
    [SerializeField] private GameObject meshObject;

    public override void OnObjectSpawn()
    {
        trailEffect.SetParent(transform);
        trailEffect.localPosition = Vector3.zero;
        base.OnObjectSpawn();
    }
    public override void OnReached()
    {
        trailEffect.SetParent(null);
        base.OnReached();
    }
}
