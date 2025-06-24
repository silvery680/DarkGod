/********************************************************************
	File: 	SkillMgr.cs
	Author:	groundhog
	Time:	2025/6/19  15:42
	Description: 技能管理器
*********************************************************************/

using System.Collections.Generic;
using UnityEngine;

public class SkillMgr : MonoBehaviour
{
    private ResSvc resSvc;
    private TimeSvc timeSvc;

    public void Init()
    {
        resSvc = ResSvc.Instance;
        timeSvc = TimeSvc.Instance;
        PECommon.Log("Init SkillMgr Done.");
    }

    /// <summary>
    /// 技能效果表现
    /// </summary>
    /// <param name="entity">逻辑实体</param>
    /// <param name="skillID">技能ID</param>
    public void AttackEffect(EntityBase entity, int skillID)
    {
        SkillCfg skillCfg = resSvc.GetSkillCfg(skillID);

        entity.SetAction(skillCfg.aniAction);
        entity.SetFx(skillCfg.fx, skillCfg.skillTime);

        CalcSkillMove(entity, skillCfg);

        entity.canControl = false;
        entity.SetDir(Vector2.zero);

        timeSvc.AddTimeTask((int tid) =>
        {
            entity.Idle();
        }, skillCfg.skillTime);
    }

    private void CalcSkillMove(EntityBase entity, SkillCfg skillData)
    {
        int sumSkillMoveTime = 0;
        List<int> skillMoveLst = skillData.skillMoveLst;
        for (int i = 0; i < skillMoveLst.Count; i++)
        {
            SkillMoveCfg skillMoveCfg = resSvc.GetSkillMoveCfg(skillData.skillMoveLst[i]);
            float speed = skillMoveCfg.moveDis / (skillMoveCfg.moveTime / 1000f);
            sumSkillMoveTime += skillMoveCfg.delayTime;

            if (sumSkillMoveTime > 0)
            {
                timeSvc.AddTimeTask((int tid) =>
                {
                    entity.SetSkillMoveState(true, speed);
                }, sumSkillMoveTime);
            }
            else
            {
                entity.SetSkillMoveState(true, speed);
            }

            sumSkillMoveTime += skillMoveCfg.moveTime;
            timeSvc.AddTimeTask((int tid) =>
            {
                entity.SetSkillMoveState(false);
            }, sumSkillMoveTime);
        }
    }
}

