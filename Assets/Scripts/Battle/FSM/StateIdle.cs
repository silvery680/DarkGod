/********************************************************************
	File: 	StateIdle.cs
	Author:	groundhog
	Time:	2025/6/19  20:51
	Description: 待机状态
*********************************************************************/

class StateIdle : IState
{
    public void Enter(EntityBase entity)
    {
        entity.currentAniState = AniState.Idle;
        PECommon.Log("Idle Enter");
    }

    public void Process(EntityBase entity)
    {
        PECommon.Log("Idle Process");
    }

    public void Exit(EntityBase entity)
    {
        PECommon.Log("Idle Exit");
    }
}