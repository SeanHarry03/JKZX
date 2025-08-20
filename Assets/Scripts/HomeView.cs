using System;
using System.Collections;
using FrameWork.UI;
using UnityEngine;
using YooAsset;


public class HomeView : BaseView
{
    protected override void Init()
    {
    }

    protected override void BeforClose()
    {
        Debug.Log("关闭之前！");
    }

    protected override void AfterClose()
    {
        Debug.Log("关闭之后");
    }

    public void OnButtonClick(string name)
    {
        // StartCoroutine(LoadGame(name));
        this.LoadSync(name);
    }

    private void LoadSync(string name)
    {
        UIManager.Instance.ShowView(name);
    }

    IEnumerator LoadGame(string name)
    {
        AssetHandle handle = YooAssets.LoadAssetAsync<GameObject>("Tow_MainPanel");
        yield return handle;
        handle.InstantiateSync(this.transform);
    }
}