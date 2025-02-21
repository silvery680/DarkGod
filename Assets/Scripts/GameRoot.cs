/****************************************************
    File：GameRoot.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:25:31
	Description ：游戏启动入口(单例)
*****************************************************/

using PEProtocol;
using UnityEngine;

public class GameRoot : MonoBehaviour 
{
    public static GameRoot Instance = null;

    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        PECommon.Log("Game Start...");

        ClearUIRoot();

        Init();
    }

    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i ++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }

        dynamicWnd.SetWndState();
    }

    private void Init()
    {
        // 服务模块初始化
        NetSvc net = GetComponent<NetSvc>();
        net.InitSvc();
        ResSvc res = GetComponent<ResSvc>();
        res.InitSvc();
        AudioSvc audio = GetComponent<AudioSvc>();
        audio.InitSvc();

        // 业务系统初始化
        LoginSys login = GetComponent<LoginSys>();
        login.InitSys();

        // 进入到登录场景并且加载相应的UI
        login.EnterLogin();
    }

    public static void AddTips(string tips)
    {
        Instance.dynamicWnd.AddTips(tips);
    }

    public static void SetLoadingWndState(bool isActive = true)
    {
        Instance.loadingWnd.SetWndState(isActive);
    }

    private PlayerData playerData = null;
    public PlayerData PlayerData
    {
        get
        {
            return playerData;
        }
    }

    public void SetPlayerData(RspLogin data)
    {
        playerData = data.playerData;
    }
}