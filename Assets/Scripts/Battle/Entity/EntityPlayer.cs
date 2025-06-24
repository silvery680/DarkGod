/********************************************************************
	File: 	EntityPlayer.cs
	Author:	groundhog
	Time:	2025/6/19  21:29
	Description: 玩家逻辑实体
*********************************************************************/

using UnityEngine;

public class EntityPlayer : EntityBase
{
    public override Vector2 GetDirInput()
    {
        return battleMgr.GetDirInput();
    }
}

