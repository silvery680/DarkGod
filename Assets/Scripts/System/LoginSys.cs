/****************************************************
    File：LoginSys.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:27:55
	Description ：登录系统
*****************************************************/

using UnityEngine;

public class LoginSys : SystemRoot 
{
    public static LoginSys Instance = null;

    public LoginWnd loginWnd;
    public CreateWnd createWnd;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        Debug.Log("Init LoginSystem");
    }

    /// <summary>
    /// 进入登录场景
    /// </summary>
    public void EnterLogin()
    {
        // 异步加载场景
        // 显示加载进度
        // 加载完成再打开登录界面
        ResSvc.Instance.AsyncLoadScene(Constants.SceneLogin, () => {
            loginWnd.SetWndState();
            AudioSvc.Instance.PlayBGMusic(Constants.BGLogin);
        });
    }

    public void RspLogin()
    {
        GameRoot.AddTips("登录成功");

        // 打开角色创建界面
        createWnd.SetWndState();
        // 关闭登录界面
        loginWnd.SetWndState(false);
    }
}