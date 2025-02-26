/****************************************************
    File：TaskWnd.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/26 17:36:2
	Description ：任务奖励界面
*****************************************************/

using PEProtocol;
using System.Collections.Generic;
using UnityEngine;

public class TaskWnd : WindowRoot 
{
    private PlayerData pd = null;
    private List<TaskRewardData> trdLst = new List<TaskRewardData>();

    protected override void InitWnd()
    {
        base.InitWnd();

        pd = GameRoot.Instance.PlayerData;
        RefreshUI();
    }

    public void ClickCloseBtn()
    {
        SetWndState(false);
    }

    public void RefreshUI()
    {
        trdLst.Clear();
    }
}