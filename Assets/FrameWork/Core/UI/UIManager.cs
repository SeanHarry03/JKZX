using UnityEngine;

namespace FrameWork.Core.UI
{
    public class UIManager : Singleton<UIManager>
    {
        public void ShowView()
        {
            Debug.Log("Show View");
        }
    }
}