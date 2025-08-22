using System;
using System.Collections.Generic;
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
        /// 界面预制体资源缓存
        /// </summary>
        private Dictionary<string, GameObject> _viewPrefabDic = new Dictionary<string, GameObject>();

        private Dictionary<string, BaseView> _viewDic = new Dictionary<string, BaseView>();

        /// <summary>
        /// 当前的全屏展示界面
        /// </summary>
        private Stack<BaseView> _normalViewStack = new Stack<BaseView>();

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
            if (string.IsNullOrEmpty(viewName))
            {
                Debug.LogWarning("viewName 不能为空");
                return;
            }

            if (this.IsLog)
                Debug.Log("打开界面--->" + viewName);


            BaseView view = this.GetView(viewName);
            if (view)
            {
                view.Show(parame);
                return;
            }

            GameObject viewPrefab = null;
            Transform viewGo = null;
            this._viewPrefabDic.TryGetValue(viewName, out viewPrefab);
            if (viewPrefab == null)
            {
                viewPrefab = Resources.Load<GameObject>(viewName);
                if (viewPrefab == null)
                {
                    //从AssetBunle加载
                    AssetHandle handle = YooAssets.LoadAssetSync<GameObject>(viewName);
                    viewPrefab = handle.AssetObject as GameObject;
                }

                _viewPrefabDic.Add(viewName, viewPrefab);
            }

            viewGo = GameObject.Instantiate(viewPrefab).transform;
            if (viewGo)
            {
                viewGo.gameObject.name = viewName;
                view = viewGo.GetComponent<BaseView>();
                if (view == null)
                {
                    Debug.LogError("没有找到界面：" + viewName);
                }

                this.SetParent(viewGo, this.GetRoot(view.layerType));
                view.Show(parame);
                //上一层界面隐藏
                if (view.layerType == LayerTypeEnum.Normal)
                {
                    if (this._normalViewStack.Count > 0)
                    {
                        this._normalViewStack.Peek().Hide();
                    }

                    this._normalViewStack.Push(view);
                }
            }
            else
            {
                Debug.LogError("没有找到资源：" + viewName);
            }

            _viewDic.Add(viewName, view);
        }

        public void CloseView(string viewName, bool isAnimation = true)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                Debug.LogWarning("viewName 不能为空");
                return;
            }

            BaseView view = this.GetView(viewName);
            if (!view)
            {
                Debug.LogWarning("没有找到界面：" + viewName);
                return;
            }

            _viewDic.Remove(viewName);
            if (view.layerType == LayerTypeEnum.Normal)
            {
                this._normalViewStack.Pop();
                if (this._normalViewStack.Count > 0)
                {
                    this._normalViewStack.Peek().Show();
                }
            }

            view.Close(true, isAnimation);
        }

        public void HideView(string viewName, bool isAnimation = true)
        {
            BaseView view = this.GetView(viewName);
            if (view)
            {
                if (view.layerType == LayerTypeEnum.Normal && this._normalViewStack.Count > 0)
                {
                    this._normalViewStack.Pop();
                    this._normalViewStack.Peek().Show();
                }

                view.Hide(isAnimation);
            }
        }

        public BaseView GetView(string viewName)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                Debug.LogWarning("UI名称不能给空");
                return null;
            }

            this._viewDic.TryGetValue(viewName, out BaseView view);

            return view;
        }

        /// <summary>
        /// 关闭所有的弹窗
        /// <param name="isAnimation">显示关闭动画</param>
        /// </summary>
        public void CloseAllPoupView(bool isAnimation = false)
        {
            if (this.IsLog)
            {
                Debug.Log("关闭所有的弹窗"!);
            }

            //弹窗界面查找
            for (int popupRootChild = 0; popupRootChild < this.PopupRoot.transform.childCount; popupRootChild++)
            {
                Transform child = this.PopupRoot.transform.GetChild(popupRootChild);
                BaseView view = child.GetComponent<BaseView>();
                if (view)
                {
                    view.Close(true, isAnimation);
                    this._viewDic.Remove(view.ViewName);
                }
            }
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

        private GameObject CreateGameObject(string goName)
        {
            GameObject node = new GameObject((goName));
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