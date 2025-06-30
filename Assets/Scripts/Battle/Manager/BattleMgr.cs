/********************************************************************
	File: 	BattleMgr.cs
	Author:	groundhog
	Time:	2025/6/19  15:36
	Description: 战场管理器
*********************************************************************/

using PEProtocol;
using System.Collections.Generic;
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
        entitySelfPlayer.Name = "AssassinBattle";
        entitySelfPlayer.SetBattleProps(props);

        PlayerController playerController = player.GetComponent<PlayerController>();
        playerController.Init();
        entitySelfPlayer.SetCtrl(playerController);
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
                em.Name = m.name;

                MonsterController mc = m.GetComponent<MonsterController>();
                mc.Init();
                em.SetCtrl(mc);

                m.SetActive(false);
                monsterDic.Add(m.name, em);
                GameRoot.Instance.dynamicWnd.AddHpItemInfo(m.name, mc.hpRoot, em.HP);
            }
        }
    }

    public void ActiveCurrentBatchMonsters()
    {
        TimeSvc.Instance.AddTimeTask((int tid1) =>
        {
            foreach (var item in monsterDic.Values)
            {
                item.SetActive(true);
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

    public void RemoveMonster(string key)
    {
        EntityMonster entityMonster;
        if (monsterDic.TryGetValue(key, out entityMonster))
        {
            monsterDic.Remove(key);
            GameRoot.Instance.dynamicWnd.RemoveHpItemInfo(key);
        }
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

    public double lastAtkTime = 0;
    private int[] comboArr = new int[]
    {
        111, 112, 113, 114, 115,
    };
    private int comboIndex = 0;
    private void ReleaseNormalAtk()
    {
        //PECommon.Log("Click Normal Attack");
        if (entitySelfPlayer.currentAniState == AniState.Attack)
        {
            // 在500ms以内进行第二次点击，存数据
            double nowAtkTime = TimeSvc.Instance.GetNowTime();
            if (nowAtkTime - lastAtkTime < Constants.ComboSpace && lastAtkTime != 0)
            {
                if (comboArr[comboIndex] != comboArr[comboArr.Length - 1])
                {
                    comboIndex += 1;
                    entitySelfPlayer.comboQue.Enqueue(comboArr[comboIndex]);
                    lastAtkTime = nowAtkTime;
                }
            }
            else
            {
                lastAtkTime = 0;
                comboIndex = 0;
            }
        }
        else if (entitySelfPlayer.currentAniState == AniState.Idle || entitySelfPlayer.currentAniState == AniState.Move)
        {
            comboIndex = 0;
            lastAtkTime = TimeSvc.Instance.GetNowTime();
            entitySelfPlayer.Attack(comboArr[comboIndex]);
        }
    }

    private void ReleaseSkill1()
    {
        //PECommon.Log("Click Skill1");
        entitySelfPlayer.Attack(101);
    }

    private void ReleaseSkill2()
    {
        //PECommon.Log("Click Skill2");
        entitySelfPlayer.Attack(102);
    }

    private void ReleaseSkill3()
    {
        //PECommon.Log("Click Skill3");
        entitySelfPlayer.Attack(103);
    }

    public Vector2 GetDirInput()
    {
        return BattleSys.Instance.GetDirInput();
    } 
    #endregion
}
