/****************************************************
    File：StateDie.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/6/28 14:33:38
	Description ：死亡状态
*****************************************************/

using UnityEngine;

public class StateDie : IState 
{
    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Die;
    }

    public void Process(EntityBase entity, params object[] args)
    {
        entity.SetAction(Constants.ActionDie);
        TimeSvc.Instance.AddTimeTask((int tid) =>
        {
            entity.SetActive(false);
        }, Constants.DieAniLength);
    }

    public void Exit(EntityBase entity, params object[] args)
    {
    }
}