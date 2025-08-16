namespace FrameWork.Core.UI
{
    /// <summary>
    /// UI配置信息
    /// </summary>
    public class UIConfig
    {
        private static ViewInfo LobbyView { get; } = new ViewInfo()
        {
            desc = "大厅界面",
            name = "LobbyView",
            resPath = " Prefab/LobbyView",
            bundleName = BundlNameEnum.resources,
            layerType = LayerType.view,
        };
    }


    public struct ViewInfo
    {
        /// <summary>
        /// 视图描述
        /// </summary>
        public string desc;

        /// <summary>
        /// 视图名称
        /// </summary>
        public string name;

        /// <summary>
        /// 资源路径
        /// </summary>
        public string resPath;

        /// <summary>
        /// 所属的Bundle名称
        /// </summary>
        public string bundleName;

        /// <summary>
        /// UI层级类型
        /// </summary>
        public LayerType layerType;
    }

    /// <summary>
    /// UI层级类型
    /// </summary>
    public enum LayerType
    {
        /// <summary>
        /// 顶部对齐
        /// </summary>
        top,

        /// <summary>
        /// 底部对齐
        /// </summary>
        bottom,

        /// <summary>
        /// 全屏界面
        /// </summary>
        view,

        /// <summary>
        /// 弹窗界面
        /// </summary>
        popup,

        /// <summary>
        /// 提示界面
        /// </summary>
        tip,
    }
}