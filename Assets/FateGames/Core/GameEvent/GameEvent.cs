using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames.Core
{
    [CreateAssetMenu(menuName = "Fate/Event", fileName = "Game Event")]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> listeners = new List<GameEventListener>();
        public void Raise()
        {
#if DEBUG
            Debug.Log(name + " raised!", this);
#endif
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].OnEventRaised();
        }

        public void RegisterListener(GameEventListener listener) => listeners.Add(listener);
        public void UnregisterListener(GameEventListener listener) => listeners.Remove(listener);
    }
}