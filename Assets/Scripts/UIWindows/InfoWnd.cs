/****************************************************
    File：InfoWnd.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/23 15:11:32
	Description ：角色信息展示界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoWnd : WindowRoot
{
    public RawImage imgChar;

    public Text txtInfo;
    public Text txtExp;
    public Image imgExpPrg;
    public Text txtPow;
    public Image imgPowPrg;

    public Text txtJob;
    public Text txtFight;
    public Text txtHp;
    public Text txtAtk;
    public Text txtDef;

    public Text dtxHp;
    public Text dtxAd;
    public Text dtxAp;
    public Text dtxAdDef;
    public Text dtxApDef;
    public Text dtxDodge;
    public Text dtxPierce;
    public Text dtxCritical;

    public Transform transDetail;

    protected override void InitWnd()
    {
        base.InitWnd();
        RegTouchEvts();
        SetActive(transDetail, false);
        RefreshUI();
    }

    private Vector2 startPos;
    private void RegTouchEvts()
    {
        OnClickDown(imgChar.gameObject, (PointerEventData evt) =>
        {
            startPos = evt.position;
            MainCitySys.Instance.SetStartRoate();
        });
        OnDrag(imgChar.gameObject, (PointerEventData evt) =>
        {
            float rote = evt.position.x - startPos.x;
            MainCitySys.Instance.SetPlayerRote(rote);
        });
    }

    private void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtInfo, pd.name + "LV." + pd.lv);
        SetText(txtExp, pd.exp + "/" + PECommon.GetExpUpValByLv(pd.lv));
        imgExpPrg.fillAmount = pd.exp * 1.0f / PECommon.GetExpUpValByLv(pd.lv);
        SetText(txtPow, pd.power + "/" + PECommon.GetPowerLimit(pd.lv));
        imgPowPrg.fillAmount = pd.power * 1.0f / PECommon.GetPowerLimit(pd.lv);

        SetText(txtJob, "暗夜刺客");
        SetText(txtFight, PECommon.GetFightByProps(pd));
        SetText(txtHp, pd.hp);
        SetText(txtAtk, (pd.ad + pd.ap));
        SetText(txtDef, (pd.addef + pd.apdef));

        SetText(dtxHp, pd.hp);
        SetText(dtxAd, pd.ad);
        SetText(dtxAp, pd.ap);
        SetText(dtxAdDef, pd.addef);
        SetText(dtxApDef, pd.apdef);
        SetText(dtxDodge, pd.dodge);
        SetText(dtxPierce, pd.pierce);
        SetText(dtxCritical, pd.critical);
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        MainCitySys.Instance.CloseInfoWnd();
    }

    public void ClickDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetActive(transDetail);
    }

    public void ClickCloseDetailBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetActive(transDetail, false);
    }
}