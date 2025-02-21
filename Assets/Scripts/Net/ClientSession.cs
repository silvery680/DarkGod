/****************************************************
    File：ClientSession.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/20 17:53:53
	Description ：客户端网络会话
*****************************************************/

using PENet;
using PEProtocol;
using UnityEngine;

public class ClientSession : PESession<GameMsg> 
{
    protected override void OnConnected()
    {
        GameRoot.AddTips("连接服务器成功!");
        PECommon.Log("Connect To Server Successful");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("RcvPack CMD:" + ((CMD)msg.cmd).ToString());
        NetSvc.Instance.AddNetMsg(msg);
    }

    protected override void OnDisConnected()
    {
        GameRoot.AddTips("服务器断开连接");
        PECommon.Log("DisConnect To Server");
    }
}