/****************************************************
    File：GameRoot.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:25:31
	Description ：游戏启动入口(单例)
*****************************************************/

using UnityEngine;

public class GameRoot : MonoBehaviour 
{
    public static GameRoot Instance = null;
    public LoadingWnd loadingWnd;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        Debug.Log("Game Start...");

        Init();
    }

    private void Init()
    {
        // 服务模块初始化
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
}