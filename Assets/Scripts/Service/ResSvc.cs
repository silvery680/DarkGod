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
        InitRDNameCfg();

        PECommon.Log("Init ResSvc...");
    }

    private Action prgCB = null;
    public void AsyncLoadScene(string sceneName, Action loaded)
    {
        GameRoot.SetLoadingWndState();
        AsyncOperation sceneAsync =  SceneManager.LoadSceneAsync(sceneName);
        sceneAsync.allowSceneActivation = false;

        float lastProgress = 0f;  // 上一帧的进度
        prgCB = () =>
        {
            float targetProgress = sceneAsync.progress / 0.9f;  // Unity的异步加载进度最大值是0.9，接近完成时会变慢
            float smoothProgress = Mathf.Lerp(lastProgress, targetProgress, 1f * Time.deltaTime);  // 使用Lerp实现更慢的过渡，每秒最多20%进度

            GameRoot.Instance.loadingWnd.SetProgress(smoothProgress);

            if (smoothProgress >= 0.99)
            {
                if (loaded != null)
                {
                    loaded();
                }
                sceneAsync.allowSceneActivation = true;
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

    #region InitCfgs
    private List<string> surnameList = new List<string>();
    private List<string> manList = new List<string>();
    private List<string> womanList = new List<string>();

    private void InitRDNameCfg()
    {
        TextAsset xml = Resources.Load<TextAsset>(PathDefine.RDNameCfg);
        if (!xml)
        {
            PECommon.Log("xml File:" + PathDefine.RDNameCfg + " not exist", LogType.Error);
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
                    switch(e.Name)
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
}