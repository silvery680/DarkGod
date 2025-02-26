/****************************************************
    File：SystemRoot.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 22:6:1
	Description ：Nothing
*****************************************************/

using UnityEngine;

public abstract class SystemRoot : MonoBehaviour 
{
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;
    protected NetSvc netSvc;
    protected TimeSvc timeSvc;

    public virtual void InitSys()
    {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
        timeSvc = TimeSvc.Instance;
    }
}