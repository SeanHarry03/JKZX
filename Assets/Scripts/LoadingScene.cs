using System;
using UnityEngine;
using FrameWork.Core;
using FrameWork.Core.UI;
using FrameWork.UIComponent;
using System.Collections;
using UnityEngine.SceneManagement;
using YooAsset;

public class LoadingScene : MonoBehaviour
{
    public ProgressBar progressBar = null;
    private float progress = 0;

    private void Awake()
    {
        ResManager.Instance.GetResources<GameObject>("One_MainPanel", BundlNameEnum.game_one);
    }

    private void Update()
    {
        this.progress += Time.deltaTime;
        this.progressBar.updateProgress(this.progress);
        if (this.progress >= 1)
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}