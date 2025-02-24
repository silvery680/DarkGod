/****************************************************
    File：GuideWnd.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/24 9:52:40
	Description ：引导对话界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class GuideWnd : WindowRoot 
{
    public Text txtName;
    public Text txtContent;
    public Image imgIcon;

    private PlayerData pd;
    private AutoGuideCfg curTaskData;
    private string[] dialogArr;
    // 对话索引
    private int dialogInd;

    protected override void InitWnd()
    {
        base.InitWnd();

        pd = GameRoot.Instance.PlayerData;
        curTaskData = MainCitySys.Instance.GetCurtTaskData();
        dialogArr = curTaskData.dialogArr.Split('#');
        dialogInd = 1;

        SetTalk();
    }

    /// <summary>
    /// 设置对话内容
    /// </summary>
    public void SetTalk()
    {
        string[] talkArr = dialogArr[dialogInd].Split('|');
        if (talkArr[0] == "0")
        {
            // 自己
            SetSprite(imgIcon, PathDefine.SelfIcon);
            SetText(txtName, pd.name);
        }
        else
        {
            // NPC
            switch (curTaskData.npcID)
            {
                case Constants.NPCWiseManID:
                    {
                        SetSprite(imgIcon, PathDefine.WiseManIcon);
                        SetText(txtName, "智者");
                    }
                    break;
                case Constants.NPCGeneralID:
                    {
                        SetSprite(imgIcon, PathDefine.GeneralIcon);
                        SetText(txtName, "将军");
                    }
                    break;
                case Constants.NPCArtisanID:
                    {
                        SetSprite(imgIcon, PathDefine.ArtisanIcon);
                        SetText(txtName, "工匠");
                    }
                    break;
                case Constants.NPCTraderID:
                    {
                        SetSprite(imgIcon, PathDefine.TraderIcon);
                        SetText(txtName, "商人");
                    }
                    break;
                default:
                    {
                        SetSprite(imgIcon, PathDefine.GuideIcon);
                        SetText(txtName, "小芸");
                    }
                    break;
            }
        }
        imgIcon.SetNativeSize();
        SetText(txtContent, talkArr[1].Replace("$name", pd.name));
    }

    private void RefreshUI()
    {

    }

    public void ClickNextBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        dialogInd += 1;
        if (dialogInd == dialogArr.Length)
        {
            // TODO 发送任务引导完成任务
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqGuide,
                reqGuide = new ReqGuide
                {
                    guideid = curTaskData.ID,
                }
            };

            netSvc.SendMsg(msg);
            SetWndState(false);
        }
        else
        {
            SetTalk();
        }
    }
}