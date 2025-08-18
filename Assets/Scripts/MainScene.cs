using System.Collections;
using System.Collections.Generic;
using FrameWork.Core;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    /// <summary>
    /// 加载游戏一
    /// </summary>
    public void LoadGameOne()
    {
        GameObject mainPanel =
            GameObject.Instantiate(
                ResManager.Instance.GetResources<GameObject>("One_MainPanel", BundlNameEnum.game_one));
        mainPanel.transform.SetParent(transform);
        mainPanel.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}