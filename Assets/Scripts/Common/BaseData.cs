/********************************************************************
	File: 	BaseData.cs
	Author:	groundhog
	Time:	2025/2/22  22:20
	Description: 配置数据类
*********************************************************************/

using System.Collections.Generic;
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
    public int power;
    public Vector3 mainCamPos;
    public Vector3 mainCamRote;
    public Vector3 playerBornPos;
    public Vector3 playerBornRote;
    public List<MonsterData> monsterLst;
}
public class MonsterData : BaseData<MonsterData>
{
    public int mWave; // 批次
    public int mIndex; // 序号
    public MonsterCfg mCfg;
    public Vector3 mBornPos;
    public Vector3 mBornRote;
    public int mLevel;
}

public class MonsterCfg : BaseData<MonsterCfg>
{
    public string mName;
    public string resPath;
    public BattleProps bps;
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

public class SkillCfg : BaseData<SkillCfg>
{
    public string skillName;
    public int skillTime;
    public int aniAction;
    public string fx;
    public DamageType dmgType;
    public List<int> skillMoveLst;
    public List<int> skillActionLst;
    public List<int> skillDamageLst;
}

public class SkillActionCfg : BaseData<SkillActionCfg>
{
    public int delayTime;
    public float radius;
    public int angle;
}


public class SkillMoveCfg : BaseData<SkillMoveCfg>
{
    public int delayTime;
    public int moveTime;
    public float moveDis;
}

public class BaseData<T>
{
    public int ID;
}

public class BattleProps
{
    public int hp;
    public int ad;
    public int ap;
    public int addef;
    public int apdef;
    public int dodge;
    public int pierce;
    public int critical;
}