/****************************************************
    File：BuyWnd.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/25 20:6:24
	Description ：购买交易窗口
*****************************************************/

using UnityEngine;

public class BuyWnd : WindowRoot 
{
    protected override void InitWnd()
    {
        base.InitWnd();
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    public void ClickSureBtn()
    {

    }
}