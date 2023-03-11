using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames.Core
{
    [CreateAssetMenu(menuName = "Fate/Object Pooling/Object Pool")]
    [System.Serializable]
    public class ObjectPool : ScriptableObject
    {
        [SerializeField] private string tag;
        private Queue<GameObject> pool = new();
        [SerializeField] private GameObject prefab;

        public string Tag { get => tag; }

        public T Get<T>(Vector3 position, Quaternion rotation) where T : Component
        {
            if (pool.Count == 0)
                AddObjectToPool();
            GameObject obj = pool.Dequeue();
            obj.transform.SetPositionAndRotation(position, rotation);
            IPooledObject pooledObject = obj.GetComponent<IPooledObject>();
            if (pooledObject != null)
                pooledObject.OnObjectSpawn();
            return obj.GetComponent<T>();
        }

        private void AddObjectToPool()
        {
            GameObject obj = Instantiate(prefab);
            IPooledObject pooledObject = obj.GetComponent<IPooledObject>();
            if (pooledObject != null)
                pooledObject.OnRelease += () => { pool.Enqueue(obj); };
            pool.Enqueue(obj);
        }

        public void ClearPool()
        {
            pool = new();
        }

    }
}

