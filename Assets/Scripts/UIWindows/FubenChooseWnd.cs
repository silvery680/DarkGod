/****************************************************
    File：FubenChooseWnd.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/27 21:11:22
	Description ：副本选择界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class FubenChooseWnd : WindowRoot 
{
    public Button[] fubenBtnArr;

    public Transform pointerTrans;

    private PlayerData pd;

    protected override void InitWnd()
    {
        base.InitWnd();
        pd = GameRoot.Instance.PlayerData;

        RefreshUI();
    }

    public void RefreshUI()
    {
        int fbid = pd.fuben;
        for (int i = 0; i < fubenBtnArr.Length; i ++)
        {
            if (i <  fbid % 10000)
            {
                SetActive(fubenBtnArr[i].gameObject);
                if (i == fbid % 10000 -1)
                {
                    pointerTrans.SetParent(fubenBtnArr[i].transform);
                    pointerTrans.localPosition = new Vector3(25, 100, 0);
                }
            }
            else
            {
                SetActive(fubenBtnArr[i].gameObject, false);
            }
        }
    }

    public void ClickFubenBtn(int fbid)
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);

        // 检查体力是否足够
        int power = resSvc.GetMapCfgData(fbid).power;
        if (power > pd.power)
        {
            GameRoot.AddTips("体力值不足");
        }
        else
        {
            netSvc.SendMsg(new GameMsg
            {
                cmd = (int)CMD.ReqFBFight,
                reqFBFight = new ReqFBFight
                {
                    fbid = fbid
                }
            });
        }
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}