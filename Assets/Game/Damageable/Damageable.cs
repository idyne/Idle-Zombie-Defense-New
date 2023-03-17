using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.Events;

public abstract class Damageable : FateMonoBehaviour
{
    [SerializeField] protected int maxHealth;
    [SerializeField] protected Transform shotPoint;
    [SerializeField] private HealthBar healthBar;
    protected int health;
    public UnityEvent OnDied = new();
    public Transform ShotPoint { get => shotPoint; }

    protected virtual void OnEnable()
    {
        Log("OnEnable", false);
        ResetHealth();
    }

    public virtual bool Hit(int damage)
    {
        Log("Hit", false);
        if (health <= 0) return false;
        SetHealth(health - damage);
        return true;
    }

    public void SetHealth(int health)
    {
        Log("SetHealth", false);
        health = Mathf.Clamp(health, 0, maxHealth);
        this.health = health;
        if (healthBar)
        {
            healthBar.SetPercent(this.health / (float)maxHealth);
            healthBar.Show(4);
        }
        if (health <= 0)
        {
            if (healthBar)
                healthBar.Hide();
            Die();
        }
    }

    public virtual void OnTriggerEnterNotification()
    {

    }

    public virtual void OnTriggerExitNotification()
    {

    }

    public void ResetHealth()
    {
        Log("ResetHealth", false);
        health = maxHealth;
    }
    public abstract void Die();

}
