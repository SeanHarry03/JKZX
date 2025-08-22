using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.Core
{
    /// <summary>
    /// 基础的Pool
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectPool<T> : IDisposable
    {
        private Queue<T> m_Pool;
        private List<T> m_Active;
        private Func<T> m_PreLoadFunc;
        private Action<T> m_GetAction;
        private Action<T> m_ReturnAction;
        private Action<T> m_DestroyAction;

        internal bool m_CollectionCheck;
        private int m_MaxSize;

        public int AllCount => m_Pool.Count + m_Active.Count;
        public int UsableCount => m_Active.Count;


        public Queue<T> Pool => m_Pool;
        public List<T> ActivePool => m_Active;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="preLoadFunc">实例化方法</param>
        /// <param name="getAction">获取回调</param>
        /// <param name="returnAction">回收回调</param>
        /// <param name="destroyAction"></param>
        /// <param name="collectionCheck">检查元素是否已经入池</param>
        /// <param name="preloadCount">预加载数量</param>
        /// <param name="maxSize"></param>
        public ObjectPool(Func<T> preLoadFunc, Action<T> getAction, Action<T> returnAction, Action<T> destroyAction,
            bool collectionCheck, int preloadCount, int maxSize)
        {
            m_PreLoadFunc = preLoadFunc;
            m_GetAction = getAction;
            m_ReturnAction = returnAction;
            m_DestroyAction = destroyAction;
            m_CollectionCheck = collectionCheck;
            m_MaxSize = maxSize;
            if (preLoadFunc == null)
            {
                Debug.LogError("preLoadFunc is null!");
                return;
            }

            var count = Mathf.Max(preloadCount, m_MaxSize);
            m_Pool = new(count);
            m_Active = new(count / 2);

            //preLoad
            for (int i = 0; i < count; i++)
            {
                Return(preLoadFunc.Invoke());
            }
        }

        public T Get()
        {
            T item;
            item = m_Pool.Count > 0 ? m_Pool.Dequeue() : m_PreLoadFunc.Invoke();
            m_GetAction?.Invoke(item);
            m_Active.Add(item);
            return item;
        }

        public void Return(T item)
        {
            if (this.m_CollectionCheck && this.m_Pool.Count > 0)
            {
                foreach (var poolItem in m_Pool)
                {
                    if ((object)item == (object)poolItem)
                        throw new InvalidOperationException(
                            "Trying to release an object that has already been released to the pool.");
                }
            }

            m_Active.Remove(item);
            m_ReturnAction?.Invoke(item);

            if (this.m_Pool.Count < this.m_MaxSize)
            {
                this.m_Pool.Enqueue(item);
            }
            else
            {
                //多了放不进就直接销毁
                m_DestroyAction?.Invoke(item);
            }
        }

        public void AllReturn()
        {
            foreach (var item in m_Active.ToArray())
            {
                Return(item);
            }
        }

        public void Clear()
        {
            AllReturn();
            if (this.m_DestroyAction != null)
            {
                foreach (T obj in this.m_Pool)
                    this.m_DestroyAction(obj);
            }

            this.m_Pool.Clear();
        }

        public void Dispose() => this.Clear();
    }
}