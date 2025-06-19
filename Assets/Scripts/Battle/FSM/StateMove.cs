/********************************************************************
	File: 	StateMove.cs
	Author:	groundhog
	Time:	2025/6/19  20:52
	Description: 移动状态
*********************************************************************/

public class StateMove : IState
{
    public void Enter(EntityBase entity)
    {
        entity.currentAniState = AniState.Move;
        PECommon.Log("Move Enter");
    }

    public void Process(EntityBase entity)
    {
        PECommon.Log("Move Process");
    }

    public void Exit(EntityBase entity)
    {
        PECommon.Log("Move Exit");
    }
}