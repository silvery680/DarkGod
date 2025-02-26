/****************************************************
    File：TimeSvc.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:28:59
	Description ：计时服务
*****************************************************/

using System;
using UnityEngine;

public class TimeSvc : SystemRoot 
{
    public static TimeSvc Instance = null;

    private PETimer pt;

    public void InitSvc()
    {
        Instance = this;
        pt = new PETimer();
        // 设置计时器工具日志
        pt.SetLog((string info) =>
        {
            PECommon.Log(info);
        });
        PECommon.Log("Init TimeSvc...");
    }

    public void Update()
    {
        pt.Update();
    }

    public int AddTimeTask(Action<int> cb, double delay,
        PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        return pt.AddTimeTask(cb, delay, timeUnit, count);
    }
}