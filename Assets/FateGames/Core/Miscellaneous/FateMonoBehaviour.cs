using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using FateGames.Tweening;
namespace FateGames.Core
{
    public class FateMonoBehaviour : MonoBehaviour
    {
#if DEBUG

        [System.Serializable]
        private struct LogPart
        {
            public string message;
            public float time;

            public LogPart(string message)
            {
                this.message = message;
                this.time = Time.time;
            }
        }

        private List<LogPart> _logs = new();
        private void AddLog(LogPart log) { _logs.Add(log); }
#endif
        public void Log(string message, bool printOnConsole = true)
        {
            //return
            if (printOnConsole)
                Debug.Log(message, this);
#if DEBUG
            AddLog(new(message));
#endif
        }

        private Transform _transform = null;
#pragma warning disable CS0108
        public Transform transform
#pragma warning restore CS0108
        {
            get
            {
                if (_transform == null)
                    _transform = base.transform;
                return _transform;
            }
        }
        public virtual void Activate()
        {
            gameObject.SetActive(true);
        }

        public virtual void Deactivate()
        {
            DOTween.Kill(this);
            gameObject.SetActive(false);
        }
    }

}

