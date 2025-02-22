/****************************************************
    File：MainCitySys.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:30:1
	Description ：主城业务系统
*****************************************************/

using UnityEngine;

public class MainCitySys : SystemRoot 
{
    public static MainCitySys Instance = null;

    public MainCityWnd mainCityWnd;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init MainCitySystem");
    }

    public void EnterMainCity()
    {
        resSvc.AsyncLoadScene(Constants.SceneMainCity, () =>
        {
            PECommon.Log("Enter MainCity...");

            // TODO 加载游戏主角

            // 打开主城场景UI
            mainCityWnd.SetWndState();

            // 播放主城背景音乐
            audioSvc.PlayBGMusic(Constants.BGMainCity);
        });
    }
}