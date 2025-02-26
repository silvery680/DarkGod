/********************************************************************
	File: 	BaseData.cs
	Author:	groundhog
	Time:	2025/2/22  22:20
	Description: 配置数据类
*********************************************************************/

using UnityEngine;

public class StrongCfg : BaseData<StrongCfg>
{
    public int pos;
    public int starLv;
    public int addHp;
    public int addHurt;
    public int addDef;
    public int minLv;
    public int coin;
    public int crystal;
}

public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
    public int npcID; // 触发任务的NPC索引号
    public string dialogArr; // 对话数组
    public int actID; // 完成这个引导之后做什么
    public int coin;
    public int exp;
}

public class MapCfg : BaseData<MapCfg>
{
    public string mapName;
    public string sceneName;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;
}

public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
    public string taskName;
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData :BaseData<TaskRewardData>
{
    public int prgs;
    public bool taked;
}

public class BaseData<T>
{
    public int ID;
}
