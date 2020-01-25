using UnityEngine;
using UnityEngine.UI;

namespace Shingeki
{
    /// <summary>
    /// 路径按钮脚本
    /// </summary>
    public class RoadButton : MonoBehaviour
    {
        /// <summary>
        /// 路径
        /// </summary>
        public Road road;

        void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        /// <summary>
        /// 按钮被点击
        /// </summary>
        private void OnClick()
        {
            GameObject.Find("GameMaster").SendMessage("ButtonClicked", transform);
        }
    }
}

