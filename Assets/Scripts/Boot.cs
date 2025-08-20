using System.Collections;
using FrameWork.Event;
using FrameWork.EventDefine;
using FrameWork.GameLogic;
using FrameWork.PatchLogic;
using UnityEngine;
using YooAsset;

public class Boot : MonoBehaviour
{
    /// <summary>
    /// 资源系统运行模式
    /// </summary>
    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;


    private IEnumerator Start()
    {
        // 游戏管理器
        GameManager.Instance.Behaviour = this;

        // 初始化事件系统
        UniEvent.Initalize();

        // 初始化资源系统
        YooAssets.Initialize();

        // 加载更新页面
        // var go = Resources.Load<GameObject>("PatchWindow");
        // GameObject.Instantiate(go);

        // 开始补丁更新流程
        var operation = new PatchOperation("DefaultPackage", PlayMode);
        YooAssets.StartOperation(operation);
        yield return operation;

        // 设置默认的资源包
        var gamePackage = YooAssets.GetPackage("DefaultPackage");
        YooAssets.SetDefaultPackage(gamePackage);
        
        // 切换到主页面场景
        SceneEventDefine.ChangeToHomeScene.SendEventMessage();
    }
}