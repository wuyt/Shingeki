using System;
using UnityEngine;

namespace Shingeki
{
    /// <summary>
    /// 关键点
    /// </summary>
    [Serializable]
    public class KeyPoint
    {
        /// <summary>
        /// 坐标
        /// </summary>
        public Vector3 position;
        /// <summary>
        /// 角度
        /// </summary>
        public Quaternion rotation;
        /// <summary>
        /// 名称
        /// </summary>
        public string name;
        /// <summary>
        /// 类型：0=扫描点；1=途经点；2=目标点
        /// </summary>
        public int pointType;
    }
}

