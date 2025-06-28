/********************************************************************
	File: 	IState.cs
	Author:	groundhog
	Time:	2025/6/19  20:46
	Description: 状态接口
*********************************************************************/

public interface IState
{
	void Enter(EntityBase entity, params object[] args);

	void Process(EntityBase entity, params object[] args);

	void Exit(EntityBase entity, params object[] args);
}

public enum AniState
{
	None,
	Idle,
	Move,
	Attack,
	Born,
	Die,
    Hit
}