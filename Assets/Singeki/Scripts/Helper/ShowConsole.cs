using UnityEngine;
using UnityEngine.UI;
using System;

namespace Shingeki.Helper
{
    /// <summary>
    /// 显示控制台信息
    /// </summary>
    public class ShowConsole : MonoBehaviour
    {
        /// <summary>
        /// 显示文本
        /// </summary>
        private Text text;

        void Awake()
        {
            //定义文本
            text = transform.Find("Canvas/Panel/TextConsole").GetComponent<Text>();
        }

        void OnEnable()
        {
            //响应事件
            text.text = "";
            Application.logMessageReceived += Received;
        }

        void OnDisable()
        {
            //取消响应
            Application.logMessageReceived -= Received;
        }

        /// <summary>
        /// 将控制台信息发送到界面
        /// </summary>
        /// <param name="logString">日志文本</param>
        /// <param name="stackTrace">追踪</param>
        /// <param name="logType">日志类型</param>
        private void Received(string logString, string stackTrace, LogType logType)
        {
            //根据类型修改文本颜色
            string strColor = "";
            switch (logType)
            {
                case LogType.Log:
                    strColor = "white";
                    break;
                case LogType.Warning:
                    strColor = "yellow";
                    break;
                default:
                    strColor = "red";
                    break;
            }
            //拼接字符串
            string strTemp = String.Format(
                "<color={0}>{2}{1}{3}-----{4}--{5}-----</color>{1}",
             strColor,
             Environment.NewLine,
             logString,
             stackTrace,
             logType.ToString(),
             Time.time);
            //更新文本
            text.text = strTemp + text.text;
        }
    }
}