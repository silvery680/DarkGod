/********************************************************************
	File: 	PETools.cs
	Author:	groundhog
	Time:	20/2/2025   14:20
	Description: 工具类
*********************************************************************/
using UnityEngine;
using System.Collections;

public class PETools
{
    /// <summary>
    /// 返回一个随机整数
    /// </summary>
    /// <param name="min">最小值（包含）</param>
    /// <param name="max">最大值（包含）</param>
    /// <param name="rd">随机数类</param>
    /// <returns></returns>
    public static int RDInt(int min, int max, System.Random rd = null)
    {
        if (rd == null)
        {
            rd = new System.Random();
        }
        int val = rd.Next(min, max + 1);
        return val;
    }

}
