/****************************************************
    File：MainCitySys.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:30:1
	Description ：主城业务系统
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.AI;

public class MainCitySys : SystemRoot
{
    public static MainCitySys Instance = null;

    public MainCityWnd mainCityWnd;
    public InfoWnd infoWnd;
    public GuideWnd guideWnd;
    public StrongWnd strongWnd;

    private PlayerController playerCtrl;
    private Transform charCamTrans;
    private AutoGuideCfg curtTaskData;
    private Transform[] npcPosTrans;
    private NavMeshAgent nav;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init MainCitySystem");
    }

    /// <summary>
    /// 进入主城
    /// </summary>
    public void EnterMainCity()
    {
        MapCfg mapData = resSvc.GetMapCfgData(Constants.MainCityMapID);
        resSvc.AsyncLoadScene(mapData.sceneName, () =>
        {
            PECommon.Log("Enter MainCity...");

            // 加载游戏主角
            LoadPlayer(mapData);

            // 打开主城场景UI
            mainCityWnd.SetWndState();

            // 播放主城背景音乐
            audioSvc.PlayBGMusic(Constants.BGMainCity);

            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            MainCityMap mcm = map.GetComponent<MainCityMap>();
            npcPosTrans = mcm.NpcPosTrans;

            // 设置人物展示相机
            if (charCamTrans != null)
            {
                charCamTrans.gameObject.SetActive(false);
            }
        });
    }

    /// <summary>
    /// 加载人物和主摄像机位置信息
    /// </summary>
    /// <param name="mapData">地图配置信息</param>
    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssissnCityPlayerPrefab, true);

        // 主角初始化
        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = new Vector3(1.5F, 1.5F, 1.5F);


        // 相机初始化
        Camera.main.transform.position = mapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mapData.mainCamRote;

        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        nav = player.GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// 设置人物移动方向
    /// </summary>
    /// <param name="dir"></param>
    public void SetMoveDir(Vector2 dir)
    {
        StopNavTask();

        if (dir == Vector2.zero)
        {
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
        else
        {
            playerCtrl.SetBlend(Constants.BlendWalk);
        }

        playerCtrl.Dir = dir;
    }

    #region StrongWnd
    public void OpenStrongWnd()
    {
        strongWnd.SetWndState();
    }

    #endregion

    #region InfoWnd
    /// <summary>
    /// 打开个人信息页
    /// </summary>
    public void OpenInfoWnd()
    {
        StopNavTask();

        if (charCamTrans == null)
        {
            charCamTrans = GameObject.FindGameObjectWithTag("CharShowCam").transform;
        }

        // 设置人物展示相机相对位置
        charCamTrans.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 2.8f + new Vector3(0, 1.2f, 0);
        charCamTrans.localEulerAngles = new Vector3(0, 180 + playerCtrl.transform.localEulerAngles.y, 0);
        charCamTrans.localScale = Vector3.one;
        charCamTrans.gameObject.SetActive(true);
        infoWnd.SetWndState();
    }

    /// <summary>
    /// 关闭跟随相机
    /// </summary>
    public void CloseInfoWnd()
    {
        if (charCamTrans != null)
        {
            charCamTrans.gameObject.SetActive(false);
            infoWnd.SetWndState(false);
        }
    }

    private float startRote = 0;
    /// <summary>
    /// 记录初始旋转角度
    /// </summary>
    public void SetStartRoate()
    {
        startRote = playerCtrl.transform.localEulerAngles.y;
    }

    /// <summary>
    /// 设置角色旋转
    /// </summary>
    /// <param name="rote">旋转角度</param>
    public void SetPlayerRote(float rote)
    {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startRote - rote, 0);
    }
    #endregion

    #region Nav
    private bool isNavGuide = false;
    /// <summary>
    /// 执行引导任务
    /// </summary>
    /// <param name="agc">任务数据</param>
    public void RunTask(AutoGuideCfg agc)
    {
        if (agc != null)
        {
            curtTaskData = agc;
        }

        // 解析任务数据
        nav.enabled = true;
        if (curtTaskData.npcID != -1)
        {
            isNavGuide = true;
            nav.enabled = true;
            nav.speed = Constants.PlayerMoveSpeed;
            nav.SetDestination(npcPosTrans[agc.npcID].position);
            playerCtrl.SetBlend(Constants.BlendWalk);
        }
        else
        {
            OpenGuideWnd();
        }
    }

    /// <summary>
    /// 检查是否到达导航点
    /// </summary>
    private void IsArriveNavPos()
    {
        float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[curtTaskData.npcID].position);

        if (dis < 0.5f)
        {
            StopNavTask();
            OpenGuideWnd();
        }
    }

    private void Update()
    {
        // 导航状态下相机跟随
        if (isNavGuide)
        {
            IsArriveNavPos();
            playerCtrl.SetCam();
        }
    }

    /// <summary>
    /// 停止导航操作
    /// </summary>
    private void StopNavTask()
    {
        if (isNavGuide)
        {
            isNavGuide = false;

            nav.isStopped = true;
            nav.enabled = false;
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
    }

    /// <summary>
    /// 打开引导界面
    /// </summary>
    private void OpenGuideWnd()
    {
        guideWnd.SetWndState();
    } 

    /// <summary>
    /// 获得当前任务的数据
    /// </summary>
    /// <returns></returns>
    public AutoGuideCfg GetCurtTaskData()
    {
        return curtTaskData;
    }

    /// <summary>
    /// 收到服务器的任务引导回应请求
    /// </summary>
    /// <param name="msg">数据包</param>
    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.rspGuide;

        GameRoot.AddTips(Constants.Color("任务奖励 金币 +" + curtTaskData.coin + " 经验 +" + curtTaskData.exp, TxtColor.Blue));

        switch(curtTaskData.actID)
        {
            case 0:
                // 与智者对话
                break;
            case 1:
                // 进入副本
                break;
            case 2:
                // 进入强化界面
                break;
            case 3:
                // 进入体力购买
                break;
            case 4:
                // 进入金币铸造
                break;
            case 5:
                // 进入世界聊天
                break;
        }
        GameRoot.Instance.SetPlayerDataByGuide(data);
        mainCityWnd.RefreshUI();
    }
    #endregion
}