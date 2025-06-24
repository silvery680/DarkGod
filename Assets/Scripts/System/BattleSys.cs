/****************************************************
    File：BattleSys.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:30:24
	Description ：战斗系统
*****************************************************/

using UnityEngine;

public class BattleSys : SystemRoot 
{
    public static BattleSys Instance = null;
    public PlayCtrlWnd playCtrlWnd;

    public BattleMgr battleMgr;

    public override void InitSys()
    {
        base.InitSys();

        Instance = this;
        PECommon.Log("Init BattleSystem");
    }

    public void StartBattle(int fbid)
    {
        GameObject go = new GameObject
        {
            name = "BattleRoot"
        };

        go.transform.SetParent(GameRoot.Instance.transform);
        battleMgr = go.AddComponent<BattleMgr>();

        battleMgr.Init(fbid);
        SetPlayerCtrlWndState();
    }

    public void SetPlayerCtrlWndState(bool isActive = true)
    {
        playCtrlWnd.SetWndState(isActive);
    }

    public void SetMoveDir(Vector2 moveDir)
    {
        battleMgr.SetSelfPlayerMoveDir(moveDir);
    }

    public void ReqReleaseSkill(int index)
    {
        battleMgr.ReqReleaseSkill(index);
    }

    public Vector2 GetDirInput()
    {
        return playCtrlWnd.currentDir;
    }
}