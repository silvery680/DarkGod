/****************************************************
    File：ResSvc.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:28:23
	Description ：资源加载服务(单例)
*****************************************************/

using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour
{
    public static ResSvc Instance = null;

    public void InitSvc()
    {
        Instance = this;
        InitRDNameCfg(PathDefine.RDNameCfg);
        InitMapCfg(PathDefine.MapCfg);
        InitGuideCfg(PathDefine.GuideCfg);
        InitStrongCfg(PathDefine.StrongCfg);

        PECommon.Log("Init ResSvc...");
    }

    private Action prgCB = null;
    public void AsyncLoadScene(string sceneName, Action loaded)
    {
        GameRoot.SetLoadingWndState();
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);

        float lastProgress = 0f;  // 上一帧的进度
        prgCB = () =>
        {
            float targetProgress = sceneAsync.progress;
            float smoothProgress = Mathf.Lerp(lastProgress, targetProgress, 1f * Time.deltaTime);
            smoothProgress = targetProgress;

            GameRoot.Instance.loadingWnd.SetProgress(smoothProgress);

            // Debug.Log(sceneAsync.progress);
            if (sceneAsync.progress == 1f && smoothProgress >= 0.995f)
            {
                if (loaded != null)
                {
                    loaded();
                }
                sceneAsync = null;
                prgCB = null;
                GameRoot.SetLoadingWndState(false);
            }
            lastProgress = smoothProgress;
        };
    }

    private void Update()
    {
        if (prgCB != null)
        {
            prgCB();
        }
    }

    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    public AudioClip LoadAudio(string path, bool cache = false)
    {
        AudioClip au = null;
        if (!adDic.TryGetValue(path, out au))
        {
            au = Resources.Load<AudioClip>(path);
            if (cache)
            {
                adDic.Add(path, au);
            }
        }
        au = Resources.Load<AudioClip>(path);
        return au;
    }

    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    public GameObject LoadPrefab(string path, bool cache = false)
    {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path, out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (cache)
            {
                goDic.Add(path, prefab);
            }
        }
        GameObject go = null;
        if (prefab != null)
        {
            go = Instantiate(prefab);
        }
        return go;
    }

    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
    public Sprite LoadSprite(string path, bool cache = false)
    {
        Sprite sp = null;
        if (!spDic.TryGetValue(path, out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if (cache)
            {
                spDic.Add(path, sp);
            }
        }
        return sp;
    }

    #region InitCfgs
    #region 随机名字配置
    private List<string> surnameList = new List<string>();
    private List<string> manList = new List<string>();
    private List<string> womanList = new List<string>();

    private void InitRDNameCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml File:" + path + " not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

            foreach (XmlElement ele in nodeList)
            {
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                // int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);

                foreach (XmlElement e in ele.ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "surname":
                            surnameList.Add(e.InnerText);
                            break;
                        case "man":
                            manList.Add(e.InnerText);
                            break;
                        case "woman":
                            womanList.Add(e.InnerText);
                            break;
                    }
                }
            }
        }
    }

    public string GetRDNameData(bool man = true)
    {
        System.Random rd = new System.Random();
        string rdName = surnameList[PETools.RDInt(0, surnameList.Count - 1, rd)];
        if (man)
        {
            rdName += manList[(PETools.RDInt(0, manList.Count - 1, rd))];
        }
        else
        {
            rdName += womanList[(PETools.RDInt(0, womanList.Count - 1, rd))];
        }

        return rdName;
    }
    #endregion

    #region 地图配置
    private Dictionary<int, MapCfg> mapCfgDataDic = new Dictionary<int, MapCfg>();
    private void InitMapCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml File:" + path + " not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

            foreach (XmlElement ele in nodeList)
            {
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int _ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                MapCfg mc = new MapCfg()
                {
                    ID = _ID
                };


                foreach (XmlElement e in ele.ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mapName":
                            mc.mapName = e.InnerText;
                            break;
                        case "sceneName":
                            mc.sceneName = e.InnerText;
                            break;
                        case "mainCamPos":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.mainCamPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "mainCamRote":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.mainCamRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "playerBornPos":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.playerBornPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "playerBornRote":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.playerBornRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                    }
                }

                mapCfgDataDic.Add(_ID, mc);
            }
        }
    }

    public MapCfg GetMapCfgData(int id)
    {
        MapCfg data = null;
        if (mapCfgDataDic.TryGetValue(id, out data))
        {
            return data;
        }
        return null;
    }
    #endregion

    #region 自动引导配置
    private Dictionary<int, AutoGuideCfg> autoGuideCfgDataDic = new Dictionary<int, AutoGuideCfg>();
    private void InitGuideCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml File:" + path + " not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

            foreach (XmlElement ele in nodeList)
            {
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int _ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                AutoGuideCfg agc = new AutoGuideCfg()
                {
                    ID = _ID
                };


                foreach (XmlElement e in ele.ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "npcID":
                            {
                                agc.npcID = int.Parse(e.InnerText);
                            }
                            break;
                        case "dilogArr":
                            {
                                agc.dialogArr = e.InnerText;
                            }
                            break;
                        case "actID":
                            {
                                agc.actID = int.Parse(e.InnerText);
                            }
                            break;
                        case "coin":
                            {
                                agc.coin = int.Parse(e.InnerText);
                            }
                            break;
                        case "exp":
                            {
                                agc.exp = int.Parse(e.InnerText);
                            }
                            break;
                    }
                }

                autoGuideCfgDataDic.Add(_ID, agc);
            }
        }
    }
    public AutoGuideCfg GetAutoGuideData(int id)
    {
        AutoGuideCfg data = null;
        if (autoGuideCfgDataDic.TryGetValue(id, out data))
        {
            return data;
        }
        return null;
    }
    #endregion

    #region 强化配置
    // 位置 + 星级
    private Dictionary<int, Dictionary<int, StrongCfg>> strongDataDic = new Dictionary<int, Dictionary<int, StrongCfg>>();
    private void InitStrongCfg(string path)
    {
        TextAsset xml = Resources.Load<TextAsset>(path);
        if (!xml)
        {
            PECommon.Log("xml File:" + path + " not exist", LogType.Error);
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml.text);

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;

            foreach (XmlElement ele in nodeList)
            {
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }

                int _ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                StrongCfg sc = new StrongCfg()
                {
                    ID = _ID
                };


                foreach (XmlElement e in ele.ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "pos":
                            {
                                sc.pos = int.Parse(e.InnerText);
                            }
                            break;
                        case "starlv":
                            {
                                sc.starLv = int.Parse(e.InnerText);
                            }
                            break;
                        case "addhp":
                            {
                                sc.addHp = int.Parse(e.InnerText);
                            }
                            break;
                        case "addhurt":
                            {
                                sc.addHurt = int.Parse(e.InnerText);
                            }
                            break;
                        case "adddef":
                            {
                                sc.addDef = int.Parse(e.InnerText);
                            }
                            break;
                        case "minlv":
                            {
                                sc.minLv = int.Parse(e.InnerText);
                            }
                            break;
                        case "coin":
                            {
                                sc.coin = int.Parse(e.InnerText);
                            }
                            break;
                        case "crystal":
                            {
                                sc.crystal = int.Parse(e.InnerText);
                            }
                            break;
                    }
                }

                Dictionary<int, StrongCfg> dic = null;
                if (strongDataDic.TryGetValue(sc.pos, out dic))
                {
                    dic.Add(sc.starLv, sc);
                }
                else
                {
                    dic = new Dictionary<int, StrongCfg>();
                    dic.Add(sc.starLv, sc);

                    strongDataDic.Add(sc.pos, dic);
                }
            }

        }
    }
    public StrongCfg GetStrongData(int id, int starLv)
    {
        StrongCfg sc = null;
        Dictionary<int, StrongCfg> dic = null;
        if (strongDataDic.TryGetValue(id, out dic))
        {
            if (dic.ContainsKey(starLv))
            {
                sc = dic[starLv];
            }
        }
        return sc;
    }

    public int GetPropAddValPreLv(int pos, int starlv, int type)
    {
        Dictionary<int, StrongCfg> posDic = null;
        int val = 0;
        if (strongDataDic.TryGetValue(pos, out posDic))
        {
            for (int i = 1; i <= starlv; i ++)
            {
                StrongCfg sc = null;
                if (posDic.TryGetValue(i, out sc))
                {
                    switch(type)
                    {
                        case 1://hp
                            val += sc.addHp;
                            break;
                        case 2://hurt
                            val += sc.addHurt;
                            break;
                        case 3://def
                            val += sc.addDef;
                            break;
                    }
                }
            }
        }
        return val;
    }
    #endregion

    #endregion
}