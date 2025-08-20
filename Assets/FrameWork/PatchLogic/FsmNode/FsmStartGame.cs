using FrameWork.EventDefine;
using FrameWork.Machine;
using UnityEngine;

namespace FrameWork.PatchLogic
{
    internal class FsmStartGame : IStateNode
    {
        private PatchOperation _owner;

        void IStateNode.OnCreate(StateMachine machine)
        {
            _owner = machine.Owner as PatchOperation;
        }

        void IStateNode.OnEnter()
        {
            PatchEventDefine.PatchStepsChange.SendEventMessage("开始游戏！");
            _owner.SetFinish();
        }

        void IStateNode.OnUpdate()
        {
        }

        void IStateNode.OnExit()
        {
        }
    }
}