using DG.Tweening;
using FrameWork.Core;
using JetBrains.Annotations;
using UnityEngine;

namespace FrameWork.UI
{
    public abstract class BaseView : MonoBehaviour
    {
        public string bundleName = BundlNameEnum.Defualt;
        public LayerTypeEnum layerType = LayerTypeEnum.Normal;
        public UIAdaptorTypeEnum uiAdaptorType = UIAdaptorTypeEnum.Normal;
        public object ExtraData;

        [SerializeField] private float tweenDuration = 0.3f;
        private Tween _tween;

        //状态
        private bool _isClose = false;
        private bool _isShow = false;
        private bool _isInit = false;
        private bool _isDestroying = false;

        private void Awake()
        {
            this.BeforShow();
            this.OpenAnimation(null);
        }

        private void OnDestroy()
        {
            this._isDestroying = true;
            this.AfterClose();
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="parames"></param>
        public void Show(object parames)
        {
            this.ExtraData = parames;
            if (!this._isInit)
            {
                this._isInit = true;
                this.Init();
            }

            if (this._isShow)
            {
                this.OnReresh();
            }

            this._isShow = true;
            this.SetActive(true);
        }

        /// <summary>
        /// 隐藏界面
        /// </summary>
        public void Hide(bool isAnimation = true)
        {
            this._isShow = false;
            if (isAnimation)
            {
                this.CloseAnimation(null);
            }
            else
            {
                this.BeforClose();
            }
        }

        /// <summary>
        /// 关闭界面
        /// <param name="isDestroy">是否销毁节点</param>
        /// <param name="isAnimation">是否需要动画</param>
        /// </summary>
        public void Close(bool isDestroy = true, bool isAnimation = true)
        {
            this._isClose = true;
            this.BeforeClose();
            //TODO：发送界面关闭的事件

            //关闭界面的动画
            if (isAnimation)
            {
                this.CloseAnimation(null);
            }
            else
            {
                this.BeforClose();
            }
        }

        protected abstract void Init();

        protected virtual void OnReresh()
        {
        }

        protected virtual void BeforShow()
        {
        }

        protected virtual void BeforeClose()
        {
        }

        /// <summary>
        /// 显示动画结束后调用
        /// </summary>
        protected virtual void AfterShow()
        {
        }


        /// <summary>
        /// 关闭动画结束后调用
        /// </summary>
        protected virtual void BeforClose()
        {
        }


        /// <summary>
        /// 界面销毁后调用
        /// </summary>
        protected virtual void AfterClose()
        {
        }


        private void SetActive(bool state)
        {
            if (this.gameObject.activeInHierarchy != state)
                this.gameObject.SetActive(state);
        }

        protected virtual void OpenAnimation([CanBeNull] TweenCallback callback)
        {
            _tween.Kill();
            transform.localScale = Vector3.zero;
            _tween = transform.DOScale(Vector3.one, tweenDuration).SetEase(Ease.OutBack).SetUpdate(true)
                .OnComplete(() =>
                {
                    if (this == null || !this.gameObject)
                    {
                        Debug.LogWarning("访问了已经销毁的对象！");
                        return;
                    }

                    if (callback != null)
                    {
                        callback();
                    }

                    this.AfterShow();
                });
        }

        protected virtual void CloseAnimation([CanBeNull] TweenCallback callback)
        {
            _tween.Kill();
            _tween = transform.DOScale(Vector3.zero, tweenDuration).SetEase(Ease.InBack).SetUpdate(true)
                .OnComplete(() =>
                {
                    if (this == null || !this.gameObject)
                    {
                        Debug.LogWarning("访问了已经销毁的对象！");
                        return;
                    }

                    if (callback != null)
                    {
                        callback();
                    }

                    this.BeforClose();
                });
        }
    }
}