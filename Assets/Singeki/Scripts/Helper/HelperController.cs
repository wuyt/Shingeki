using UnityEngine;

namespace Shingeki.Helper
{
    /// <summary>
    /// 辅助功能控制
    /// </summary>
    public class HelperController : MonoBehaviour
    {
        /// <summary>
        /// 画布
        /// </summary>
        private GameObject canvas;


        void Awake()
        {
            var helpers = GameObject.FindObjectsOfType<HelperController>();
            for (int i = 1; i < helpers.Length; i++)
            {
                Destroy(helpers[i].gameObject);
            }
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
            //赋值
            canvas = transform.Find("Canvas").gameObject;
            //设定默认状态
            canvas.SetActive(false);
        }

        void Update()
        {
            //如果按下返回键，修改画布状态
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                canvas.SetActive(!canvas.activeSelf);
            }
        }
    }
}