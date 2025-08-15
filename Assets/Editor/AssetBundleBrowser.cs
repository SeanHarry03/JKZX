using System.IO;
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

    [MenuItem("CustomTools/AssetBundles/��ȡ��Դ������AB��")]
    public static void FindResAB()
    {
        // ��ȡ�û���AssetsĿ¼��ѡ�еĶ��󣬲��Ҵ洢��������
        GameObject[] AllSelectObj = Selection.gameObjects;//�����û�ѡ��Ķ���
        // ����ж���ѡ�У���ִ��ѡ�в���
        if (AllSelectObj != null && AllSelectObj.Length > 0)
        {
            // Ϊѡ�еĶ������� AssetBundle ����
            foreach (Object OneSelectObj in AllSelectObj)
            {
                //AssetDatabase.GetAssetPath(selectedObject) ����ǻ�ȡѡ�����������·��           
                string OneSelectAssetPath = AssetDatabase.GetAssetPath(OneSelectObj);
                Debug.Log(OneSelectAssetPath);

                //��ȡ���������������һ����Դ��·����ͨ�����Ψһ��·����ȡ����Դ������Ϣ
                AssetImporter assetImporter = AssetImporter.GetAtPath(OneSelectAssetPath);
                //����AssetBundle���ֺͺ�׺����
                string asssetpath = assetImporter.assetPath.Replace("Assets/Game_One", "");
                Debug.Log(asssetpath);
                Debug.Log(Path.GetDirectoryName(asssetpath));
                string[] dps = AssetDatabase.GetDependencies(assetImporter.assetPath);
                Debug.Log("������Դ" + dps.Length);
                for (int i = 0; i < dps.Length; i++)
                {
                    Debug.Log(dps[i]);
                }
                //assetImporter.SetAssetBundleNameAndVariant("Models", "unity");
            }
            // ˢ�� AssetDatabase��ȷ���ڱ༭�����ܹ����������ɵ� AssetBundles
            AssetDatabase.Refresh();
        }
        else
        {
            UnityEngine.Debug.LogWarning("δѡ������");
        }
        //Debug.Log("��ѡ�е�����ָ��һ��AssetBundle����-Models");
    }
}
