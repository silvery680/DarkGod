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
        });
    }

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
    }
}