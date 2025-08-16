using System;
using UnityEngine;

namespace FrameWork.Core
{
    public class Singleton<T> where T : new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new();
                }

                return _instance;
            }
        }
    }
}