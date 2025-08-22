using DG.Tweening;
using FrameWork.Core;
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

        public string ViewName { get; private set; }

        private void OnCreate()
        {
            this.ViewName = this.gameObject.name;
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="isAnimation">是否需要动画</param>
        /// <param name="parames"></param>
        public void Show(object parames = null)
        {
            this._isShow = true;
            this.ExtraData = parames;
            if (!this._isInit)
            {
                this._isInit = true;
                this.OnCreate();
                this.Init();
            }
            else
            {
                this.OnReresh();
            }

            this.SetActive(true);
            this.BeforShow();
            this.OpenAnimation();
        }

        /// <summary>
        /// 隐藏界面
        /// </summary>
        public void Hide(bool isAnimation = true)
        {
            this._isShow = false;
            if (isAnimation)
            {
                this.CloseAnimation(false, () => { this.SetActive(false); });
            }
            else
            {
                this.SetActive(false);
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
            if (this._isClose)
            {
                Debug.LogWarning("界面已经关闭！");
                return;
            }

            this._isClose = true;
            this.BeforeClose();
            //TODO：发送界面关闭的事件

            //关闭界面的动画
            if (isAnimation)
            {
                this.CloseAnimation(isDestroy);
            }
            else
            {
                this.DestroySelf();
            }
        }

        /// <summary>
        /// 关闭按钮的点击事件
        /// </summary>
        public virtual void OnCloseBtn()
        {
            UIManager.Instance.CloseView(this.ViewName);
        }

        protected virtual void OpenAnimation(TweenCallback callback = null)
        {
            _tween.Kill();
            transform.localScale = Vector3.zero;
            _tween = transform.DOScale(Vector3.one, tweenDuration).SetEase(Ease.OutBack).SetUpdate(true)
                .OnComplete(() =>
                {
                    if (!this || this._isDestroying)
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

        protected virtual void CloseAnimation(bool isDestroy, TweenCallback callback = null)
        {
            _tween.Kill();
            _tween = transform.DOScale(Vector3.zero, tweenDuration).SetEase(Ease.InBack).SetUpdate(true)
                .OnComplete(() =>
                {
                    if (!this || this._isDestroying)
                    {
                        Debug.LogWarning("访问了已经销毁的对象！");
                        return;
                    }

                    if (callback != null)
                    {
                        callback();
                    }

                    if (isDestroy)
                        this.DestroySelf();
                });
        }

        private void SetActive(bool state)
        {
            if (this.gameObject.activeInHierarchy != state)
                this.gameObject.SetActive(state);
        }

        private void DestroySelf()
        {
            if (!this._isDestroying)
            {
                this._isDestroying = true;
                GameObject o = this.gameObject;
                o.SetActive(false);
                GameObject.Destroy(o);
            }

            this.AfterClose();
        }

        protected abstract void Init();

        protected virtual void OnReresh()
        {
        }

        /// <summary>
        /// 界面打开前
        /// </summary>
        protected virtual void BeforShow()
        {
        }

        /// <summary>
        /// 打开动画结束后调用
        /// </summary>
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
        /// 动画播放前调用
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
    }
}