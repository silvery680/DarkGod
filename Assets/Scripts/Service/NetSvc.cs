/****************************************************
    File：NetSvc.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 16:29:18
	Description ：网络服务
*****************************************************/

using PENet;
using PEProtocol;
using UnityEngine;

public class NetSvc : MonoBehaviour 
{
    public static NetSvc Instance = null;

    PENet.PESocket<ClientSession, GameMsg> client = null;
    public void InitSvc()
    {
        Instance = this;
        PECommon.Log("Init NetService...");

        client = new PESocket<ClientSession, GameMsg>();

        client.SetLog(true, (string msg, int lv) =>
        {
            switch (lv)
            {
                case 0:
                    msg = "Log:" + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warn:" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error:" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "Info:" + msg;
                    Debug.Log(msg);
                    break;
            }
        });
        client.StartAsClient(SrvCfg.srvIP, SrvCfg.srvPort);
    }

    public void SendMsg(GameMsg msg)
    {
        if (client.session != null)
        {
            client.session.SendMsg(msg);
        }
        else
        {
            GameRoot.AddTips("服务器未连接");
            InitSvc();
        }
    }

    private void Update()
    {

    }
}