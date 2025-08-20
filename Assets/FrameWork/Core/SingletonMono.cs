using System;
using UnityEngine;

namespace FrameWork.Core
{
    public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        _instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }

            this.Construction();
        }

        /// <summary>
        /// 初始化函数
        /// </summary>
        protected virtual void Construction()
        {
        }
    }
}