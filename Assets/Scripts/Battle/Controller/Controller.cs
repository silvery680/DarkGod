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
    public Transform hpRoot;

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

    protected Transform camTrans;

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

    public virtual void SetAtkRotationLocal(Vector2 atkDir)
    {
        float angle = Vector2.SignedAngle(atkDir, new Vector2(0, 1));
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }

    public virtual void SetAtkRotationCam(Vector2 atkDir)
    {
        float angle = Vector2.SignedAngle(atkDir, new Vector2(0, 1)) + camTrans.eulerAngles.y;
        Vector3 eulerAngles = new Vector3(0, angle, 0);
        transform.localEulerAngles = eulerAngles;
    }
}