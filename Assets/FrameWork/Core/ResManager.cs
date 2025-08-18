using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Collections;
using System;

namespace FrameWork.Core
{
    /// <summary>
    /// 资源管理器
    /// </summary>
    public class ResManager : Singleton<ResManager>
    {
        private Dictionary<string, AssetBundle> _mBundleDic = new Dictionary<string, AssetBundle>();

        /// <summary>
        /// 获取Bundle包资源
        /// </summary>
        /// <param name="bundleName"></param>
        public AssetBundle GetBundle(string bundlePath)
        {
            _mBundleDic.TryGetValue(bundlePath, out AssetBundle bundle);
            if (bundle == null)
            {
                bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath +
                                                               ("/" + bundlePath + ".ab")));
            }

            return bundle;
        }

        public T GetResources<T>(string name, string bundlName = BundlNameEnum.resources)
            where T : UnityEngine.Object
        {
            if (bundlName == BundlNameEnum.resources)
            {
            }
            else
            {
                AssetBundle assetBundle = GetBundle(bundlName);
                // var list = assetBundle.LoadAllAssets();
                // Debug.Log("全部资源");
                // foreach (var asset in list)
                // {
                //     Debug.Log(asset.ToString());
                //     Debug.LogWarning(asset.ToString());
                //     if (asset.GetType() == typeof(UnityEngine.GameObject))
                //     {
                //         // GameObject go = (GameObject)GameObject.Instantiate(asset);
                //         return asset as T;
                //     }
                // }

                if (assetBundle != null)
                {
                    T asset = assetBundle.LoadAsset<T>(name);
                    Debug.Log("加载成功");
                    Debug.Log(asset);
                    return asset;
                }
            }

            return null;
        }


        /// <summary>
        /// 异步加载Bundle包资源
        /// </summary>
        /// <param name="bundleName">包名</param>
        /// <param name="onComplete">加载完成的回调函数</param>
        /// <returns></returns>
        public IEnumerator GetBundleASycn(string bundleName, System.Action<AssetBundle> onComplete)
        {
            _mBundleDic.TryGetValue(bundleName, out AssetBundle bundle);
            if (bundle == null)
            {
                AssetBundleCreateRequest bundleLoadRequest =
                    AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, (bundleName + ".ab")));
                yield return bundleLoadRequest;
                AssetBundle myLoadedAssetBundle = bundleLoadRequest.assetBundle;
                if (!myLoadedAssetBundle)
                {
                    Debug.Log("AssetBundle 加载失败！");
                    onComplete?.Invoke(null);
                    yield break;
                }

                _mBundleDic.Add(bundleName, myLoadedAssetBundle);
                onComplete?.Invoke(myLoadedAssetBundle);
            }
            else
            {
                onComplete?.Invoke(bundle);
            }
        }
    }
}