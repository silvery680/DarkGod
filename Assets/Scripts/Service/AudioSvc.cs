/****************************************************
    File：AudioSvc.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:28:51
	Description ：声音播放服务
*****************************************************/

using UnityEngine;

public class AudioSvc : MonoBehaviour 
{
    public static AudioSvc Instance = null;
    public AudioSource bgAudio;
    public AudioSource uiAudio;

    public void InitSvc()
    {
        Instance = this;
        Debug.Log("Init AudioService...");
    }

    public void PlayBGMusic(string name, bool isLoop = true)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        if (bgAudio.clip == null || bgAudio.clip.name != audio.name)
        {
            bgAudio.clip = audio;
            bgAudio.loop = isLoop;
            bgAudio.Play();
        }
    }

    public void PlayUIAudio(string name)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        uiAudio.clip = audio;
        uiAudio.Play();
    }
}