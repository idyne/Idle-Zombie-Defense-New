using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FateGames.Core
{
    public abstract class RuntimeSet<T> : ScriptableObject
    {
        public List<T> Items = new();
        public UnityEvent OnChange = new();

        public virtual void Add(T t)
        {
            if (!Items.Contains(t))
            {
                Items.Add(t);
                OnChange.Invoke();
#if DEBUG
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
                Debug.Log(name + " changed!", this);
#endif
            }
        }
    }
}
