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
        PECommon.Log("Server Connect");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("Server Response:");
    }

    protected override void OnDisConnected()
    {
        PECommon.Log("Server DisConnect");
    }
}