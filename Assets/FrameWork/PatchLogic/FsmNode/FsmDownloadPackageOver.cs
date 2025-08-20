﻿using FrameWork.EventDefine;
using FrameWork.Machine;
using UnityEngine;

namespace FrameWork.PatchLogic
{
    internal class FsmDownloadPackageOver : IStateNode
    {
        private StateMachine _machine;

        void IStateNode.OnCreate(StateMachine machine)
        {
            _machine = machine;
        }
        void IStateNode.OnEnter()
        {
            PatchEventDefine.PatchStepsChange.SendEventMessage("资源文件下载完毕！");
            _machine.ChangeState<FsmClearCacheBundle>();
        }
        void IStateNode.OnUpdate()
        {
        }
        void IStateNode.OnExit()
        {
        }
    }
}