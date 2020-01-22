using UnityEngine;
using UnityEngine.UI;
using System;
using easyar;

namespace Shingeki.Helper
{
    /// <summary>
    /// 显示信息
    /// </summary>
    public class ShowInformation : MonoBehaviour
    {
        /// <summary>
        /// 文本
        /// </summary>
        private Text text;
        /// <summary>
        /// 摄像机
        /// </summary>
        private GameObject goCamera;
        /// <summary>
        /// easyAR运动跟踪根游戏对象
        /// </summary>
        private GameObject goWorld;
        /// <summary>
        /// 导航根游戏对象
        /// </summary>
        private GameObject goNavRoot;
        /// <summary>
        /// 图片目标数组
        /// </summary>
        private ImageTargetController[] itcs;

        void Start()
        {
            text = transform.Find("Canvas/Panel/TextInformation")
            .GetComponent<Text>();
        }

        void OnEnable()
        {
            goCamera = GameObject.Find("/Main Camera");
            goWorld = GameObject.Find("/WorldRoot");
            goNavRoot = GameObject.Find("/NavRoot");
            itcs = GameObject.FindObjectsOfType<ImageTargetController>();
        }

        void OnDisable()
        {
            goCamera = null;
            goWorld = null;
            goNavRoot = null;
            itcs = null;
        }

        void Update()
        {
            text.text = "";
            if (goCamera != null)
            {
                text.text = string.Format(
                    "Main Camera->position:{0};rotation:{1}{2}{3}",
                    goCamera.transform.position,
                    goCamera.transform.rotation,
                    Environment.NewLine,
                    text.text);
            }
            if (goWorld != null)
            {
                text.text = string.Format(
                    "WorldRoot->position:{0};rotation:{1}{2}{3}",
                    goWorld.transform.position,
                    goWorld.transform.rotation,
                    Environment.NewLine,
                    text.text);
            }
            if (goNavRoot != null)
            {
                text.text = string.Format(
                    "NavRoot->position:{0};rotation:{1}{2}{3}",
                    goNavRoot.transform.position,
                    goNavRoot.transform.rotation,
                    Environment.NewLine,
                    text.text);
            }

            if (itcs.Length > 0)
            {
                foreach (var item in itcs)
                {
                    text.text = string.Format(
                        "{4}->position:{0};rotation:{1}{2}{3}",
                        item.transform.position,
                        item.transform.rotation,
                        Environment.NewLine,
                        text.text,
                        item.transform.name);
                }
            }
        }
    }
}