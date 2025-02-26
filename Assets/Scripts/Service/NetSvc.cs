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
using System.Collections.Generic;

public class NetSvc : MonoBehaviour
{
    public static NetSvc Instance = null;

    PENet.PESocket<ClientSession, GameMsg> client = null;
    private Queue<GameMsg> msgQue = new Queue<GameMsg>();
    private static readonly string obj = "lock";

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

    public void AddNetMsg(GameMsg msg)
    {
        lock (obj)
        {
            msgQue.Enqueue(msg);
        }
    }

    private void Update()
    {
        if (msgQue.Count > 0)
        {
            lock (obj)
            {
                GameMsg msg = msgQue.Dequeue();
                HandOnMsg(msg);
            }
        }
    }

    private void HandOnMsg(GameMsg msg)
    {
        if ((ErrorCode)msg.err != ErrorCode.None)
        {
            switch ((ErrorCode)msg.err)
            {
                case ErrorCode.LackDiamond:
                    GameRoot.AddTips("客户端水晶数据异常");
                    PECommon.Log("客户端与服务端水晶数据不一致", LogType.Error);
                    break;
                case ErrorCode.LackLevel:
                    GameRoot.AddTips("客户端等级数据异常");
                    PECommon.Log("客户端与服务端等级数据不一致", LogType.Error);
                    break;
                case ErrorCode.LackCoin:
                    GameRoot.AddTips("客户端金币数据异常");
                    PECommon.Log("客户端与服务端金币数据不一致", LogType.Error);
                    break;
                case ErrorCode.LackCrystal:
                    GameRoot.AddTips("客户端水晶数据异常");
                    PECommon.Log("客户端与服务端水晶数据不一致", LogType.Error);
                    break;
                case ErrorCode.SeverDataError:
                    GameRoot.AddTips("客户端数据异常");
                    PECommon.Log("客户端与服务端数据不一致", LogType.Error);
                    break;
                case ErrorCode.UpdateDBError:
                    GameRoot.AddTips("网络不稳定");
                    PECommon.Log("数据库更新错误", LogType.Error);
                    break;
                case ErrorCode.AcctIsOnline:
                    GameRoot.AddTips("当前账号已经上线");
                    break;
                case ErrorCode.WrongPass:
                    GameRoot.AddTips("密码错误");
                    break;
            };
            return;
        }

        switch ((CMD)msg.cmd)
        {
            case CMD.RspLogin:
                LoginSys.Instance.RspLogin(msg);
                break;
            case CMD.RspRename:
                LoginSys.Instance.RspRename(msg);
                break;
            case CMD.RspGuide:
                MainCitySys.Instance.RspGuide(msg);
                break;
            case CMD.RspStrong:
                MainCitySys.Instance.RspStrong(msg);
                break;
            case CMD.PshChat:
                MainCitySys.Instance.PshChat(msg);
                break;
            case CMD.RspBuy:
                MainCitySys.Instance.RspBuy(msg);
                break;
            case CMD.PshPower:
                MainCitySys.Instance.PshPower(msg);
                break;
        }
    }
}