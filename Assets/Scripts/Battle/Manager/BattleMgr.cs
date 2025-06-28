/********************************************************************
	File: 	BattleMgr.cs
	Author:	groundhog
	Time:	2025/6/19  15:36
	Description: 战场管理器
*********************************************************************/

using PEProtocol;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

public class BattleMgr : MonoBehaviour {
    private ResSvc resSvc;
    private AudioSvc audioSvc;

    private StateMgr stateMgr;
    private SkillMgr skillMgr;
    private MapMgr mapMgr;
    private MapCfg mapCfg;

    private EntityPlayer entitySelfPlayer;

    private Dictionary<string, EntityMonster> monsterDic = new Dictionary<string, EntityMonster>();

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
        mapCfg = resSvc.GetMapCfgData(fbid);
        Debug.Log(fbid);
        resSvc.AsyncLoadScene(mapCfg.sceneName, () =>
        {
            // 初始化地图数据
            GameObject map = GameObject.FindGameObjectWithTag("MapRoot");
            mapMgr = map.GetComponent<MapMgr>();
            mapMgr.Init(this);

            map.transform.localPosition = Vector3.zero;
            map.transform.localScale = Vector3.one;

            Camera.main.transform.localPosition = mapCfg.mainCamPos;
            Camera.main.transform.localEulerAngles = mapCfg.mainCamRote;

            LoadPlayer();
            entitySelfPlayer.Idle();

            // 激活第一批怪物
            ActiveCurrentBatchMonsters();

            audioSvc.PlayBGMusic(Constants.BGHuangYe);
        });
        PECommon.Log("Init BattleMgr Done.");
    }

    private void LoadPlayer()
    {
        GameObject player = resSvc.LoadPrefab(PathDefine.AssissnBattlePlayerPrefab);

        player.transform.position = mapCfg.playerBornPos;
        player.transform.localEulerAngles = mapCfg.playerBornRote;
        player.transform.localScale = Vector3.one;

        PlayerData pd = GameRoot.Instance.PlayerData;
        BattleProps props = new BattleProps
        {
            hp = pd.hp,
            ad = pd.ad,
            ap = pd.ad,
            addef = pd.addef,
            apdef = pd.apdef,
            dodge = pd.dodge,
            pierce = pd.pierce,
            critical = pd.critical,
        };

        entitySelfPlayer = new EntityPlayer()
        {
            battleMgr = this,
            stateMgr = stateMgr,
            skillMgr = skillMgr,
        };
        entitySelfPlayer.SetBattleProps(props);

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.Init();
        entitySelfPlayer.controller = playerController;
    }

    public void LoadMonsterByWaveID(int wave)
    {
        for (int i = 0; i < mapCfg.monsterLst.Count; i++)
        {
            MonsterData md = mapCfg.monsterLst[i];
            if (md.mWave == wave)
            {
                GameObject m = resSvc.LoadPrefab(md.mCfg.resPath, true);
                m.transform.localPosition = md.mBornPos;
                m.transform.localEulerAngles=md.mBornRote;
                m.transform.localScale = Vector3.one;

                m.name = "m" + md.mWave + "_" + md.mIndex;

                EntityMonster em = new EntityMonster
                {
                    battleMgr = this,
                    stateMgr = stateMgr,
                    skillMgr = skillMgr,
                };
                // 设置初始属性
                em.md = md;
                em.SetBattleProps(md.mCfg.bps);

                MonsterController mc = m.GetComponent<MonsterController>();
                mc.Init();
                em.controller = mc;

                monsterDic.Add(m.name, em);
                m.SetActive(false);
            }
        }
    }

    public void ActiveCurrentBatchMonsters()
    {
        TimeSvc.Instance.AddTimeTask((int tid1) =>
        {
            foreach (var item in monsterDic.Values)
            {
                item.controller.gameObject.SetActive(true);
                item.Born();
                // 出生一秒后进Idle
                TimeSvc.Instance.AddTimeTask((int tid2) =>
                {
                    item.Idle();
                }, 1000);
            }
        }, 500);
    }

    public List<EntityMonster> GetEntityMonsters()
    {
        List<EntityMonster> monsterLst = new List<EntityMonster>();
        foreach (var item in monsterDic.Values)
        {
            monsterLst.Add(item);
        }
        return monsterLst;
    }

    #region 技能释放与角色控制
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
        switch (index)
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
    #endregion
}
