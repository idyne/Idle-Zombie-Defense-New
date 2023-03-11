using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FateGames.Core
{
    public class FateMonoBehaviour : MonoBehaviour
    {
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
            gameObject.SetActive(false);
        }
    }

}
