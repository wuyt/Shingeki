using System;
using UnityEngine;

namespace Shingeki
{
    /// <summary>
    /// 路径
    /// </summary>
    [Serializable]
    public class Road
    {
        /// <summary>
        /// 起始坐标
        /// </summary>
        public Vector3 startingPosition;
        /// <summary>
        /// 到达坐标
        /// </summary>
        public Vector3 arrivelPosition;
        /// <summary>
        /// 起始点名称
        /// </summary>
        public string startingName;
        /// <summary>
        /// 到达点名称
        /// </summary>
        public string arrivelName;
    }
}

