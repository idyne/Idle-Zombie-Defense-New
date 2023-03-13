using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class ShooterTest : MonoBehaviour
{
    [SerializeField] private ObjectPool zombiePool;
    [SerializeField] private ObjectPool crookPool;
    [SerializeField] private ObjectPool gangsterPool;
    [SerializeField] private ObjectPool sniperPool;
    [SerializeField] private ObjectPool infantryPool;
    [SerializeField] private ObjectPool butcherPool;
    [SerializeField] private ObjectPool bazookaPool;
    [SerializeField] private ObjectPool missileLauncherPool;

    private void Awake()
    {
        InputManager.GetKeyDownEvent(KeyCode.Q).AddListener(() =>
        {
            Vector3 position = new(Random.Range(-10, 10), 0, Random.Range(10, 20));
            zombiePool.Get<Zombie>(position, Quaternion.identity);

        });
        InputManager.GetKeyDownEvent(KeyCode.Alpha1).AddListener(() =>
        {
            Vector3 position = new(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            crookPool.Get<Soldier>(position, Quaternion.identity);

        });
        InputManager.GetKeyDownEvent(KeyCode.Alpha2).AddListener(() =>
        {
            Vector3 position = new(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            gangsterPool.Get<Soldier>(position, Quaternion.identity);

        });
        InputManager.GetKeyDownEvent(KeyCode.Alpha3).AddListener(() =>
        {
            Vector3 position = new(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            sniperPool.Get<Soldier>(position, Quaternion.identity);

        });
        InputManager.GetKeyDownEvent(KeyCode.Alpha4).AddListener(() =>
        {
            Vector3 position = new(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            infantryPool.Get<Soldier>(position, Quaternion.identity);

        });
        InputManager.GetKeyDownEvent(KeyCode.Alpha5).AddListener(() =>
        {
            Vector3 position = new(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            butcherPool.Get<Soldier>(position, Quaternion.identity);

        });
        InputManager.GetKeyDownEvent(KeyCode.Alpha6).AddListener(() =>
        {
            Vector3 position = new(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            bazookaPool.Get<Soldier>(position, Quaternion.identity);

        });
        InputManager.GetKeyDownEvent(KeyCode.Alpha7).AddListener(() =>
        {
            Vector3 position = new(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            missileLauncherPool.Get<Soldier>(position, Quaternion.identity);

        });
    }

    private void Start()
    {
        /*Vector3 position = new(Random.Range(-5, 5), 0, Random.Range(-5, 5));
        missileLauncherPool.Get<Soldier>(position, Quaternion.identity);*/
    }
}
