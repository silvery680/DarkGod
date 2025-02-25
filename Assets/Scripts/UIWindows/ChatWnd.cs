/****************************************************
    File：ChatWnd.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/25 15:1:4
	Description ：聊天界面
*****************************************************/

using PEProtocol;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatWnd : WindowRoot 
{
    public Image imgWorld;
    public Image imgGuild;
    public Image imgFriend;

    public Text txtChat;
    public InputField iptChat;

    // 0-world 1-guild 2-friend
    private int chatType;
    private List<string> chatList = new List<string>();

    protected override void InitWnd()
    {
        base.InitWnd();

        chatType = 0;
        RefreshUI();
    }

    public void AddChatMsg(string name, string chat)
    {
        chatList.Add(Constants.Color(name + " : ", TxtColor.Blue) + chat);
        if (chatList.Count > 12)
        {
            chatList.RemoveAt(0);
        }
        if (GetWndState())
        {
            RefreshUI();
        }
    }

    private void RefreshUI()
    {
        if (chatType == 0)
        {
            string chatMsg = "";
            for (int i = 0; i < chatList.Count; i ++)
            {
                chatMsg += chatList[i] + "\n";
            }
            SetText(txtChat, chatMsg);

            SetSprite(imgWorld, PathDefine.SelectedChatBtn);
            SetSprite(imgGuild, PathDefine.UnselectedChatBtn);
            SetSprite(imgFriend, PathDefine.UnselectedChatBtn);
        }
        else if (chatType == 1)
        {
            SetText(txtChat, "尚未加入工会");
            SetSprite(imgWorld, PathDefine.UnselectedChatBtn);
            SetSprite(imgGuild, PathDefine.SelectedChatBtn);
            SetSprite(imgFriend, PathDefine.UnselectedChatBtn);
        }
        else if (chatType == 2)
        {
            SetText(txtChat, "暂无好友信息");
            SetSprite(imgWorld, PathDefine.UnselectedChatBtn);
            SetSprite(imgGuild, PathDefine.UnselectedChatBtn);
            SetSprite(imgFriend, PathDefine.SelectedChatBtn);
        }
    }

    public void ClickSendBtn()
    {
        if (iptChat.text != null && iptChat.text != "" && iptChat.text != " ")
        {
            if (iptChat.text.Length > 12)
            {
                GameRoot.AddTips("输入的信息不能超过12个字");
            }
            else
            {
                // 发送网络消息到服务器
                GameMsg msg = new GameMsg
                {
                    cmd = (int)CMD.SndChat,
                    sndChat = new SndChat
                    {
                        chat = iptChat.text,
                    }
                };
                iptChat.text = "";
                netSvc.SendMsg(msg);
            }
        }

    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        SetWndState(false);
    }

    public void ClickWorldBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 0;
        RefreshUI();
    }

    public void ClickGuildBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 1;
        RefreshUI();
    }

    public void ClickFriendBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        chatType = 2;
        RefreshUI();
    }
}