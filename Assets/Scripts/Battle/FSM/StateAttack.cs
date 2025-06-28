/****************************************************
    File：StateAttack.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/6/22 17:20:42
	Description ：攻击状态
*****************************************************/

using UnityEngine;

public class StateAttack : IState 
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Attack;
        PECommon.Log("Enter StateAttack.");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        PECommon.Log("Process StateAttack.");
        // 技能效果表现和伤害运算
        entity.SkillAttack((int)args[0]);
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        PECommon.Log("Exit StateAttack.");
        entity.canControl = true;
        entity.SetAction(Constants.ActionDefault);
    }
}