using UnityEngine;
using UnityEngine.UI;

namespace FrameWork
{
    /// <summary>
    /// ���������
    /// </summary>
    public class ProgressBar : MonoBehaviour
    {
        public Image FillBar = null;

        /// <summary>
        /// ���½���
        /// </summary>
        /// <param name="progress"></param>
        public void updateProgress(float progress)
        {
            if (progress < 0)
            {
                Debug.LogWarning("��������");
                return;
            }
            this.FillBar.fillAmount = progress;
        }
    }

}

