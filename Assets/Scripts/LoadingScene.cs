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

        // 根据不同平台生成路径
        if (Application.platform == RuntimePlatform.Android)
        {
            filePath = "jar:file://" + Application.dataPath + "!/assets/dog.jpg";
        }
        else
        {
            filePath = "file://" + Application.streamingAssetsPath + "/dog.jpg";
        }

        // 使用WWW加载资源
        WWW www = new WWW(filePath);
        yield return www;

        // 将加载的图片应用到材质
        if (www.texture != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.mainTexture = www.texture;
        }
    }
}
