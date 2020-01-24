using UnityEngine;
using UnityEngine.UI;

namespace Shingeki
{
    /// <summary>
    /// 关键点按钮脚本
    /// </summary>
    public class KeyPointButton : MonoBehaviour
    {
        /// <summary>
        /// 关键点
        /// </summary>
        public KeyPoint point;

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
