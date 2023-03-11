using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;

public class ShooterTest : MonoBehaviour
{
    [SerializeField] private ObjectPool zombiePool;
    [SerializeField] private ObjectPool crookPool;

    private void Awake()
    {
        InputManager.GetKeyDownEvent(KeyCode.Q).AddListener(() =>
        {
            Vector3 position = new(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            zombiePool.Get<Zombie>(position, Quaternion.identity);

        });
        InputManager.GetKeyDownEvent(KeyCode.E).AddListener(() =>
        {
            Vector3 position = new(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            crookPool.Get<Soldier>(position, Quaternion.identity);

        });
    }
}
