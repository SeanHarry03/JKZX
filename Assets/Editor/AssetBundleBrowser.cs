using UnityEditor;
using UnityEngine;
public class AssetBundleBrowser
{
    [MenuItem("CustomTools/AssetBundles/Build AssetBundle")]
    public static void BuildAssetBundle()
    {
        // ����Ab�������Ŀ¼
        string assetBundleDirectory = "Assets/AssetBundles";
        // ���Ŀ¼�Ƿ���ڣ�����������򴴽���Ŀ¼
        if (!System.IO.Directory.Exists(assetBundleDirectory))
        {
            System.IO.Directory.CreateDirectory(assetBundleDirectory);
        }
        // ʹ��BuildPipeline����AssetBundles
        // ����1�����Ŀ¼
        // ����2������ѡ�����ʹ��Ĭ��ѡ�None��
        // ����3��Ŀ��ƽ̨������ѡ��Windowsƽ̨


        Debug.Log("��ʼ���");
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);

        Debug.Log("������");
    }
}
