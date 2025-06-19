/********************************************************************
	File: 	StateMgr.cs
	Author:	groundhog
	Time:	2025/6/19  15:42
	Description: 状态管理器
*********************************************************************/

using System.Collections.Generic;
using UnityEngine;
public class StateMgr : MonoBehaviour
{
	private Dictionary<AniState, IState> fsm = new Dictionary<AniState, IState>();

    public void Init()
    {
		fsm.Add(AniState.Idle, new StateIdle());
		fsm.Add(AniState.Move, new StateMove());

		PECommon.Log("Init StateMgr Done.");
    }

	public void ChangeState(EntityBase entity, AniState targetState)
	{
		if (entity.currentAniState == targetState)
		{
			return;
		}
		
		if (fsm.ContainsKey(targetState))
		{
			if (entity.currentAniState != AniState.None)
			{
				fsm[entity.currentAniState].Exit(entity);
			}
			fsm[targetState].Enter(entity);
			fsm[targetState].Process(entity);
		}
	}
}
