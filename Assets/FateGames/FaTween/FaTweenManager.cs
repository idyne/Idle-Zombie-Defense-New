using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FateGames.Tweening
{
    public class FaTweenManager : MonoBehaviour
    {
        #region Singenlton
        private static FaTweenManager instance = null;

        public static FaTweenManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("FaTweenManager").AddComponent<FaTweenManager>();
                    //DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }
        #endregion

        private static List<FaCore> tweens;

        public void Add(FaCore tween)
        {
            if (!tweens.Contains(tween))
                tweens.Add(tween);
            else
                Debug.Log("FaTween add Error");
        }

        public void Remove(FaCore tween)
        {
            if (tweens.Contains(tween))
                tweens.Remove(tween);
            else
                Debug.Log("FaTween remove Error");
        }

        private void Awake()
        {
            tweens = new List<FaCore>();
        }

        private void Update()
        {
            for (int i = 0; i < tweens.Count; i++)
            {
                tweens[i].Proceed();
            }
        }
    }
}
