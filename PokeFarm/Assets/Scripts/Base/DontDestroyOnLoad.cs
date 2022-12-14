using System;
using UnityEngine;

namespace Base
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        public static DontDestroyOnLoad Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(Instance);
                return;
            }
            
            Destroy(gameObject);
        }
    }
}