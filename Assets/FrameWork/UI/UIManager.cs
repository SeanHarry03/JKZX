using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FrameWork.Core;
using UnityEngine;
using YooAsset;

namespace FrameWork.UI
{
    public class UIManager : SingletonMono<UIManager>
    {
        [SerializeField] private Canvas mainCanvas; // 在Inspector中拖拽赋值 
        [NonSerialized] public GameObject NormalRoot;
        [NonSerialized] public GameObject PopupRoot;
        [NonSerialized] public GameObject TipRoot;
        [NonSerialized] public GameObject WarnRoot;

        /// <summary>
        /// 界面缓存
        /// </summary>
        private Dictionary<string, GameObject> _viewPrefabDic = new Dictionary<string, GameObject>();

        /// <summary>
        /// 界面状态的日志打印
        /// </summary>
        [NonSerialized] public bool IsLog = true;


        public GameObject GetMainCanvas()
        {
            return mainCanvas.gameObject;
        }

        protected override void Construction()
        {
            if (mainCanvas == null)
            {
                mainCanvas = FindObjectOfType<Canvas>();
            }

            //生成界面的根节点
            this.CreateViewRoot();
        }

        public void ShowView(string viewName, bool isAnimation = true, params object[] parame)
        {
            if (this.IsLog)
                Debug.Log("打开界面--->" + viewName);
            BaseView view = null;
            GameObject viewPrefab = Resources.Load<GameObject>(viewName);
            Transform viewGo = null;
            string bundleName = "resoucres";
            if (viewPrefab == null)
            {
                //从AssetBunle加载
                AssetHandle handle = YooAssets.LoadAssetSync<GameObject>(viewName);

                viewGo = handle.InstantiateSync().transform;

                string assetPath = YooAssets.GetAssetInfo(viewName).AssetPath;
                bundleName = assetPath.Split('/')[2];
            }
            else
            {
                viewGo = GameObject.Instantiate(viewPrefab).transform;
            }

            if (viewGo)
            {
                view = viewGo.GetComponent<BaseView>();
                if (view == null)
                {
                    Debug.LogError("没有找到界面：" + viewName);
                }

                this.SetParent(viewGo, this.GetRoot(view.layerType));
                view.Show(parame);
            }
            else
            {
                Debug.LogError("没有找到资源：" + viewName);
            }
        }

        public void CloseView()
        {
        }

        public void HideView()
        {
        }


        private void CreateViewRoot()
        {
            this.NormalRoot = this.CreateGameObject("NormalRoot");
            this.PopupRoot = this.CreateGameObject("PopupRoot");
            this.TipRoot = this.CreateGameObject("TipRoot");
            this.WarnRoot = this.CreateGameObject("WarnRoot");
        }

        private Transform GetRoot(LayerTypeEnum layerType)
        {
            switch (layerType)
            {
                case LayerTypeEnum.Normal:
                    return this.NormalRoot.transform;
                case LayerTypeEnum.Popup:
                    return PopupRoot.transform;
                case LayerTypeEnum.Tip:
                    return TipRoot.transform;
                case LayerTypeEnum.Warn:
                    return WarnRoot.transform;
                default:
                    return NormalRoot.transform;
            }
        }

        private GameObject CreateGameObject(string name)
        {
            GameObject node = new GameObject((name));
            node.transform.SetParent(mainCanvas.transform);
            node.transform.localScale = Vector3.one;
            node.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            return node;
        }

        private void SetParent(Transform tempTransform, Transform parent)
        {
            tempTransform.SetParent(parent);
            tempTransform.localScale = Vector3.one;
            tempTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}