using FateGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdSoundListener : MonoBehaviour
{
    [SerializeField] private SoundEntity sound;
    [SerializeField] private SoundManager soundManager;
    private SoundWorker worker;

    public void StartSound()
    {
        worker = soundManager.PlaySound(sound);
    }

    public void StopSound()
    {
        if (worker == null) return;
        worker.Stop();
    }
}
