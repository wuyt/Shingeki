using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;

namespace Shingeki
{
    /// <summary>
    /// 公共功能
    /// </summary>
    public class Common : MonoBehaviour
    {


        void Awake()
        {
            pathKeyPoints = Application.persistentDataPath + "/keypoints.txt";
            pathRoads = Application.persistentDataPath + "/roads.txt";
        }

        #region 普通功能

        /// <summary>
        /// 退出应用
        /// </summary>
        public void Exit()
        {
            Application.Quit();
        }

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="name">场景名称</param>
        public void SceneLoad(string name)
        {
            SceneManager.LoadScene(name);
        }

        #endregion

        #region  保存读取关键点和路径

        /// <summary>
        /// 关键点保存路径
        /// </summary>
        private string pathKeyPoints;
        /// <summary>
        /// 路径保存路径
        /// </summary>
        private string pathRoads;
        /// <summary>
        /// 保存关键点
        /// </summary>
        /// <param name="jsons">关键点json数组</param>
        public void SaveKeyPoints(string[] jsons)
        {
            SaveStringArray(jsons, pathKeyPoints);
        }
        /// <summary>
        /// 保存路径
        /// </summary>
        /// <param name="jsons">路径json数组</param>
        public void SaveRoads(string[] jsons)
        {
            SaveStringArray(jsons, pathRoads);
        }
        /// <summary>
        /// 读取关键点
        /// </summary>
        /// <returns>关键点json列表</returns>
        public List<string> LoadKeyPoints()
        {
            return LoadStringList(pathKeyPoints);
        }
        /// <summary>
        /// 读取路径
        /// </summary>
        /// <returns>路径json列表</returns>
        public List<string> LoadRoads()
        {
            return LoadStringList(pathRoads);
        }
        /// <summary>
        /// 保存字符串数组到文件
        /// </summary>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="path">保存路径</param>
        private void SaveStringArray(string[] stringArray, string path)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    foreach (var s in stringArray)
                    {
                        writer.WriteLine(s);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
        /// <summary>
        /// 读取文本到字符串列表
        /// </summary>
        /// <param name="path">读取路径</param>
        /// <returns>字符串列表</returns>
        private List<string> LoadStringList(string path)
        {
            List<string> list = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        list.Add(sr.ReadLine());
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
            return list;
        }
        #endregion
    }
}