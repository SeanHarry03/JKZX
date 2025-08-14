using UnityEngine;
using FrameWork;
public class LoadingScene : MonoBehaviour
{
    public ProgressBar progressBar = null;

    // Start is called before the first frame update
    void Start()
    {
        AssetBundle assetBundle = AssetBundle.LoadFromFile("Assets/AssetBundles/game_one.ab");
        var list = assetBundle.LoadAllAssets();
        Debug.Log(list);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
