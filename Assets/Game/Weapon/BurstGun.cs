using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstGun : Gun
{
    [SerializeField] private int burstCount = 3;
    [SerializeField] private float burstPeriod = 0.1f;
    private WaitForSeconds waitForBurstPeriod;

    private void Awake()
    {
        waitForBurstPeriod = new(burstPeriod);
    }

    public override void Shoot(Damageable target)
    {
        StartCoroutine(Burst(burstCount, target));
    }

    private IEnumerator Burst(int count, Damageable target)
    {
        if (count <= 0)
            yield break;
        base.Shoot(target);
        yield return waitForBurstPeriod;
        yield return Burst(count - 1, target);
    }

}
