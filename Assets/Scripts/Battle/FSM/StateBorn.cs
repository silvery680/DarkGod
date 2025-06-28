/****************************************************
    File：StateBorn.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/6/28 14:28:15
	Description ：出生状态
*****************************************************/

using UnityEngine;

public class StateBorn : IState 
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Born;
    }

    public void Process(EntityBase entity, params object[] args)
    {
        // 播放出生动画
        entity.SetAction(Constants.ActionBorn);
        TimeSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.SetAction(Constants.ActionDefault);
        }, 500);
    }

    public void Exit(EntityBase entity, params object[] args)
    {
    }
}