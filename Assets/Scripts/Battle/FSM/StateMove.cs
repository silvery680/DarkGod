/********************************************************************
	File: 	StateMove.cs
	Author:	groundhog
	Time:	2025/6/19  20:52
	Description: 移动状态
*********************************************************************/

public class StateMove : IState
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Move;
        //PECommon.Log("Move Enter");
    }

    public void Process(EntityBase entity, params object[] args)
    {
        //PECommon.Log("Move Process");
        entity.SetBlend(Constants.BlendMove);
    }

    public void Exit(EntityBase entity, params object[] args)
    {
        //PECommon.Log("Move Exit");
    }
}