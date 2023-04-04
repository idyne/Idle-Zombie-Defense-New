using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FateGames.Core;
using FateGames.Tweening;
public class RewindController : FateMonoBehaviour
{
    [SerializeField] private UnityEvent onRewindStarted, onRewindEnded;
    [SerializeField] private FloatVariable rewindDuration;
    public void Rewind()
    {
        onRewindStarted.Invoke();
        FaTween.DelayedCall(rewindDuration.Value, onRewindEnded.Invoke);
    }
}
