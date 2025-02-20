/****************************************************
    File：LoadingWnd.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/19 17:1:57
	Description ：加载进度界面
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : WindowRoot 
{
    public Text txtTips;
    public Image imgFG;
    public Image imgPoint;
    public Text txtPrg;

    private float fgWidth;

    protected override void InitWnd()
    {
        base.InitWnd();

        fgWidth = imgFG.GetComponent<RectTransform>().sizeDelta.x;

        SetText(txtTips, "这是一条游戏Tips");
        SetText(txtPrg, "0%");
        imgFG.fillAmount = 0;
        imgPoint.transform.localPosition = Vector3.zero;
    }

    public void SetProgress(float prg)
    {
        SetText(txtPrg, (int)(prg * 100) + "%");
        imgFG.fillAmount = prg;

        float posX = prg * fgWidth;
        imgPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(posX, 0);
    }
}