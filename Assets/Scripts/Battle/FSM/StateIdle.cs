/********************************************************************
	File: 	StateIdle.cs
	Author:	groundhog
	Time:	2025/6/19  20:51
	Description: 待机状态
*********************************************************************/

using UnityEngine;

class StateIdle : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Idle;
        entity.SetDir(Vector2.zero);
        //PECommon.Log("Idle Enter");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        if (entity.nextSkillId != 0)
        {
            entity.Attack(entity.nextSkillId);
        }
        else
        {
            if (entity.GetDirInput() != Vector2.zero)
            {
                entity.Move();
                entity.SetDir(entity.GetDirInput());
            }
            else
            {
                entity.SetBlend(Constants.BlendIdle);
            }    
        }
        //PECommon.Log("Idle Process");
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        //PECommon.Log("Idle Exit");
    }
}