using UnityEngine;

namespace FrameWork.Core
{
    public enum TimerStation
    {
        NotWorked,
        DoWorking,
        DoneWorked
    }

    public delegate void TimerAction();

    public class GameTimer
    {
        private float m_StartTime;
        private float m_LifeCycle;
        private TimerAction m_Task;
        private TimerStation m_TimerStation;
        private bool m_IsStopTime;
        private bool m_IsRealTime;
        public TimerStation TimerStation => m_TimerStation;
        public bool IsRealTime => m_IsRealTime;

        public float Progress
        {
            get
            {
                if (m_TimerStation == TimerStation.DoWorking)
                {
                    return (m_LifeCycle - m_StartTime) / m_LifeCycle;
                }
                else if (m_TimerStation == TimerStation.NotWorked)
                {
                    return 0;
                }
                else if (m_TimerStation == TimerStation.DoneWorked)
                {
                    return 1;
                }

                return 0;
            }
        }

        public GameTimer() //new初始化
        {
            ResetTimer();
        }

        public void StartTimer(bool isRealTime, float startTime, TimerAction task, bool isRepeat = false)
        {
            m_IsRealTime = isRealTime;
            m_StartTime = startTime;
            m_LifeCycle = startTime;
            m_Task = task;
            m_IsStopTime = false;
            m_TimerStation = TimerStation.DoWorking;
        }

        //前提是他还在执行
        public void ReSetStartTimer(float startTime, bool isAddTime = false, TimerAction task = null)
        {
            m_StartTime = isAddTime ? m_StartTime + startTime : startTime;
            m_LifeCycle = m_StartTime;
            if (task != null)
            {
                m_Task = task;
            }

            m_TimerStation = TimerStation.DoWorking;
        }

        public void UpdateTimer(float time)
        {
            if (m_IsStopTime) return;

            m_StartTime -= time;

            if (m_StartTime <= 0)
            {
                m_Task?.Invoke();
                m_IsStopTime = true;
                m_TimerStation = TimerStation.DoneWorked;
            }
        }

        /// <summary>
        /// 不会受到ScaleTime影响
        /// </summary>
        public void UpdateRealTimer(float time)
        {
            if (m_IsStopTime) return;

            m_StartTime -= time;

            if (m_StartTime <= 0)
            {
                m_Task?.Invoke();
                m_IsStopTime = true;
                m_TimerStation = TimerStation.DoneWorked;
            }
        }

        public void ResetTimer(bool isActionDone = false)
        {
            if (isActionDone) m_Task?.Invoke();
            m_TimerStation = TimerStation.NotWorked;
            m_StartTime = 0;
            m_LifeCycle = 0;
            m_Task = null;
            m_IsStopTime = true;
            m_IsRealTime = false;
        }
    }
}