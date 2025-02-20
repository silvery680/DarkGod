/****************************************************
    File：ResSvc.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:28:23
	Description ：资源加载服务(单例)
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour
{
    public static ResSvc Instance = null;

    public void InitSvc()
    {
        Instance = this;
        Debug.Log("Init ResSvc...");
    }

    private Action prgCB = null;
    public void AsyncLoadScene(string sceneName, Action loaded)
    {
        GameRoot.Instance.loadingWnd.SetWndState();
        AsyncOperation sceneAsync =  SceneManager.LoadSceneAsync(sceneName);
        sceneAsync.allowSceneActivation = false;

        float lastProgress = 0f;  // 上一帧的进度
        prgCB = () =>
        {
            float targetProgress = sceneAsync.progress / 0.9f;  // Unity的异步加载进度最大值是0.9，接近完成时会变慢
            float smoothProgress = Mathf.Lerp(lastProgress, targetProgress, 0.9f * Time.deltaTime);  // 使用Lerp实现更慢的过渡，每秒最多20%进度

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
                GameRoot.Instance.loadingWnd.SetWndState(false);
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
}