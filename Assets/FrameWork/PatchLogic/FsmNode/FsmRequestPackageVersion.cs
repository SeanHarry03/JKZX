using System.Collections;
using FrameWork.EventDefine;
using FrameWork.GameLogic;
using FrameWork.Machine;
using UnityEngine;
using YooAsset;

namespace FrameWork.PatchLogic
{
    internal class FsmRequestPackageVersion : IStateNode
    {
        private StateMachine _machine;

        void IStateNode.OnCreate(StateMachine machine)
        {
            _machine = machine;
        }

        void IStateNode.OnEnter()
        {
            PatchEventDefine.PatchStepsChange.SendEventMessage("请求资源版本 !");
            GameManager.Instance.StartCoroutine(UpdatePackageVersion());
        }

        void IStateNode.OnUpdate()
        {
        }

        void IStateNode.OnExit()
        {
        }

        private IEnumerator UpdatePackageVersion()
        {
            var packageName = (string)_machine.GetBlackboardValue("PackageName");
            var package = YooAssets.GetPackage(packageName);
            var operation = package.RequestPackageVersionAsync();
            yield return operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning(operation.Error);
                PatchEventDefine.PackageVersionRequestFailed.SendEventMessage();
            }
            else
            {
                Debug.Log($"Request package version : {operation.PackageVersion}");
                _machine.SetBlackboardValue("PackageVersion", operation.PackageVersion);
                _machine.ChangeState<FsmUpdatePackageManifest>();
            }
        }
    }
}