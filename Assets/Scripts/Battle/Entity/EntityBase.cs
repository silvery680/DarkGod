/********************************************************************
	File: 	EntityBase.cs
	Author:	groundhog
	Time:	2025/6/19  20:48
	Description: 实体数据基类
*********************************************************************/

public class EntityBase
{
	public AniState currentAniState = AniState.None;

	public StateMgr stateMgr = null;

	public Controller controller = null;

	public void Move()
	{
		stateMgr.ChangeState(this, AniState.Move);
	}

	public void Idle () 
	{
        stateMgr.ChangeState(this, AniState.Idle);
    }
}