using UnityEngine;
using FrameWork;
using System.Collections;
public class LoadingScene : MonoBehaviour
{
    public ProgressBar progressBar = null;

    // Start is called before the first frame update
    void Start()
    {
        AssetBundle assetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/game_one.ab");
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

    IEnumerator LoadResource()
    {
        string filePath = string.Empty;

        // ���ݲ�ͬƽ̨����·��
        if (Application.platform == RuntimePlatform.Android)
        {
            filePath = "jar:file://" + Application.dataPath + "!/assets/dog.jpg";
        }
        else
        {
            filePath = "file://" + Application.streamingAssetsPath + "/dog.jpg";
        }

        // ʹ��WWW������Դ
        WWW www = new WWW(filePath);
        yield return www;

        // �����ص�ͼƬӦ�õ�����
        if (www.texture != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = www.texture;
        }
    }
}
