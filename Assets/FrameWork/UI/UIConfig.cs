namespace FrameWork.UI
{
    /// <summary>
    /// UI配置信息
    /// </summary>
    public enum UIConfig
    {
        MainPanel,
        One_GameMainPanel,
    }

    /// <summary>
    /// UI适配方案
    /// </summary>
    public enum UIAdaptorTypeEnum
    {
        /// <summary>
        /// 顶部对齐
        /// </summary>
        Top,

        /// <summary>
        /// 底部对齐
        /// </summary>
        Bottom,

        /// <summary>
        /// 左侧对齐
        /// </summary>
        Left,

        /// <summary>
        /// 右侧对齐
        /// </summary>
        Right,

        /// <summary>
        /// 全屏界面
        /// </summary>
        Normal,

        /// <summary>
        /// 弹窗界面
        /// </summary>
        Popup,

        /// <summary>
        /// 提示界面
        /// </summary>
        Tip,
    }

    /// <summary>
    /// UI层级设置
    /// </summary>
    public enum LayerTypeEnum
    {
        /// <summary>
        /// 普通界面
        /// </summary>
        Normal,

        /// <summary>
        /// 弹窗界面
        /// </summary>
        Popup,

        /// <summary>
        /// 提示界面
        /// </summary>
        Tip,

        /// <summary>
        /// 错误提示界面（最顶层，所有界面之上）
        /// </summary>
        Warn,
    }
}