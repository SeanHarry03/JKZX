using UnityEditor;
using UnityEngine;
public class AssetBundleBrowser
{
    [MenuItem("CustomTools/AssetBundles/Build AssetBundle")]
    public static void BuildAssetBundle()
    {
        // 定义Ab包的输出目录
        string assetBundleDirectory = "Assets/AssetBundles";
        // 检查目录是否存在，如果不存在则创建该目录
        if (!System.IO.Directory.Exists(assetBundleDirectory))
        {
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        }
        // 使用BuildPipeline构建AssetBundles
        // 参数1：输出目录
        // 参数2：构建选项，这里使用默认选项（None）
        // 参数3：目标平台，这里选择Windows平台


        Debug.Log("开始打包");
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        Debug.Log("打包完成");
    }
}
