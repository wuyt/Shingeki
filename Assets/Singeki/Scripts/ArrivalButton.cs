using UnityEngine;
using UnityEngine.UI;

namespace Shingeki
{
    public class ArrivalButton : MonoBehaviour
    {
        /// <summary>
        /// 导航目的地
        /// </summary>
        public Transform arrival;

        void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        }

        /// <summary>
        /// 按钮被点击
        /// </summary>
        private void OnClick()
        {
            GameObject.Find("GameMaster").SendMessage("ButtonClicked", arrival);
        }
    }
}

