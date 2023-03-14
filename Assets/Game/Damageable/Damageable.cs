using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.Events;

public abstract class Damageable : FateMonoBehaviour
{
    [SerializeField] protected int maxHealth;
    [SerializeField] protected Transform shotPoint;
    protected int health;
    public UnityEvent OnDied = new();
    public int FutureHealth { get; private set; }
    public bool GoingToDie { get => FutureHealth <= 0; }
    public Transform ShotPoint { get => shotPoint; }

    public readonly UnityEvent OnGoingToDie = new();
#if DEBUG
    public List<string> logs = new();
#endif
    protected virtual void OnEnable()
    {
#if DEBUG
        logs.Add("OnEnable");
#endif
        ResetHealth();
        ResetFutureHealth();
    }
    public void AddFutureHealth(int gain)
    {
#if DEBUG
        logs.Add("AddFutureHealth");
#endif
        FutureHealth = Mathf.Clamp(FutureHealth + gain, 0, maxHealth);
        if (FutureHealth <= 0)
        {
            AnnounceFutureDeath();
        }
    }

    public virtual void AnnounceFutureDeath()
    {
#if DEBUG
        logs.Add("AnnounceFutureDeath");
#endif
        OnGoingToDie.Invoke();
    }

    public virtual bool Hit(int damage, bool addFutureHealth = false)
    {
#if DEBUG
        logs.Add("Hit");
#endif
        if (health <= 0) return false;
        SetHealth(health - damage);
        if (addFutureHealth) AddFutureHealth(-damage);
        return true;
    }

    public void SetHealth(int health)
    {
#if DEBUG
        logs.Add("SetHealth");
#endif
        health = Mathf.Clamp(health, 0, maxHealth);
        this.health = health;
        if (health <= 0)
            Die();
    }

    public void ResetHealth()
    {
#if DEBUG
        logs.Add("ResetHealth");
#endif
        health = maxHealth;
    }
    public void ResetFutureHealth()
    {
#if DEBUG
        logs.Add("ResetFutureHealth");
#endif
        FutureHealth = maxHealth;
    }

    public abstract void Die();

}
