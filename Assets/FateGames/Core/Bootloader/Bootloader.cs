using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace FateGames.Core
{
    public class Bootloader : MonoBehaviour
    {
        private static bool booted = false;
        private static Bootloader instance = null;
        [SerializeField] private ZoneManager zoneManager;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private SceneManager sceneManager;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private SoundManager soundManager;
        [SerializeField] private GameStateVariable gameState;
        private void Awake()
        {
            if (instance == null) instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
            Boot();
            Destroy(gameObject);
        }
        public void Boot()
        {
            if (booted) return;
            gameState.Value = GameState.BOOTING;
            DOTween.SetTweensCapacity(200, 312);
            saveManager.Initialize();
            gameManager.Initialize();
            InputManager.Initialize();
            soundManager.Initialize();
            zoneManager.Initialize();
            sceneManager.Initialize();
            ObjectPooler.Initialize();
            booted = true;
        }


    }

}

