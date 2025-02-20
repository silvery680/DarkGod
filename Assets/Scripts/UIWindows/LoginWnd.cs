/****************************************************
    File：LoginWnd.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 20:22:42
	Description ：登录界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : WindowRoot
{
    public InputField iptAcct;
    public InputField iptPass;
    public Button btnEnter;
    public Button btnNotice;
    protected override void InitWnd()
    {
        base.InitWnd();

        //获取本地存储的账号密码
        if (PlayerPrefs.HasKey("Acct") && PlayerPrefs.HasKey("Pass"))
        {
            iptAcct.text = PlayerPrefs.GetString("Acct");
            iptPass.text = PlayerPrefs.GetString("Pass");
        }
        else
        {
            iptAcct.text = "";
            iptPass.text = "";
        }
    }

    /// <summary>
    /// 点击进入游戏
    /// </summary>
    public void ClickEnterBtn()
    {
        audioSvc.PlayUIAudio(Constants.UILoginBtn);

        string acct = iptAcct.text;
        string pass = iptPass.text;
        if (acct != "" && pass != "")
        {
            // TODO：更新本地存储的账号密码
            PlayerPrefs.SetString("Acct", acct);
            PlayerPrefs.SetString("Pass", pass);

            // TODO: 发送网络消息，请求登录

            // TO Remove
            LoginSys.Instance.RspLogin();
        }
        else
        {
            GameRoot.AddTips("账号或密码为空");
        }
    }

    public void ClickNoticeBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        GameRoot.AddTips("功能正在开发中...");
        
        // TODO:公告栏功能
    }
}