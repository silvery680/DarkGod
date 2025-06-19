/********************************************************************
	File: 	IState.cs
	Author:	groundhog
	Time:	2025/6/19  20:46
	Description: 状态接口
*********************************************************************/

public interface IState
{
	void Enter(EntityBase entity);

	void Process(EntityBase entity);

	void Exit(EntityBase entity);
}

public enum AniState
{
	None,
	Idle,
	Move,
}