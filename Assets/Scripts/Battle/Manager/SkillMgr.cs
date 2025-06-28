/********************************************************************
	File: 	SkillMgr.cs
	Author:	groundhog
	Time:	2025/6/19  15:42
	Description: 技能管理器
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Graphs;
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

    public void SkillAttack(EntityBase entity, int skillID)
    {
        SkillCfg skillCfg = resSvc.GetSkillCfg(skillID);

        AttackDamage(entity, skillCfg);
        AttackEffect(entity, skillCfg);
    }

    private void AttackDamage(EntityBase entity, SkillCfg skillCfg)
    {
        List<int> actionLst = skillCfg.skillActionLst;
        int sum = 0;
        for (int i = 0; i < actionLst.Count; i++)
        {
            SkillActionCfg skillAction = resSvc.GetSkillActionCfg(actionLst[i]);
            sum += skillAction.delayTime;
            int index = i;
            if (sum > 0)
            {
                timeSvc.AddTimeTask((int tid) =>
                {
                    SkillAction(entity, skillCfg, index);
                }, sum);
            }
            else
            {
                // 瞬发技能
                SkillAction(entity, skillCfg, index);
            }
        }
    }

    private void SkillAction(EntityBase caster, SkillCfg skillCfg, int index)
    {
        SkillActionCfg skillActionCfg = resSvc.GetSkillActionCfg(skillCfg.skillActionLst[index]); 
        int damage = skillCfg.skillDamageLst[index];

        // 获取场景中怪物实体，遍历运算
        List<EntityMonster> monsterLst = caster.battleMgr.GetEntityMonsters();

        for (int i = 0; i < monsterLst.Count; i++)
        {
            EntityMonster target = monsterLst[i];
            // 判断距离，判断角度
            if (InSkillRange(caster.GetPos(), target.GetPos(), skillActionCfg.radius)
                && InSkillAngle(caster.GetTrans(), target.GetPos(), skillActionCfg.angle))
            {
                // 计算伤害
                CalcDamage(caster, target, skillCfg, damage);
            }
        }
    }

    System.Random rd = new System.Random();
    private void CalcDamage(EntityBase caster, EntityBase target, SkillCfg skillCfg, int damage)
    {
        int dmgSum = damage;
        if (skillCfg.dmgType == DamageType.AD)
        {
            // 计算闪避
            int dodgeNum = PETools.RDInt(1, 100, rd);
            if (dodgeNum <= target.BattleProps.dodge)
            {
                // UI显示闪避 TODO
                PECommon.Log("闪避Rate:" + dodgeNum + "/" + target.BattleProps.dodge);
                return;
            }
            // 计算属性加成
            dmgSum += caster.BattleProps.ad;

            // 计算暴击
            int criticalNum = PETools.RDInt(1, 100, rd);
            if (criticalNum <= caster.BattleProps.critical)
            {
                float criticalRate = 1 + (PETools.RDInt(1, 100, rd) / 100.0f);
                dmgSum = (int)(criticalRate * dmgSum);
                PECommon.Log("暴击Rate:" + criticalRate + "/" + caster.BattleProps.critical);
            }

            // 计算穿甲
            int adddef = (int)((1 - caster.BattleProps.pierce / 100.0f) * target.BattleProps.addef);
            dmgSum -= adddef;
        }
        else if (skillCfg.dmgType == DamageType.AP)
        {
            // 计算属性加成
            dmgSum += caster.BattleProps.ap;
            // 计算魔法抗性
            dmgSum -= target.BattleProps.apdef;
        }
        else
        {

        }

        if (dmgSum < 0) 
        {
            dmgSum = 0;
            return;
        }

        if (target.HP < dmgSum)
        {
            target.HP = 0;
            // 目标死亡 TODO
            target.Die();
        }
        else
        {
            target.HP -= dmgSum;
            target.Hit();
        }
    }

    private bool InSkillRange(Vector3 from, Vector3 to, float range)
    {
        float dis = Vector3.Distance(from, to);
        if (dis <= range)
        {
            return true;
        }
        return false;
    }

    private bool InSkillAngle(Transform trans, Vector3 to, float angle)
    {
        if (angle == 360) return true;
        else
        {
            Vector3 start = trans.forward;
            Vector3 dir = (to - trans.position).normalized;

            float ang = Vector3.Angle(start, dir);

            if (ang <= angle / 2)
            {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 技能效果表现
    /// </summary>
    /// <param name="entity">逻辑实体</param>
    /// <param name="skillID">技能ID</param>
    private void AttackEffect(EntityBase entity, SkillCfg skillCfg)
    {
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

