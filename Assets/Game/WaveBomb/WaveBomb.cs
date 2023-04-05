using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class WaveBomb : FateMonoBehaviour
{
    private float range = 100;
    private bool explode = false;
    private float currentRange = 1;
    [SerializeField] private LayerMask zombieLayerMask;

    private void FixedUpdate()
    {
        if (!explode) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, currentRange, zombieLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Zombie zombie = colliders[i].GetComponent<Zombie>();
            zombie.Hit(int.MaxValue);
        }
        if (currentRange >= range)
        {
            explode = false;
            return;
        }
        currentRange += Time.fixedDeltaTime * 26;

    }
    public void Explode()
    {
        currentRange = 1;
        explode = true;
    }
}
