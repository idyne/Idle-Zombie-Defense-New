using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using System;
using UnityEngine.AI;
using UnityEngine.Events;
using FSG.MeshAnimator.Snapshot;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : Damageable, IPooledObject
{
    [SerializeField] protected List<ZombieLevelData> levelData;
    [SerializeField] protected SaveDataVariable saveData;
    [SerializeField] protected ZombieSet zombieSet;
    [SerializeField] protected LayerMask enemyLayerMask;
    [SerializeField] protected MeshRenderer meshRenderer;
    [SerializeField] protected SnapshotMeshAnimator meshAnimator;
    private MaterialPropertyBlock propertyBlock;
    public event Action OnRelease;
    private int level = 1;
    protected NavMeshAgent agent;
    protected float cooldown = 5;
    protected int damage = 5;
    private IEnumerator checkEnemiesRoutine = null;
    private WaitForSeconds waitForCheckEnemiesPeriod = new(0.2f);
    protected float lastHitTime = float.MinValue;
    protected bool stopped { get => agent.isStopped; }
    public bool InCooldown { get => Time.time < lastHitTime + cooldown; }

    private void Awake()
    {
#if DEBUG
        logs.Add("Awake");
#endif
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        propertyBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        InputManager.GetKeyDownEvent(KeyCode.K).AddListener(() => { PlayAnimation("Attack"); });
        InputManager.GetKeyDownEvent(KeyCode.L).AddListener(() => { PlayAnimation("Walk"); });
    }


    private void SetColor(Color color)
    {
#if DEBUG
        logs.Add("SetColor");
#endif
        meshRenderer.material.color = color;
        /*
        propertyBlock.SetColor("_BaseColor", color);
        meshRenderer.SetPropertyBlock(propertyBlock);*/
    }

    private void OnDisable()
    {
#if DEBUG
        logs.Add("OnDisable");
#endif
        zombieSet.Remove(this);
    }

    public void SetLevel(int level)
    {
#if DEBUG
        logs.Add("SetLevel");
#endif
        this.level = Mathf.Clamp(level, 1, levelData.Count);
        SetAttributes(levelData[this.level]);
    }

    public void SetAttributes(ZombieLevelData data)
    {
#if DEBUG
        logs.Add("SetAttributes");
#endif
        SetSpeed(data.Speed);
        cooldown = data.Cooldown;
        damage = data.Damage;
        SetColor(data.Color);
        maxHealth = data.MaxHealth;
        transform.localScale = data.Scale * Vector3.one;
    }

    public override void AnnounceFutureDeath()
    {
        zombieSet.Remove(this);
        base.AnnounceFutureDeath();
    }

    public void OnTowerDestroyed()
    {
        if (checkEnemiesRoutine != null) StopCoroutine(checkEnemiesRoutine);
        SetSpeed(0.8f);
        SetDestinationToCenter();
    }

    public void OnLoseScreenShowed()
    {
        Stop();
    }

    public void SetSpeed(float speed)
    {
        meshAnimator.speed = speed;
        agent.speed = speed;
    }

    protected void CheckEnemies()
    {
        //if (InCooldown) return;
        float radius = 0.5f;
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
        else if (stopped) SetDestinationToCenter();
    }

    public void Stop()
    {
#if DEBUG
        logs.Add("Stop");
#endif
        if (stopped) return;
        agent.isStopped = true;
    }

    public override bool Hit(int damage, bool addFutureHealth = false)
    {
        if (health <= 0) return false;
        Flash();
        if (!base.Hit(damage, addFutureHealth)) return false;
        return true;
    }

    public void Flash()
    {
        IEnumerator flash()
        {
            SetColor(Color.white);
            yield return new WaitForSeconds(0.1f);
            SetColor(levelData[level].Color);
        }
        StartCoroutine(flash());
    }

    protected void Hit(Damageable damageable)
    {
#if DEBUG
        logs.Add("Hit");
#endif
        if (InCooldown) return;
        PlayAnimation("Attack");
        damageable.Hit(damage, true);
        lastHitTime = Time.time;
    }

    protected void PlayAnimation(string name)
    {
#if DEBUG
        logs.Add("PlayAnimation");
#endif
        //Debug.Log("PlayAnimation " + name, this);
        if (name == meshAnimator.currentAnimation.AnimationName)
            meshAnimator.RestartAnim();
        else
            meshAnimator.Play(name);
    }


    public override void Die()
    {
#if DEBUG
        logs.Add("Die");
#endif
        DropMoney();
        Release();
        OnDied.Invoke();
    }

    public void DropMoney()
    {
#if DEBUG
        logs.Add("DropMoney");
#endif
        // TODO change to real money
        int money = 50001;
        saveData.AddMoney(money);
    }

    public void OnObjectSpawn()
    {
#if DEBUG
        logs.Add("OnObjectSpawn");
#endif
        Activate();
        agent.enabled = true;
        zombieSet.Add(this);
        SetDestinationToCenter();

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
#if DEBUG
        logs.Add("SetDestinationToCenter");
#endif
        Vector3 destination;
        if (NavMesh.SamplePosition(transform.position.normalized, out NavMeshHit hit, 10, NavMesh.AllAreas))
        {
            destination = hit.position;
        }
        else destination = Vector3.zero;
        Debug.DrawRay(destination, Vector3.up, Color.white, 10);
        agent.SetDestination(Vector3.zero);
        PlayAnimation("Walk");
    }

    public void Release()
    {
#if DEBUG
        logs.Add("Release");
#endif
        if (checkEnemiesRoutine != null) StopCoroutine(checkEnemiesRoutine);
        agent.enabled = false;
        Deactivate();
        OnRelease.Invoke();
    }



}
