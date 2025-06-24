/********************************************************************
	File: 	BattleMgr.cs
	Author:	groundhog
	Time:	2025/6/19  15:36
	Description: 战场管理器
*********************************************************************/

using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class BattleMgr : MonoBehaviour {
    private ResSvc resSvc;
    private AudioSvc audioSvc;

    private StateMgr stateMgr;
    private SkillMgr skillMgr;
    private MapMgr mapMgr;

    private EntityPlayer entitySelfPlayer;

    public void Init(int fbid)
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;

        // 初始化各项管理器
        stateMgr = gameObject.AddComponent<StateMgr>();
        stateMgr.Init();
        skillMgr = gameObject.AddComponent<SkillMgr>();
        skillMgr.Init();

        // 加载战场地图
        MapCfg mapData = resSvc.GetMapCfgData(fbid);
        resSvc.AsyncLoadScene(mapData.sceneName, () =>
        {
            // 初始化地图数据
            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            mapMgr = map.GetComponent<MapMgr>();
            mapMgr.Init();

            map.transform.localPosition = Vector3.zero;
            map.transform.localScale = Vector3.one;

            Camera.main.transform.localPosition = mapData.mainCamPos;
            Camera.main.transform.localEulerAngles = mapData.mainCamRote;

            LoadPlayer(mapData);
            entitySelfPlayer.Idle();

            audioSvc.PlayBGMusic(Constants.BGHuangYe);
        });
        PECommon.Log("Init BattleMgr Done.");
    }

    private void LoadPlayer(MapCfg mapData)
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssissnBattlePlayerPrefab);

        player.transform.position = mapData.playerBornPos;
        player.transform.localEulerAngles = mapData.playerBornRote;
        player.transform.localScale = Vector3.one;

        entitySelfPlayer = new EntityPlayer()
        {
            battleMgr = this,
            stateMgr = stateMgr,
            skillMgr = skillMgr,
        };

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.Init();
        entitySelfPlayer.controller = playerController;
    }

    public void SetSelfPlayerMoveDir(Vector2 moveDir)
    {
        //PECommon.Log(moveDir.ToString());
        if (entitySelfPlayer.canControl == false)
        {
            return;
        }

        if (moveDir == Vector2.zero)
        {
            entitySelfPlayer.Idle();
        }
        else
        {
            entitySelfPlayer.Move();
            entitySelfPlayer.SetDir(moveDir);
        }
    }

    public void ReqReleaseSkill(int index)
    {
        switch(index)
        {
            case 0:
                ReleaseNormalAtk();
                break;
            case 1:
                ReleaseSkill1();
                break;
            case 2:
                ReleaseSkill2();
                break;
            case 3:
                ReleaseSkill3();
                break;
        }
    }

    private void ReleaseNormalAtk()
    {
        PECommon.Log("Click Normal Attack");
    }

    private void ReleaseSkill1()
    {
        PECommon.Log("Click Skill1");
        entitySelfPlayer.Attack(101);
    }

    private void ReleaseSkill2()
    {
        PECommon.Log("Click Skill2");
    }

    private void ReleaseSkill3()
    {
        PECommon.Log("Click Skill3");
    }

    public Vector2 GetDirInput()
    {
        return BattleSys.Instance.GetDirInput();
    }
}
