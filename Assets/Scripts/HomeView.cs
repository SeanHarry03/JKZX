using System.Collections;
using FrameWork.UI;
using UnityEngine;
using YooAsset;

public class HomeView : BaseView
{
    protected override void Init()
    {
    }

    public void OnButtonClick(string name)
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