using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FrameWork.Core
{
    public class BundleManager : Singleton<BundleManager>
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

        public T GetResources<T>(string name, string bundlName = BundlNameEnum.Resources)
            where T : UnityEngine.Object
        {
            if (bundlName == BundlNameEnum.Resources)
            {
            }
            else
            {
                AssetBundle assetBundle = GetBundle(bundlName);

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
        /// 从当前所有的包中查找指定的资源，返回第一个结果
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetResourcesWithAll<T>(string name, out string bundleName) where T : UnityEngine.Object
        {
            T resAsset = Resources.Load<T>(name);
            if (resAsset)
            {
                bundleName = BundlNameEnum.Resources;
                return resAsset;
            }

            foreach (string key in _mBundleDic.Keys)
            {
                _mBundleDic.TryGetValue(key, out AssetBundle bundle);
                if (bundle)
                {
                    T asset = bundle.LoadAsset<T>(name);
                    if (asset)
                    {
                        bundleName = key;
                        return asset;
                    }
                }
                else
                {
                    bundleName = null;
                    Debug.LogError("AssetBundle 加载失败！");
                    break;
                }
            }

            bundleName = null;
            Debug.Log("没有找到资源：" + name);
            return null;
        }

        /// <summary>
        /// 异步加载Bundle包资源
        /// </summary>
        /// <param name="bundleName">包名</param>
        /// <<param name="onProgress">加载进度的回调函数</param>
        /// <param name="onComplete">加载完成的回调函数</param>
        /// <returns></returns>
        public IEnumerator GetBundleASycn(string bundleName, System.Action<float> onProgress,
            System.Action<AssetBundle> onComplete)
        {
            _mBundleDic.TryGetValue(bundleName, out AssetBundle bundle);
            if (bundle == null)
            {
                AssetBundleCreateRequest bundleLoadRequest =
                    AssetBundle.LoadFromFileAsync(Path.Combine(Application.streamingAssetsPath, (bundleName + ".ab")));
                // yield return bundleLoadRequest;
                onProgress?.Invoke(0f);
                while (!bundleLoadRequest.isDone)
                {
                    onProgress?.Invoke(bundleLoadRequest.progress);
                    yield return null;
                }

                onProgress?.Invoke(1f);

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