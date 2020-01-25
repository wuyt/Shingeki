using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shingeki
{
    public class RoadsController : MonoBehaviour
    {
        /// <summary>
        /// 起始点下拉列表
        /// </summary>
        private Dropdown dpdStart;
        /// <summary>
        /// 到达点下拉列表
        /// </summary>
        private Dropdown dpdArrivals;
        /// <summary>
        /// 公共方法
        /// </summary>
        private Common common;
        /// <summary>
        /// 关键点列表
        /// </summary>
        private List<KeyPoint> points;
        /// <summary>
        /// 路径按钮预制件
        /// </summary>
        public RoadButton prefab;
        /// <summary>
        /// 滚动视图内容对象
        /// </summary>
        private Transform svContent;
        /// <summary>
        ///显示文本框
        /// </summary>
        private Text uiText;
        /// <summary>
        /// 删除按钮
        /// </summary>
        private Button buttonDelete;
        /// <summary>
        /// 选中对象
        /// </summary>
        private Transform selected;

        void Start()
        {
            //绑定下拉列表初始化
            dpdStart = GameObject.Find("/Canvas/Panel/dpdStart").GetComponent<Dropdown>();
            dpdArrivals = GameObject.Find("/Canvas/Panel/dpdArrivals").GetComponent<Dropdown>();
            common = GetComponent<Common>();
            points = new List<KeyPoint>();
            BindDropdown();
            //添加路径按钮初始化
            GameObject.Find("/Canvas/Panel/ButtonAdd").GetComponent<Button>().onClick.AddListener(AddRoad);
            svContent = GameObject.Find("/Canvas/Panel/Scroll View/Viewport/Content").transform;
            uiText = GameObject.Find("/Canvas/Panel/Text").GetComponent<Text>();
            //删除按钮的初始化
            buttonDelete = GameObject.Find("/Canvas/Panel/ButtonDelete").GetComponent<Button>();
            buttonDelete.onClick.AddListener(DeleteRoad);
            buttonDelete.interactable = false;
            //保存和加载初始化
            GameObject.Find("/Canvas/Panel/ButtonSave").GetComponent<Button>().onClick.AddListener(Save);
            Load();
        }

        /// <summary>
        /// 绑定下拉列表
        /// </summary>
        private void BindDropdown()
        {
            var list = common.LoadKeyPoints();
            foreach (var item in list)
            {
                KeyPoint point = JsonUtility.FromJson<KeyPoint>(item);
                if (point.pointType != 2)
                {
                    points.Add(point);
                    dpdStart.options.Add(new Dropdown.OptionData(point.name));
                    dpdArrivals.options.Add(new Dropdown.OptionData(point.name));
                }
            }
        }

        #region 添加路径

        /// <summary>
        /// 添加路径
        /// </summary>
        private void AddRoad()
        {
            RoadButton rb = Instantiate(prefab, svContent);

            rb.road.startingName = dpdStart.captionText.text;
            rb.road.startingPosition = GetPointPositonByName(dpdStart.captionText.text);
            rb.road.arrivelName = dpdArrivals.captionText.text;
            rb.road.arrivelPosition = GetPointPositonByName(dpdArrivals.captionText.text);
            rb.GetComponentInChildren<Text>().text = dpdStart.captionText.text + "<==>" + dpdArrivals.captionText.text;

            uiText.text = "添加了路径：" + dpdStart.captionText.text + "<==>" + dpdArrivals.captionText.text;
        }

        /// <summary>
        /// 根据关键点名称返回坐标
        /// </summary>
        /// <param name="name">关键点名称</param>
        /// <returns>坐标</returns>
        private Vector3 GetPointPositonByName(string name)
        {
            foreach (var p in points)
            {
                if (p.name == name)
                {
                    return p.position;
                }
            }
            return Vector3.zero;
        }

        #endregion

        #region 删除路径

        /// <summary>
        /// 滚动视图中的按钮点击
        /// </summary>
        /// <param name="btnTF">按钮的变体</param>
        public void ButtonClicked(Transform btnTF)
        {
            selected = btnTF;
            uiText.text = selected.GetComponentInChildren<Text>().text;

            buttonDelete.interactable = true;
        }
        /// <summary>
        /// 删除路径
        /// </summary>
        private void DeleteRoad()
        {
            Destroy(selected.gameObject);
            uiText.text = "删除成功。";
            buttonDelete.interactable = false;
        }
        #endregion

        #region 保存和启动加载

        /// <summary>
        /// 保存路径
        /// </summary>
        private void Save()
        {
            //遍历滚动视图下的游戏对象，获取JSON字符串数组
            string[] jsons = new string[svContent.childCount];
            for (int i = 0; i < svContent.childCount; i++)
            {
                jsons[i] = JsonUtility.ToJson(svContent.GetChild(i).GetComponent<RoadButton>().road);
            }

            common.SaveRoads(jsons);
            uiText.text = "保存完成。";
        }

        /// <summary>
        /// 根据路径加载按钮
        /// </summary>
        private void Load()
        {
            var list = common.LoadRoads();

            foreach (var item in list)
            {
                var rb = Instantiate(prefab, svContent);
                rb.road = JsonUtility.FromJson<Road>(item);
                rb.GetComponentInChildren<Text>().text = rb.road.startingName + "<==>" + rb.road.arrivelName;
            }
        }
        #endregion
    }
}

