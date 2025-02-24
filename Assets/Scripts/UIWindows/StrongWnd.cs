/****************************************************
    File：StrongWnd.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/24 17:47:36
	Description ：强化升级窗口
*****************************************************/

using PEProtocol;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StrongWnd : WindowRoot 
{
    public Image imgCurtPos;
    public Text txtStartLv;
    public Transform starTransGrp;
    public Text propHp1;
    public Text propHurt1;
    public Text propDef1;
    public Text propHP2;
    public Text propHurt2;
    public Text propDef2;
    public Image propArr1;
    public Image propArr2;
    public Image propArr3;

    public Text txtNeedLv;
    public Text txtCostCoin;
    public Text txtCostCrystal;

    public Transform costTransRoot;
    public Text txtCoin;

    public Transform posBtnTrans;
    private Image[] imgs = new Image[6];
    private int currentIndex;
    private PlayerData pd;


    protected override void InitWnd()
    {
        base.InitWnd();

        pd = GameRoot.Instance.PlayerData;
        RegClickEvts();
        RefreshItem();
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }

    private void RegClickEvts()
    {
        for (int i = 0; i < posBtnTrans.childCount; i ++)
        {
            Image img = posBtnTrans.GetChild(i).GetComponent<Image>();

            OnClick(img.gameObject, (object args) =>
            {
                ClickPosItem((int)args);
                audioSvc.PlayUIAudio(Constants.UIClickBtn);
            }, i);
            imgs[i] = img;
        }
    }

    private void ClickPosItem(int index)
    {
        currentIndex = index;
        PECommon.Log("Click Item: " + index);
        for (int i = 0; i < imgs.Length; i ++)
        {
            Transform trans = imgs[i].transform;
            if (i == currentIndex)
            {
                // 箭头显示
                SetSprite(imgs[i], PathDefine.ItemArrorBG);
                trans.localPosition = new Vector3(10.3f, trans.localPosition.y, 0f);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(330f, 90f);
            }
            else
            {
                SetSprite(imgs[i], PathDefine.ItemPlatBG);
                trans.localPosition = new Vector3(0f, trans.localPosition.y, 0f);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 70f);
            }
        }
        RefreshItem();
    }

    private void RefreshItem()
    {
        switch(currentIndex)
        {
            case 0:
                SetSprite(imgCurtPos, PathDefine.ItemToukui);
                break;
            case 1:
                SetSprite(imgCurtPos, PathDefine.ItemBody);
                break;
            case 2:
                SetSprite(imgCurtPos, PathDefine.ItemYaobu);
                break;
            case 3:
                SetSprite(imgCurtPos, PathDefine.ItemHand);
                break;
            case 4:
                SetSprite(imgCurtPos, PathDefine.ItemLeg);
                break;
            case 5:
                SetSprite(imgCurtPos, PathDefine.ItemFoot);
                break;
        }
        SetText(txtStartLv, pd.strongArr[currentIndex] + "星级");

        int curtStartLv = pd.strongArr[currentIndex];
        for (int i = 0; i < starTransGrp.childCount; i ++)
        {
            Image img = starTransGrp.GetChild(i).GetComponent<Image>();
            if (i < curtStartLv)
            {
                SetSprite(img, PathDefine.SpStar2);
            }
            else
            {
                SetSprite(img, PathDefine.SpStar1);
            }
        }

        int sumAddHp = resSvc.GetPropAddValPreLv(currentIndex, curtStartLv, 1);
        int sumAddHurt = resSvc.GetPropAddValPreLv(currentIndex, curtStartLv, 2);
        int sumAddDef = resSvc.GetPropAddValPreLv(currentIndex, curtStartLv, 3);
        SetText(propHp1, "生命 " + sumAddHp);
        SetText(propHurt1, "伤害 " + sumAddHurt);
        SetText(propDef1, "防御 " + sumAddDef);

        int nextStartLv = curtStartLv + 1;
        StrongCfg nextSc = resSvc.GetStrongData(currentIndex, nextStartLv);
        if (nextSc != null)
        {
            SetActive(costTransRoot);

            SetActive(propHP2);
            SetActive(propHurt2);
            SetActive(propDef2);

            SetActive(propArr1);
            SetActive(propArr2);
            SetActive(propArr3);

            SetText(propHP2, "强化后 + " + nextSc.addHp);
            SetText(propHurt2, "强化后 + " + nextSc.addHurt);
            SetText(propDef2, "强化后 + " + nextSc.addDef);

            SetText(txtNeedLv, "需要等级: " + nextSc.minLv);
            SetText(txtCostCoin, nextSc.coin  + "/" + pd.coin);
            SetText(txtCostCrystal, nextSc.crystal + "/" + pd.crystal);
        }
        else
        {
            SetActive(propHP2, false);
            SetActive(propHurt2, false);
            SetActive(propDef2, false);

            SetActive(propArr1, false);
            SetActive(propArr2, false);
            SetActive(propArr3, false);

            SetActive(costTransRoot, false);
        }

        SetText(txtCoin, pd.coin);
    }
}