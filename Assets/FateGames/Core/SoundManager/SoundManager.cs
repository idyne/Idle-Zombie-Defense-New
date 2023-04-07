using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames.Core
{
    [CreateAssetMenu(menuName = "Fate/Sound/SoundManager")]
    public class SoundManager : ScriptableObject
    {
        [SerializeField] private GameStateVariable gameState;
        [SerializeField] private BoolVariable soundOn;
        [SerializeField] private GameObject soundWorkerPrefab;
        [SerializeField] private WorkingSoundWorkerSet workingWorkerSet;
        [SerializeField] private AvailableSoundWorkerSet availableWorkerSet;
        private int workerCount { get => workingWorkerSet.Items.Count + availableWorkerSet.Items.Count; }

        public void Initialize()
        {
            soundOn.Value = true;
        }

        public void StopWorkers()
        {
            foreach (SoundWorker worker in workingWorkerSet.Items)
                worker.Stop();
        }

        private SoundWorker GetAvailableWorker()
        {
            SoundWorker worker = null;
            void GetWorker() { if (availableWorkerSet.Items.Count > 0) worker = availableWorkerSet.Items[0]; }
            GetWorker();
            if (worker == null)
            {
                DoubleWorkers();
                GetWorker();
            }
            return worker;
        }

        private void DoubleWorkers()
        {
            int number = workerCount > 0 ? workerCount : 1;
            for (int i = 0; i < number; i++)
                InstantiateWorker();
        }

        private void InstantiateWorker()
        {
            Instantiate(soundWorkerPrefab);
        }

        public void PlaySoundOneShot(SoundEntity entity)
        {
            PlaySound(entity, Vector3.zero);
        }
        public void PlaySoundOneShotIgnorePause(SoundEntity entity)
        {
            PlaySound(entity, Vector3.zero, true);
        }

        public SoundWorker PlaySound(SoundEntity entity, bool ignoreListenerPause = false)
        {
            return PlaySound(entity, Vector3.zero, ignoreListenerPause);
        }

        public SoundWorker PlaySound(SoundEntity entity, Vector3 position, bool ignoreListenerPause = false, bool pauseOnStartIfGamePaused = false)
        {
            if (entity == null) return null;
            if (!ignoreListenerPause && !pauseOnStartIfGamePaused && gameState.Value == GameState.PAUSED) return null;
            SoundWorker worker = GetAvailableWorker();
            float pitch = Random.Range(entity.PitchRangeMin, entity.PitchRangeMax);
            worker.Initialize(entity.Clip, entity.Volume, pitch, entity.SpatialBlend, entity.Loop, position, ignoreListenerPause);
            worker.Play();
            if (!ignoreListenerPause && gameState.Value == GameState.PAUSED && pauseOnStartIfGamePaused)
            {
                worker.Pause();
            }
            return worker;
        }

    }
}
