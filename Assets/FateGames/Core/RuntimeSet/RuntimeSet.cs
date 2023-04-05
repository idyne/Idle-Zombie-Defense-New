using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FateGames.Core
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
#if DEBUG
        [SerializeField] private bool logChange = false;
#endif
        public List<T> Items = new();
        public UnityEvent OnChange = new();

        public virtual void Add(T t)
        {
            if (!Items.Contains(t))
            {
                Items.Add(t);
                OnChange.Invoke();
#if DEBUG
                if (logChange)
                    Debug.Log(name + " changed!", this);
#endif
            }
        }

        public virtual void Remove(T t)
        {
            if (Items.Contains(t))
            {
                Items.Remove(t);
                OnChange.Invoke();
#if DEBUG
                if (logChange)
                    Debug.Log(name + " changed!", this);
#endif
            }
        }
    }
}
