using System;
using System.Threading.Tasks;
using UnityEngine;

namespace FrameWork.Core
{
    /// <summary>
    /// NewTimerManager 轻量高效定时器
    /// 当前定时器程序在Update中执行，使用Parallel类方法
    /// ObjectPool对象池对象回收
    /// 已及更方便灵活的调用方式
    /// </summary>
    public class NewTimerManager : SingletonMono<NewTimerManager>
    {
        [SerializeField, Header("配置计时器的数量")] private int timerCount = 10;
        private ObjectPool<GameTimer> m_ObjectPool;
        public int AllItemCount => m_ObjectPool.AllCount;
        public int ActiveItemCount => m_ObjectPool.UsableCount;


        protected override void Construction()
        {
            base.Construction();
            m_ObjectPool = new ObjectPool<GameTimer>(CreateTimer, null, null, null, true, timerCount, 100);
        }

        private void Update()
        {
            UpdateTime();
        }

        private GameTimer CreateTimer()
        {
            var timer = new GameTimer();
            return timer;
        }


        /// <summary>
        /// 计时器
        /// 获取</summary>
        /// <param name="timer">计时的时间</param>
        /// <param name="action">计时完成后调用的委托</param>
        /// <param name="scaleTimeControl">是否会受到ScaleTime影响的计时器</param>
        /// <returns>计时器</returns>
        public GameTimer GetTimer(float timer, TimerAction action, bool scaleTimeControl = false)
        {
            GameTimer gameTimer = m_ObjectPool.Get();
            gameTimer.StartTimer(scaleTimeControl, timer, action);
            return gameTimer;
        }

        /// <summary>
        /// 计时器
        /// 获取</summary>
        /// <param name="gameTimer">计时器</param>
        /// <param name="timer">计时的时间</param>
        /// <param name="isAddTime"></param>
        /// <param name="action">计时完成后调用的委托</param>
        /// <param name="scaleTimeControl"></param>
        /// <returns>计时器</returns>
        public GameTimer ResetTimer(GameTimer gameTimer, float timer, TimerAction action, bool isAddTime = false,
            bool scaleTimeControl = false)
        {
            //说明这个计时器已经被回收了,重新启用一个计时器
            if (gameTimer != null && gameTimer.TimerStation == TimerStation.DoWorking)
            {
                gameTimer.ReSetStartTimer(timer, isAddTime, action);
            }
            else
            {
                gameTimer = GetTimer(timer, action, scaleTimeControl);
            }

            return gameTimer;
        }


        /// <summary>
        /// 计时器，按输入间隔执行一次Action，最后退出时如果时间未达到要求也不会调用
        /// TODO 建议还是走 Mono的RepeatInvoke
        /// 获取</summary>
        /// <param name="timer">计时的时间</param>
        /// <param name="interval">间隔时间</param>
        /// <param name="intervalAction"></param>
        /// <param name="action">计时完成后调用的委托</param>
        /// <param name="isRepeat">间隔时间调用</param>
        /// <param name="scaleTimeControl">是否会受到ScaleTime影响的计时器</param>
        /// <returns>计时器</returns>
        [Obsolete("请使用 Mono的RepeatInvoke")]
        public GameTimer GetTimerExecutionIntervalSecond(float timer, float interval, Action intervalAction,
            Action action, bool isRepeat,
            bool scaleTimeControl = false)
        {
            return null;
        }

        /// <summary>
        /// 停止计时器的方法
        /// </summary>
        /// <param name="gameTimer"></param>
        /// <param name="needActionDone">是否需要直接完成委托</param>
        public void CalcelTimer(GameTimer gameTimer, bool needActionDone = false)
        {
            if (gameTimer == null) return;
            //非工作计时器不能被销毁，因为可能会注册其他事件
            if (gameTimer.TimerStation != TimerStation.DoWorking) return;
            //是否需要完成委托
            gameTimer.ResetTimer(needActionDone);
            m_ObjectPool.Return(gameTimer);
        }

        /// <summary>
        /// 推动WorkingTimer的计时方法，以及处理完成工作的Timer
        /// </summary>
        private void UpdateTime()
        {
            if (ActiveItemCount == 0) return;
            var deltaTime = Time.deltaTime;
            var unscaledDeltaTime = Time.unscaledDeltaTime;

            Parallel.For(0, m_ObjectPool.ActivePool.Count, i =>
            {
                var timer = m_ObjectPool.ActivePool[i];
                if (timer.TimerStation == TimerStation.DoWorking)
                {
                    if (timer.IsRealTime)
                    {
                        timer.UpdateRealTimer(unscaledDeltaTime);
                    }
                    else
                    {
                        timer.UpdateTimer(deltaTime);
                    }
                }
            });

            for (int i = 0; i < m_ObjectPool.ActivePool.Count; i++)
            {
                var timer = m_ObjectPool.ActivePool[i];
                if (timer.TimerStation == TimerStation.DoneWorked)
                {
                    timer.ResetTimer(); //确保线程安全，不在Parallel中执行Task
                    m_ObjectPool.Return(timer);
                }
            }
        }
    }
}