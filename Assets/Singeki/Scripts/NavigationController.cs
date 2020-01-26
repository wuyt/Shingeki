using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using easyar;

namespace Shingeki
{
    public class NavigationController : MonoBehaviour
    {
        /// <summary>
        /// 导航选择画布
        /// </summary>
        private GameObject canvasNav;
        /// <summary>
        /// 导航根对象
        /// </summary>
        private Transform navRoot;
        /// <summary>
        /// 目标按钮
        /// </summary>
        public ArrivalButton prefabButton;
        /// <summary>
        /// 公共方法
        /// </summary>
        private Common common;
        /// <summary>
        /// 滚动视图内容对象
        /// </summary>
        private Transform svContent;
        /// <summary>
        /// 目的地预制件
        /// </summary>
        public Transform prefabArrival;
        /// <summary>
        /// 路径预制件
        /// </summary>
        public Transform prefabRoad;
        /// <summary>
        /// 导航路线
        /// </summary>
        private LineRenderer line;

        /// <summary>
        /// 导航代理
        /// </summary>
        private NavMeshAgent agent;
        /// <summary>
        /// 导航路径
        /// </summary>
        private NavMeshPath path;
        /// <summary>
        /// 导航（动态更新用）
        /// </summary>
        private NavMeshSurface surface;
        /// <summary>
        /// 导航目的地
        /// </summary>
        private Transform arrival;
        /// <summary>
        /// 玩家
        /// </summary>
        public Transform player;
        /// <summary>
        /// 状态
        /// </summary>
        private int status;
        /// <summary>
        /// 识别图片目标对象
        /// </summary>
        private ImageTargetController imageTarget;
        /// <summary>
        /// 显示导航菜单按钮
        /// </summary>
        private Button buttonNav;
        /// <summary>
        /// 扫描框图片
        /// </summary>
        private GameObject scanFrame;
        /// <summary>
        /// 扫描预制件
        /// </summary>
        public Transform prefabScan;


        void Start()
        {
            SetPrefabs();

            //界面切换初始化
            canvasNav = GameObject.Find("/CanvasNav");
            buttonNav = GameObject.Find("/Canvas/ButtonNav").GetComponent<Button>();
            buttonNav.onClick.AddListener(ShowUINav);
            buttonNav.interactable = false;
            GameObject.Find("/CanvasNav/Panel/ButtonClose").GetComponent<Button>().onClick.AddListener(CloseUINav);
            //加载目标点
            navRoot = GameObject.Find("/NavRoot").transform;
            common = GetComponent<Common>();
            svContent = GameObject.Find("/CanvasNav/Panel/Scroll View/Viewport/Content").transform;

            LoadArrivalsAndScan();
            LoadRoads();
            SetLine();

            status = -1;

            imageTarget = GameObject.Find("ImageTarget").GetComponent<ImageTargetController>();
            imageTarget.TargetFound += OnFound;
            imageTarget.TargetLost += OnLost;
            scanFrame = GameObject.Find("/Canvas/Image");
            scanFrame.SetActive(false);

            CloseUINav();
        }

        #region 界面切换

        /// <summary>
        /// 显示导航UI
        /// </summary>
        private void ShowUINav()
        {
            canvasNav.SetActive(true);
        }
        /// <summary>
        /// 关闭导航UI
        /// </summary>
        private void CloseUINav()
        {
            canvasNav.SetActive(false);
        }
        #endregion

        #region 生成导航空间
        /// <summary>
        /// 加载目标点和扫描点
        /// </summary>
        private void LoadArrivalsAndScan()
        {
            var list = common.LoadKeyPoints();
            foreach (var item in list)
            {
                KeyPoint point = JsonUtility.FromJson<KeyPoint>(item);
                if (point.pointType == 0)
                {
                    //生成目标点
                    var arrival = Instantiate(prefabArrival, navRoot.Find("Arrivals"));
                    arrival.position = point.position;
                    arrival.rotation = point.rotation;
                    //生成按钮
                    var ab = Instantiate(prefabButton, svContent);
                    ab.GetComponentInChildren<Text>().text = point.name;
                    ab.arrival = arrival;
                }
                if (point.pointType == 2)
                {
                    var scan = Instantiate(prefabScan, navRoot);
                    scan.position = point.position;
                    scan.rotation = point.rotation;
                    scan.name = "Scan";
                }
            }
        }
        /// <summary>
        /// 加载路径
        /// </summary>
        private void LoadRoads()
        {
            var list = common.LoadRoads();
            foreach (var item in list)
            {
                Road road = JsonUtility.FromJson<Road>(item);

                var tfRoad = Instantiate(prefabRoad, navRoot.Find("Roads"));
                tfRoad.position = (road.startingPosition + road.arrivelPosition) / 2;
                tfRoad.LookAt(road.arrivelPosition);
                tfRoad.localScale = new Vector3(0.02f, 1f, (road.arrivelPosition - road.startingPosition).magnitude * 0.1f + 0.2f);
            }
        }
        #endregion

        #region 导航
        /// <summary>
        /// 设置路径显示效果
        /// </summary>
        private void SetLine()
        {
            line = navRoot.Find("Line").gameObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.positionCount = 0;
            line.widthMultiplier = 0.1f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] {
                new GradientColorKey(Color.blue, 0.0f),
                new GradientColorKey(Color.blue, 1.0f) },
                new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0.0f),
                new GradientAlphaKey(1f, 1.0f) });
            line.colorGradient = gradient;
        }
        /// <summary>
        /// 烘培路径
        /// </summary>
        private void BakePath()
        {
            surface = FindObjectOfType<NavMeshSurface>();
            agent = FindObjectOfType<NavMeshAgent>();
            agent.enabled = false;
            surface.BuildNavMesh();
            path = new NavMeshPath();
        }

        /// <summary>
        /// 滚动视图中的按钮点击
        /// </summary>
        /// <param name="btnTF">按钮的变体</param>
        public void ButtonClicked(Transform tfArrival)
        {
            //停止重复
            CancelInvoke("DisplayPath");

            arrival = tfArrival;

            //隐藏其他的目的地
            Transform root = navRoot.Find("Arrivals");
            for (int i = 0; i < root.childCount; i++)
            {
                root.GetChild(i).gameObject.SetActive(false);
            }
            arrival.gameObject.SetActive(true);

            //重复开始
            InvokeRepeating("DisplayPath", 0, 0.5f);


            CloseUINav();
        }

        /// <summary>
        /// 显示导航线
        /// </summary>
        private void DisplayPath()
        {
            //将代理移动到当前位置
            agent.transform.position = player.position;
            agent.enabled = true;
            //计算路径
            agent.CalculatePath(arrival.position, path);
            //显示路径
            line.positionCount = path.corners.Length;
            line.SetPositions(path.corners);
            //停止代理
            agent.enabled = false;
        }
        #endregion

        #region 状态改变
        /// <summary>
        /// 发现识别图片
        /// </summary>
        private void OnFound()
        {
            if (status == -1)
            {
                status = 0;
                scanFrame.SetActive(true);
                StartCoroutine("Wait");
            }
        }
        /// <summary>
        /// 识别图片丢失
        /// </summary>
        private void OnLost()
        {
            if (status == 0)
            {
                StopCoroutine("Wait");
                scanFrame.SetActive(false);
                status = -1;
            }
        }
        /// <summary>
        /// 等待
        /// </summary>
        /// <returns></returns>
        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(1f);
            status = 1;
            scanFrame.SetActive(false);
            buttonNav.interactable = true;

            //调整NavRooot和现实对齐
            Transform help = new GameObject().transform;
            Transform scan = navRoot.Find("Scan");
            help.position = scan.localPosition;
            help.eulerAngles = scan.localEulerAngles;
            navRoot.parent = help;
            help.position = imageTarget.transform.position;
            help.eulerAngles = imageTarget.transform.eulerAngles;

            BakePath();

            ShowUINav();
        }


        void OnDestroy()
        {
            imageTarget.TargetFound -= OnFound;
            imageTarget.TargetLost -= OnLost;
        }

        #endregion

        /// <summary>
        /// 设置预制件
        /// </summary>
        private void SetPrefabs()
        {
            if (GameObject.Find("/GameHelper/Canvas") == null)
            {
                prefabRoad.GetComponent<MeshRenderer>().enabled = false;
                prefabScan.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
