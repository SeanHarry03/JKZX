using UnityEngine;
using UnityEngine.UI;

namespace FrameWork.UIComponent
{
    /// <summary>
    /// 进度条组件
    /// </summary>
    public class ProgressBar : MonoBehaviour
    {
        public Image FillBar = null;

        /// <summary>
        /// 更新进度条
        /// </summary>
        /// <param name="progress"></param>
        public void updateProgress(float progress)
        {
            if (progress < 0)
            {
                Debug.LogWarning("参数不能小于0");
                return;
            }
            this.FillBar.fillAmount = progress;
        }
    }

}

