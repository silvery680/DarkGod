/********************************************************************
	File: 	EntityBase.cs
	Author:	groundhog
	Time:	2025/6/19  20:48
	Description: 实体数据基类
*********************************************************************/

using UnityEngine;

public class EntityBase
{
	public AniState currentAniState = AniState.None;

	public BattleMgr battleMgr = null;
	public StateMgr stateMgr = null;
	public SkillMgr skillMgr = null;
	public Controller controller = null;

	public bool canControl = true;

	public void Move()
	{
		stateMgr.ChangeState(this, AniState.Move, null);
	}

	public void Idle () 
	{
        stateMgr.ChangeState(this, AniState.Idle, null);
    }

    public void Attack(int skillID)
    {
        stateMgr.ChangeState(this, AniState.Attack, skillID);
    }

    public virtual void SetBlend(float blend)
	{
		if (controller != null)
		{
			controller.SetBlend(blend);
		}
	}

	public virtual void SetDir(Vector2 dir)
	{
		if (controller != null)
		{
			controller.Dir = dir;
		}
	}

	public virtual void SetAction(int act)
	{
		if (controller != null)
		{
			controller.SetAction(act);
		}
	}

	public virtual void SetFx(string name, float destory)
	{
		if (controller != null)
		{
			controller.SetFx(name, destory);
		}
	}

    public virtual void SetSkillMoveState(bool move, float speed = 0)
    {
		if (controller != null)
		{
			controller.SetSkillMoveState(move, speed);
		}
    }

    public virtual void AttackEffect(int skillID)
	{
		skillMgr.AttackEffect(this, skillID);
	}

	public virtual Vector2 GetDirInput()
	{
		return Vector2.zero;
	}
}