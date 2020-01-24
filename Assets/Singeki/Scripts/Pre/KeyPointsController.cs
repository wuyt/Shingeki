using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Shingeki.Pre
{
    /// <summary>
    /// 预备的关键点控制
    /// </summary>
    public class KeyPointsController : MonoBehaviour
    {

        /// <summary>
        /// 关键点设置UI
        /// </summary>
        private GameObject uiKeyPoints;
        /// <summary>
        /// 返回UI
        /// </summary>
        private GameObject uiBack;
        /// <summary>
        /// 文本框
        /// </summary>
        private Text uiText;
        /// <summary>
        /// 选中的游戏对象
        /// </summary>
        private Transform selected;
        /// <summary>
        /// 滚动视图内容对象
        /// </summary>
        private Transform svContent;
        /// <summary>
        /// 名称输入框
        /// </summary>
        private InputField inputField;
        /// <summary>
        /// 类型下拉列表
        /// </summary>
        private Dropdown dropdown;
        /// <summary>
        /// 按钮预制件
        /// </summary>
        public KeyPointButton prefab;
        /// <summary>
        /// 添加按钮
        /// </summary>
        private Button buttonAdd;
        /// <summary>
        /// 删除按钮
        /// </summary>
        private Button buttonDelete;
        /// <summary>
        /// 公共方法
        /// </summary>
        private Common common;

        void Start()
        {
            //切换界面的初始化
            uiBack = GameObject.Find("/Canvas/ButtonBack");
            uiKeyPoints = GameObject.Find("/Canvas/Panel");
            uiText = uiKeyPoints.transform.Find("Text").GetComponent<Text>();
            uiKeyPoints.transform.Find("ButtonClose").GetComponent<Button>().onClick.AddListener(Close);
            //添加按钮的初始化
            svContent = uiKeyPoints.transform.Find("Scroll View/Viewport/Content");
            inputField = uiKeyPoints.transform.Find("InputField").GetComponent<InputField>();
            dropdown = uiKeyPoints.transform.Find("Dropdown").GetComponent<Dropdown>();
            buttonAdd = uiKeyPoints.transform.Find("ButtonAdd").GetComponent<Button>();
            buttonAdd.onClick.AddListener(AddKeyPoint);
            buttonAdd.interactable = false;
            //删除按钮的初始化
            buttonDelete = uiKeyPoints.transform.Find("ButtonDelete").GetComponent<Button>();
            buttonDelete.onClick.AddListener(DeleteKeyPoint);
            buttonDelete.interactable = false;
            //保存按钮响应初始化
            uiKeyPoints.transform.Find("ButtonSave").GetComponent<Button>().onClick.AddListener(Save);
            common = gameObject.GetComponent<Common>();

            Load();

            uiKeyPoints.SetActive(false);
        }
        void Update()
        {
            TapScreen();
        }

        #region 界面切换

        /// <summary>
        /// 关闭设置UI
        /// </summary>
        private void Close()
        {
            uiKeyPoints.SetActive(false);
            uiBack.SetActive(true);
        }
        /// <summary>
        /// 点击屏幕
        /// </summary>
        private void TapScreen()
        {
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    selected = hit.transform;
                    SelectedObject();
                }
            }
        }
        /// <summary>
        /// 选中对象
        /// </summary>
        private void SelectedObject()
        {
            uiBack.SetActive(false);
            uiKeyPoints.SetActive(true);
            uiText.text = "Position:" + selected.position;

            buttonAdd.interactable = true;
        }

        #endregion

        #region 添加关键点
        /// <summary>
        /// 添加关键点
        /// </summary>
        private void AddKeyPoint()
        {
            if (inputField.text != "")
            {
                //根据预制件生成游戏对象
                KeyPointButton kpb = Instantiate<KeyPointButton>(prefab, svContent);
                //设置关键点对象
                kpb.point.name = inputField.text;
                kpb.point.position = selected.position;
                kpb.point.rotation = selected.rotation;
                kpb.point.pointType = dropdown.value;
                //设置按钮名称
                kpb.GetComponentInChildren<Text>().text = inputField.text;
                //设置其他UI
                inputField.text = "";
                dropdown.value = 0;
                uiText.text = "添加成功。";
                buttonAdd.interactable = false;
            }
        }

        #endregion

        #region 删除关键点

        /// <summary>
        /// 滚动视图中的按钮点击
        /// </summary>
        /// <param name="btnTF">按钮的变体</param>
        public void ButtonClicked(Transform btnTF)
        {
            selected = btnTF;

            KeyPointButton kpb = selected.GetComponent<KeyPointButton>();
            uiText.text = kpb.point.name + ":" + kpb.point.position + ",type:" + kpb.point.pointType;

            buttonAdd.interactable = false;
            buttonDelete.interactable = true;
        }
        /// <summary>
        /// 删除关键点
        /// </summary>
        private void DeleteKeyPoint()
        {
            Destroy(selected.gameObject);
            uiText.text = "删除成功。";
            buttonDelete.interactable = false;
        }

        #endregion

        #region 保存和启动加载

        /// <summary>
        /// 保存关键点
        /// </summary>
        private void Save()
        {
            //遍历滚动视图下的游戏对象，获取JSON字符串数组
            string[] jsons = new string[svContent.childCount];
            for (int i = 0; i < svContent.childCount; i++)
            {
                jsons[i] = JsonUtility.ToJson(svContent.GetChild(i).GetComponent<KeyPointButton>().point);
            }

            common.SaveKeyPoints(jsons);
            uiText.text = "保存完成。";
        }

        /// <summary>
        /// 根据关键点加载按钮
        /// </summary>
        private void Load()
        {
            var list = common.LoadKeyPoints();

            foreach (var item in list)
            {
                var kpb = Instantiate(prefab, svContent);
                kpb.point = JsonUtility.FromJson<KeyPoint>(item);
                kpb.GetComponentInChildren<Text>().text = kpb.point.name;
            }
        }
        #endregion
    }

}

