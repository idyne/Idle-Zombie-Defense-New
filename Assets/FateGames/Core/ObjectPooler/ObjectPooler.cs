using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames.Core
{
    public static class ObjectPooler
    {
        private static ObjectPool[] pools;

        public static void Initialize()
        {
            pools = Resources.FindObjectsOfTypeAll<ObjectPool>();
        }

        public static void OnNewLevel()
        {
            Initialize();
            foreach (ObjectPool pool in pools)
            {
                pool.OnNewLevel();
            }
        }
    }
}

