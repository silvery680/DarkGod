/****************************************************
    File：BuyWnd.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/25 20:6:24
	Description ：购买交易窗口
*****************************************************/

using PEProtocol;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BuyWnd : WindowRoot
{
    private BuyType buyType;
    public Text txtInfo;
    private PlayerData curtPd;

    protected override void InitWnd()
    {
        base.InitWnd();

        RefreshUI();
    }

    private void RefreshUI()
    {
        switch (buyType)
        {
            case BuyType.Power:
                txtInfo.text = "是否花费" + Constants.Color("10钻石", TxtColor.Red)
                     + "购买" + Constants.Color("100体力", TxtColor.Green) + "?";
                break;
            case BuyType.Coin:
                txtInfo.text = "是否花费" + Constants.Color("10钻石", TxtColor.Red)
                     + "购买" + Constants.Color("1000金币", TxtColor.Green) + "?";
                break;
        }
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    public void ClickSureBtn()
    {
        curtPd = GameRoot.Instance.PlayerData;
        int _cost;
        
        // 发送请求网络购买消息
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqBuy,
        };

        switch (buyType)
        {
            case BuyType.Power:
                {
                    if (TryBuyPower(out _cost))
                    {
                        msg.reqBuy = new ReqBuy
                        {
                            type = BuyType.Power,
                            cost = _cost,
                        };
                        netSvc.SendMsg(msg);
                    }
                }
                break;
            case BuyType.Coin:
                {
                    if (TryBuyCoin(out _cost))
                    {
                        msg.reqBuy = new ReqBuy
                        {
                            type = BuyType.Coin,
                            cost = _cost,
                        };
                        netSvc.SendMsg(msg);
                    }
                }
                break;
        }
    }

    private bool TryBuyPower(out int costDiamond)
    {
        costDiamond = 10;
        if (curtPd.diamond > costDiamond) return true;
        else
        {
            GameRoot.AddTips("钻石不足，购买失败");
            return false;
        }
    }

    private bool TryBuyCoin(out int costDiamond)
    {
        costDiamond = 20;
        if (curtPd.diamond > costDiamond) return true;
        else
        {
            GameRoot.AddTips("钻石不足，购买失败");
            return false;
        }
    }

    public void SetBuyType(BuyType type)
    {
        buyType = type;
    }

}