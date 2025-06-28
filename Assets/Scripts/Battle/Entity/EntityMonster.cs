/****************************************************
    File：EntityMonster.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/6/24 23:5:25
	Description ：怪物逻辑实体
*****************************************************/

using UnityEngine;

public class EntityMonster : EntityBase 
{
    public MonsterData md;

    public override void SetBattleProps(BattleProps props)
    {
        int level = md.mLevel;

        BattleProps p = new BattleProps()
        {
            hp = props.hp * level,
            ad = props.ad * level,
            ap = props.ap * level,
            addef = props.addef * level,
            apdef = props.apdef * level,
            dodge = props.dodge * level,
            pierce = props.pierce * level,
            critical = props.critical * level,
        };

        BattleProps = p;
        HP = p.hp;


    }
}