using System.Collections;
using FrameWork.Event;
using FrameWork.EventDefine;
using UnityEngine;
using YooAsset;

namespace FrameWork.GameLogic
{
    public class GameManager 
    {
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameManager();
                return _instance;
            }
        }

        private readonly EventGroup _eventGroup = new EventGroup();

        /// <summary>
        /// 协程启动器
        /// </summary>
        public MonoBehaviour Behaviour;


        private GameManager()
        {
            // 注册监听事件
            _eventGroup.AddListener<SceneEventDefine.ChangeToHomeScene>(OnHandleEventMessage);
            _eventGroup.AddListener<SceneEventDefine.ChangeToBattleScene>(OnHandleEventMessage);
        }

        /// <summary>
        /// 开启一个协程
        /// </summary>
        public void StartCoroutine(IEnumerator enumerator)
        {
            Behaviour.StartCoroutine(enumerator);
        }

        /// <summary>
        /// 接收事件
        /// </summary>
        private void OnHandleEventMessage(IEventMessage message)
        {
            if (message is SceneEventDefine.ChangeToHomeScene)
            {
                YooAssets.LoadSceneAsync("MainScene");
            }
            else if (message is SceneEventDefine.ChangeToBattleScene)
            {
                YooAssets.LoadSceneAsync("scene_battle");
            }
        }
    }
}