/****************************************************
    File：MissionChooseWnd.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/27 21:11:22
	Description ：副本选择界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

public class MissionChooseWnd : WindowRoot 
{
    public Button[] missionBtnArr;

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
        int missionID = pd.mission;
        for (int i = 0; i < missionBtnArr.Length; i ++)
        {
            if (i <  missionID % 10000)
            {
                SetActive(missionBtnArr[i].gameObject);
                if (i == missionID % 10000 -1)
                {
                    pointerTrans.SetParent(missionBtnArr[i].transform);
                    pointerTrans.localPosition = new Vector3(25, 100, 0);
                }
            }
            else
            {
                SetActive(missionBtnArr[i].gameObject, false);
            }
        }
    }

    public void ClickCloseBtn()
    {
        audioSvc.PlayUIAudio(Constants.UIClickBtn);
        SetWndState(false);
    }
}