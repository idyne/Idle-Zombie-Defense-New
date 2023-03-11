using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FateGames.Core
{
    public interface IPooledObject
    {
        public event Action OnRelease;
        public void OnObjectSpawn();
        public void Release();
        public void Activate();
        public void Deactivate();
    }
}

