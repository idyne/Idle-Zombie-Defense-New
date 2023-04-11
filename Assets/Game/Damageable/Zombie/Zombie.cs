using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using System;
using UnityEngine.AI;
using UnityEngine.Events;
using FSG.MeshAnimator.Snapshot;
using DG.Tweening;

public class Zombie : Damageable, IPooledObject
{
    [SerializeField] protected float incomeIncrease = 0.2f;
    [SerializeField] protected ObjectPool deadZombiePool;
    [SerializeField] protected List<ZombieLevelData> levelData;
    [SerializeField] protected SaveDataVariable saveData;
    [SerializeField] protected ZombieSet zombieSet;
    [SerializeField] protected LayerMask enemyLayerMask;
    [SerializeField] protected Renderer meshRenderer;
    [SerializeField] protected int mainMaterialIndex;
    [SerializeField] protected bool mechanim = false;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SnapshotMeshAnimator meshAnimator;
    [SerializeField] private MoneyBurster moneyBurster;
    [SerializeField] private ObjectPool bloodSplash;

    private int money = 1;

    private IEnumerator flashCoroutine = null;
    public event Action OnRelease;
    private int level = 1;
    protected NavMeshAgent agent;
    protected float cooldown = 5;
    protected int damage = 5;
    private IEnumerator checkEnemiesRoutine = null;
    private WaitForSeconds waitForCheckEnemiesPeriod = new(0.2f);
    protected float lastHitTime = float.MinValue;
    private Tween freezeTween = null;
    protected bool Stopped { get => agent.isStopped; }
    public bool InCooldown { get => Time.time < lastHitTime + cooldown; }
    public bool Flashing { get => flashCoroutine != null; }
    public bool CheckingEnemies { get => checkEnemiesRoutine != null; }

    private void Awake()
    {
        Log("Awake", false);
        InitializeNavMeshAgent();
    }

    private void InitializeNavMeshAgent()
    {
        agent = gameObject.AddComponent<NavMeshAgent>();
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        agent.angularSpeed = 1440;
        agent.acceleration = 50;
        agent.radius = 0.25f;
        agent.enabled = false;
    }

    private void SetColor(Color color)
    {
        Log("SetColor", false);
        meshRenderer.materials[mainMaterialIndex].color = color;
    }
    public bool Frozen { get => freezeTween != null; }
    public void Freeze(float duration)
    {
        Unfreeze();
        ZombieLevelData data = levelData[level];
        SetSpeed(data.Speed / 2f);
        SetCooldown(data.Cooldown * 2f);
        Color originalColor = data.Color;
        Color frozenColor = new Color(originalColor.r, originalColor.g, originalColor.b + 10f, 1);
        SetColor(frozenColor);
        freezeTween = DOVirtual.DelayedCall(duration, Unfreeze);
    }
    public void Unfreeze()
    {
        if (!Frozen) return;
        freezeTween.Kill();
        freezeTween = null;
        ZombieLevelData data = levelData[level];
        SetSpeed(data.Speed);
        SetColor(data.Color);
        SetCooldown(data.Cooldown);
    }
    private void OnDisable()
    {
        Log("OnDisable", false);
        zombieSet.Remove(this);
    }

    public void SetLevel(int level)
    {
        Log("SetLevel", false);
        this.level = Mathf.Clamp(level, 1, levelData.Count);
        SetAttributes(levelData[this.level]);
    }

    public void SetAttributes(ZombieLevelData data)
    {
        Log("SetAttributes", false);
        SetSpeed(data.Speed);
        SetCooldown(data.Cooldown);
        damage = data.Damage;
        SetColor(data.Color);
        baseMaxHealth = data.MaxHealth;
        money = data.Money;
        ResetHealth();
        transform.localScale = data.Scale * Vector3.one;
    }

    public void SetCooldown(float cooldown)
    {
        this.cooldown = cooldown;
    }

    public void OnTowerDestroyed()
    {
        Log("OnTowerDestroyed", false);
        StopCheckingEnemies();
        SetSpeed(0.8f);
        SetDestinationToCenter();
    }

    public void StopCheckingEnemies()
    {
        Log("StopCheckingEnemies", false);
        if (!CheckingEnemies) return;
        StopCoroutine(checkEnemiesRoutine);
        checkEnemiesRoutine = null;
    }

    public void OnLoseScreenShowed()
    {
        Log("OnLoseScreenShowed", false);
        Stop();
    }

    public void SetSpeed(float speed)
    {
        Log("SetSpeed", false);
        if (mechanim)
            animator.speed = speed;
        else
            meshAnimator.speed = speed;
        agent.speed = speed;
    }

    protected void CheckEnemies()
    {
        Log("CheckEnemies", false);
        float radius = 1f;
        int maxColliders = 1;
        Collider[] hitColliders = new Collider[maxColliders];
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders, enemyLayerMask);
        if (numColliders > 0)
        {
            Stop();

            for (int i = 0; i < numColliders; i++)
            {
                Damageable damageable = hitColliders[i].GetComponent<Damageable>();
                Hit(damageable);
            }
        }
        else
        {
            StopCheckingEnemies();
            SetDestinationToCenter();
        }

    }

    public override void OnTriggerEnterNotification()
    {
        StartCheckingEnemies();
    }

    public override void OnTriggerExitNotification()
    {
        if (CheckingEnemies)
        {
            StopCheckingEnemies();
            SetDestinationToCenter();
        }
    }

    public void Stop()
    {
        Log("Stop", false);
        if (Stopped) return;
        agent.isStopped = true;
        //StopCheckingEnemies();
    }

    public override bool Hit(int damage)
    {
        if (health <= 0) return false;
        Flash();
        if (!base.Hit(damage)) return false;
        if (health > 0)
            Push(damage / (float)maxHealth);
        return true;
    }

    public void Push(float value)
    {
        Log("Push", false);
        if (!agent.enabled) { Debug.LogError("Agent is not enabled!", this); return; }
        value = Mathf.Clamp(value, 0, 1);
        agent.Move(2.5f * value * transform.position.normalized);
    }

    public void Flash()
    {
        Log("Flash", false);
        if (Flashing) CancelFlash();
        IEnumerator flash()
        {
            SetColor(Color.white);
            yield return new WaitForSeconds(0.05f);
            SetColor(levelData[level].Color);
        }
        flashCoroutine = flash();
        StartCoroutine(flashCoroutine);
    }

    public void CancelFlash()
    {
        Log("CancelFlash", false);
        if (!Flashing) return;
        StopCoroutine(flashCoroutine);
        flashCoroutine = null;
    }

    protected void Hit(Damageable damageable)
    {
        Log("Hit", false);
        if (InCooldown) return;
        PlayAnimation("Attack");
        damageable.Hit(damage);
        lastHitTime = Time.time;
    }

    protected void PlayAnimation(string name)
    {
        Log("PlayAnimation", false);
        //Debug.Log("PlayAnimation " + name, this);
        if (mechanim)
        {
            animator.SetTrigger(name);
        }
        else
        {
            if (name == meshAnimator.currentAnimation.AnimationName)
                meshAnimator.RestartAnim();
            else
                meshAnimator.Play(name);
        }
    }


    public override void Die()
    {
        Log("Die", false);
        DropMoney();
        zombieSet.Remove(this);
        DeadStandardZombie deadZombie = deadZombiePool.Get<DeadStandardZombie>(transform.position, Quaternion.identity);
        deadZombie.Initialize(levelData[level], transform);
        deadZombie.Animate();
        bloodSplash.Get<Transform>(shotPoint.position, Quaternion.identity);
        OnDied.Invoke();
        Release();

    }

    public void DropMoney()
    {
        Log("DropMoney", false);
        int money = Mathf.CeilToInt(this.money + saveData.Value.IncomeLevel * incomeIncrease);
        if (moneyBurster) moneyBurster.Burst(money, transform.position);
    }

    public void OnObjectSpawn()
    {
        Log("OnObjectSpawn", false);
        Activate();
        agent.enabled = true;
        zombieSet.Add(this);
        SetDestinationToCenter();
    }

    public void StartCheckingEnemies()
    {
        Log("StartCheckingEnemies", false);
        if (CheckingEnemies)
        {
            Debug.LogError("Already checking enemies!", this);
            return;
        }
        IEnumerator checkEnemies()
        {
            CheckEnemies();
            yield return waitForCheckEnemiesPeriod;
            checkEnemiesRoutine = checkEnemies();
            yield return checkEnemiesRoutine;
        }
        checkEnemiesRoutine = checkEnemies();
        StartCoroutine(checkEnemiesRoutine);
    }

    public void SetDestinationToCenter()
    {
        Log("StartCheckinSetDestinationToCentergEnemies", false);
        Vector3 destination;
        if (NavMesh.SamplePosition(transform.position.normalized, out NavMeshHit hit, 10, NavMesh.AllAreas))
        {
            destination = hit.position;
        }
        else destination = Vector3.zero;
        Debug.DrawRay(destination, Vector3.up, Color.white, 10);
        agent.isStopped = false;
        agent.SetDestination(Vector3.zero);
        PlayAnimation("Walk");
    }

    public void Release()
    {
        Log("Release", false);
        StopCheckingEnemies();
        agent.enabled = false;
        Deactivate();
        CancelFlash();
        OnDied.RemoveAllListeners();
        OnRelease.Invoke();
    }



}
