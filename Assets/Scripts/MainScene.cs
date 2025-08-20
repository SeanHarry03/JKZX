using System;
using System.Collections;
using FrameWork.UI;
using UnityEngine;
using YooAsset;

public class MainScene : MonoBehaviour
{
    private void Start()
    {
        // GameObject.Instantiate(Resources.Load<GameObject>("HomeView"), this.transform);
        UIManager.Instance.ShowView("HomeView");
    }
}