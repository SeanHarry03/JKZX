using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class AssetBundleBrowser
{
    /// <summary>
    /// key: ab包名称   value：ab包依赖的资源的Set集合
    /// </summary>
    private static Dictionary<string, HashSet<string>> _dependsDic;

    [MenuItem("CustomTools/AssetBundles/Build AssetBundle")]
    public static void BuildAssetBundle()
    {
        // 定义Ab包的输出目录
        string assetBundleDirectory = "Assets/StreamingAssets";
        // 检查目录是否存在，如果不存在则创建该目录
        if (!System.IO.Directory.Exists(assetBundleDirectory))
        {
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        }
        // 使用BuildPipeline构建AssetBundles
        // 参数1：输出目录
        // 参数2：构建选项，这里使用默认选项（None）
        // 参数3：目标平台，这里选择Windows平台

        
        
        Debug.Log("构建成功");
        // BuildAssetBundleOptions options = BuildAssetBundleOptions.None;
        // options |= BuildAssetBundleOptions.LZ4; // 使用 LZ4 压缩方式
        
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows);
    }

    [MenuItem("CustomTools/AssetBundles/获取选中资源的依赖资源")]
    public static void FindResAB()
    {
        // 获取用户在Assets目录下选中的对象，并且存储在数组中
        GameObject[] AllSelectObj = Selection.gameObjects; //返回用户选择的对象
        // 如果有对象被选中，则执行选中操作
        if (AllSelectObj != null && AllSelectObj.Length > 0)
        {
            // 为选中的对象设置 AssetBundle 名称
            foreach (Object OneSelectObj in AllSelectObj)
            {
                //AssetDatabase.GetAssetPath(selectedObject) 这句是获取选择的物体所在路径                    
                string OneSelectAssetPath = AssetDatabase.GetAssetPath(OneSelectObj);
                Debug.Log(OneSelectAssetPath);

                //获取这个遍历到的其中一个资源的路径，通过这个唯一的路径获取该资源编码信息
                AssetImporter assetImporter = AssetImporter.GetAtPath(OneSelectAssetPath);
                //设置AssetBundle名字和后缀变体
                string assetPath = assetImporter.assetPath.Replace("Assets", "");
                Debug.Log(assetPath);
                string abName = Path.GetDirectoryName(assetPath);
                Debug.Log("AB " + abName);
                AssetBundleBrowser.FindDepends(assetImporter.assetPath, GetDependsSet(abName));
                //assetImporter.SetAssetBundleNameAndVariant("Models", "unity");
            }

            // 刷新 AssetDatabase，确保在编辑器中能够看到新生成的 AssetBundles
            AssetDatabase.Refresh();
        }
        else
        {
            UnityEngine.Debug.LogWarning("未选中物体");
        }
    }

    /// <summary>
    /// 查询资源依赖的资源
    /// </summary>
    /// <param name="assetPath"></param>
    private static void FindDepends(string assetPath, HashSet<string> dependsSet)
    {
        Debug.Log("1111");
        string[] dps = AssetDatabase.GetDependencies(assetPath);
        foreach (string dependPath in dps)
        {
            Debug.Log(dependPath);
            if (dependPath.Contains(".cs"))
            {
                continue;
            }

            //��������Դ
            if (false == dependsSet.Contains(dependPath))
            {
                dependsSet.Add(dependPath);
            }
        }
    }

    private static HashSet<string> GetDependsSet(string abName)
    {
        if (false == _dependsDic.ContainsKey(abName))
        {
            _dependsDic[abName] = new HashSet<string>();
        }

        return _dependsDic[abName];
    }
}