using System;
using UnityEngine;
using FrameWork.Core;
using FrameWork.Core.UI;
using FrameWork.UIComponent;
using System.Collections;

public class LoadingScene : MonoBehaviour
{
    public ProgressBar progressBar = null;

    private void Awake()
    {
        // ResManager.Instance.GetResources<GameObject>("One_MainPanel",BundlNameEnum.game_one);
        // StartCoroutine((ResManager.Instance.GetBundleASycn(BundlNameEnum.game_one, LoadTest)));
        AbTest();
    }

    private void LoadTest(AssetBundle assetBundle)
    {
        if (assetBundle != null)
        {
            Debug.Log("AssetBundle loaded: " + assetBundle.name);
            // 在这里使用加载的AssetBundle
        }
        else
        {
            Debug.Log("Failed to load AssetBundle!");
        }
    }

    private void AbTest()
    {
        AssetBundle assetBundle = AssetBundle.LoadFromFile("Assets/StreamingAssets/game_one.ab");
        var list = assetBundle.LoadAllAssets();
        foreach (var asset in list)
        {
            Debug.Log(asset.ToString());
            if (asset.GetType() == typeof(UnityEngine.GameObject))
            {
                Debug.LogWarning(asset.ToString());
                GameObject go = (GameObject)Instantiate(asset);
                go.transform.SetParent(this.transform);
                go.transform.localPosition = Vector3.zero;
            }
        }
    }
}