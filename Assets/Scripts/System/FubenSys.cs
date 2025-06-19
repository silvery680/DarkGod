/****************************************************
    File：FubenSys.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:30:12
	Description ：副本业务系统
*****************************************************/

using PEProtocol;
using UnityEngine;

public class FubenSys : SystemRoot 
{
    public static FubenSys Instance = null;

    public FubenChooseWnd fubenChooseWnd;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init FubenSys");
    }

    public void EnterFuben()
    {
        SetFubenWndState();
    }

    public void SetFubenWndState(bool isActive = true)
    {
        fubenChooseWnd.SetWndState(isActive);
    }

    public void RspFBFight(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerDataByFBStart(msg.rspFBFight);
        MainCitySys.Instance.mainCityWnd.SetWndState(false);
        SetFubenWndState(false);
        BattleSys.Instance.StartBattle(msg.rspFBFight.fbid);
    }
}