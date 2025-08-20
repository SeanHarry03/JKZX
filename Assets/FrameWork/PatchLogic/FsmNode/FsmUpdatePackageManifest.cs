using System.Collections;
using FrameWork.EventDefine;
using FrameWork.GameLogic;
using FrameWork.Machine;
using UnityEngine;
using YooAsset;

namespace FrameWork.PatchLogic
{
    public class FsmUpdatePackageManifest : IStateNode
    {
        private StateMachine _machine;

        void IStateNode.OnCreate(StateMachine machine)
        {
            _machine = machine;
        }

        void IStateNode.OnEnter()
        {
            PatchEventDefine.PatchStepsChange.SendEventMessage("更新资源清单！");
            GameManager.Instance.StartCoroutine(UpdateManifest());
        }

        void IStateNode.OnUpdate()
        {
        }

        void IStateNode.OnExit()
        {
        }

        private IEnumerator UpdateManifest()
        {
            var packageName = (string)_machine.GetBlackboardValue("PackageName");
            var packageVersion = (string)_machine.GetBlackboardValue("PackageVersion");
            var package = YooAssets.GetPackage(packageName);
            var operation = package.UpdatePackageManifestAsync(packageVersion);
            yield return operation;

            if (operation.Status != EOperationStatus.Succeed)
            {
                Debug.LogWarning(operation.Error);
                PatchEventDefine.PackageManifestUpdateFailed.SendEventMessage();
                yield break;
            }
            else
            {
                _machine.ChangeState<FsmCreateDownloader>();
            }
        }
    }
}