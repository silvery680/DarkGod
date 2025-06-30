/****************************************************
    File：StateHit.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/6/28 14:57:27
	Description ：受击状态
*****************************************************/

using System.Collections;
using UnityEngine;

public class StateHit : IState 
{


    public void Enter(EntityBase entity, params object[] args)
    {
        entity.currentAniState = AniState.Hit;
    }

    public void Process(EntityBase entity, params object[] args)
    {
        entity.SetDir(Vector2.zero);
        entity.SetAction(Constants.ActionHit);

        TimeSvc.Instance.AddTimeTask((int tid1) =>
        {
            TimeSvc.Instance.AddTimeTask((int tid2) =>
            {
                entity.SetAction(Constants.ActionDefault);
                entity.Idle();
            }, (int)(GetHitAniLen(entity) * 1000.0f));
        }, 50);

    }

    public void Exit(EntityBase entity, params object[] args)
    {
    }

    private float GetHitAniLen(EntityBase entity)
    {
        AnimatorStateInfo animatorStateInfo = entity.GetAnimatorStateInfo();
        float remainTime = (1.0f - animatorStateInfo.normalizedTime) * animatorStateInfo.length;
        return remainTime;
        //AnimationClip[] clips = entity.controller.ani.runtimeAnimatorController.animationClips;
        //for (int i = 0; i < clips.Length; i++)
        //{
        //    string clipName = clips[i].name;
        //    if (clipName.Contains("hit") || clipName.Contains("Hit") || clipName.Contains("HIT"))
        //    {
        //        return clips[i].length;
        //    }
        //}
        ////保护值
        // return 1;
    }
}