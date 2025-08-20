using System;
using System.Collections.Generic;
using FrameWork.Core;
using UnityEngine;

namespace FrameWork.UI
{
    public class UIManager : SingletonMono<UIManager>
    {
        [SerializeField] private Canvas mainCanvas; // 在Inspector中拖拽赋值 
        private Dictionary<string, BaseView> _viewPrefabDic = new Dictionary<string, BaseView>();
        [NonSerialized] public GameObject NormalRoot;
        [NonSerialized] public GameObject PopupRoot;
        [NonSerialized] public GameObject TipRoot;
        [NonSerialized] public GameObject WarnRoot;


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

        public void ShowView(string viewName,object parames, string bundlName = "", bool isAnimation = true)
        {
            if (this.IsLog)
                Debug.Log("打开界面--->" + viewName);
            BaseView viewPrefab = null;
            _viewPrefabDic.TryGetValue(viewName + "_bundle_" + bundlName, out viewPrefab);
            if (viewPrefab == null)
            {
                if (bundlName != "")
                {
                    viewPrefab = BundleManager.Instance.GetResources<BaseView>(viewName, bundlName);
                }
                else
                {
                    viewPrefab = BundleManager.Instance.GetResourcesWithAll<BaseView>(viewName, out bundlName);
                }

                if (viewPrefab)
                    _viewPrefabDic.Add(viewName + "_bundle_" + bundlName, viewPrefab);
                else
                {
                    Debug.Log("没有找到界面--->" + viewName);
                    return;
                }
            }
            BaseView view = GameObject.Instantiate<BaseView>(viewPrefab, this.GetRoot(viewPrefab.layerType));
            // view.Show(parames);
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
            GameObject node = new GameObject(("NormalRoot"));
            node.transform.SetParent(mainCanvas.transform);
            node.transform.localScale = Vector3.one;
            return node;
        }
    }
}