/********************************************************************
	File: 	Controller.cs
	Author:	groundhog
	Time:	2025/6/19  21:34
	Description: 表现实体控制器抽象基类
*********************************************************************/

using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
	public Animator ani;
    public CharacterController ctrl;

    protected bool isMove = false;
    private Vector2 dir = Vector2.zero;
    public Vector2 Dir
    {
        get
        {
            return dir;
        }

        set
        {
            if (value == Vector2.zero)
            {
                isMove = false;
            }
            else
            {
                isMove = true;
            }
            dir = value;
        }
    }

    protected bool skillMove = false;
    protected float skillMoveSpeed = 0f;
    protected TimeSvc timerSvc;
    protected Dictionary<string, GameObject> fxDic = new Dictionary<string, GameObject>();

    public virtual void Init()
    {
        timerSvc = TimeSvc.Instance;
    }

    public virtual void SetBlend(float blend)
    {
        ani.SetFloat("Blend", blend);
    }

    public virtual void SetAction(int action)
    {
        ani.SetInteger("Action", action);
    }

    public virtual void SetFx(string name, float destory)
    {

    }

    public void SetSkillMoveState(bool move, float skillSpeed = 0f)
    {
        skillMove = move;
        skillMoveSpeed = skillSpeed;
    }
}