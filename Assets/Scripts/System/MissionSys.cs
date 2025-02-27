/****************************************************
    File：MissionSys.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:30:12
	Description ：副本业务系统
*****************************************************/

using UnityEngine;

public class MissionSys : SystemRoot 
{
    public static MissionSys Instance = null;

    public MissionChooseWnd missionChooseWnd;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init MissionSys");
    }

    public void EnterMission()
    {
        OpenMissionChooseWnd();
    }

    public void OpenMissionChooseWnd()
    {
        missionChooseWnd.SetWndState();
    }
}