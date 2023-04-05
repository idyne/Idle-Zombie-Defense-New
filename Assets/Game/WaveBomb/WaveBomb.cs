using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using FateGames.Tweening;

public class WaveBomb : FateMonoBehaviour
{
    private float range = 100;
    private bool explode = false;
    private float currentRange = 1;
    [SerializeField] private LayerMask zombieLayerMask;
    [SerializeField] private ObjectPool megaExplosionYellow;

    private void FixedUpdate()
    {
        if (!explode) return;
        Debug.Log("1");
        Collider[] colliders = Physics.OverlapSphere(transform.position, currentRange, zombieLayerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Zombie zombie = colliders[i].GetComponent<Zombie>();
            zombie.Hit(int.MaxValue);
        }
        Debug.Log("2");
        if (currentRange >= range)
        {
            explode = false;
            return;
        }
        currentRange += Time.fixedDeltaTime * 26;

    }
    public void Explode()
    {
        Debug.Log("3");
        megaExplosionYellow.Get<Transform>(transform.position, Quaternion.identity);
        currentRange = 1;
        FaTween.DelayedCall(1.5f, () => { explode = true; });
        
    }
}
