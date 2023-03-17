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
        [SerializeField] private int startSize = 0;
        protected int totalSize = 0;
        protected Transform poolContainer = null;

        public string Tag { get => tag; }


        public T Get<T>(Vector3 position, Quaternion rotation) where T : Component
        {
            while (totalSize < startSize)
                AddObjectToPool(true);
            if (pool.Count == 0)
                AddObjectToPool();
            if (poolContainer == null)
                poolContainer = new GameObject(tag + " Pool").transform;
            GameObject obj = pool.Dequeue();
            Transform objTransform = obj.transform;
            objTransform.SetParent(poolContainer);
            objTransform.SetPositionAndRotation(position, rotation);
            IPooledObject pooledObject = obj.GetComponent<IPooledObject>();
            if (pooledObject != null)
                pooledObject.OnObjectSpawn();
            return obj.GetComponent<T>();
        }

        private void AddObjectToPool(bool deactivateOnStart = false)
        {
            GameObject obj = Instantiate(prefab);
            IPooledObject pooledObject = obj.GetComponent<IPooledObject>();
            if (pooledObject != null)
                pooledObject.OnRelease += () => { pool.Enqueue(obj); };
            pool.Enqueue(obj);
            totalSize += 1;
            obj.SetActive(!deactivateOnStart);
        }

        public void ClearPool()
        {
            pool = new();
        }

        public void OnNewLevel()
        {
            poolContainer = null;
            totalSize = 0;
            ClearPool();
        }

    }
}

