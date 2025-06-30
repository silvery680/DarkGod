/****************************************************
    File：PlayCtrlWnd.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/6/19 19:3:31
	Description ：王佳控制界面
*****************************************************/

using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayCtrlWnd : WindowRoot 
{
    public Image imgTouch;
    public Image imgDirBg;
    public Image imgDirPoint;

    public Text txtLevel;
    public Text txtName;
    public Text txtExpPrg;

    public Transform expPrgTrans;

    private float pointDis;
    private Vector2 startPos = Vector2.zero;
    private Vector2 defaultPos = Vector2.zero;

    public Vector2 currentDir;

    protected override void InitWnd()
    {
        base.InitWnd();

        pointDis = Constants.ScreenOPDis * PETools.GetGlobalRate();

        defaultPos = imgDirBg.transform.position;
        SetActive(imgDirPoint, false);
        RegisterTouchEvts();
        sk1CDTime = resSvc.GetSkillCfg(101).cdTime / 1000.0f;
        sk2CDTime = resSvc.GetSkillCfg(102).cdTime / 1000.0f;
        sk3CDTime = resSvc.GetSkillCfg(103).cdTime / 1000.0f;

        RefreshUI();
    }

    public void RefreshUI()
    {
        PlayerData pd = GameRoot.Instance.PlayerData;
        SetText(txtLevel, pd.lv);
        SetText(txtName, pd.name);


        #region EXP
        //express
        int expPrgVal = (int)(pd.exp * 1.0f / PECommon.GetExpUpValByLv(pd.lv) * 100);
        SetText(txtExpPrg, expPrgVal + "%");
        // 经验块索引号
        int index = expPrgVal / 10;

        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();

        float screenWidth = Screen.width / PETools.GetGlobalRate();
        float width = (screenWidth - 180) / 10;

        grid.cellSize = new Vector2(width, 7);

        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index)
            {
                img.fillAmount = 1;
            }
            else if (i == index)
            {
                img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            }
            else
            {
                img.fillAmount = 0;
            }
        }
        #endregion
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ClickNormalAtk();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ClickSkill1();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ClickSkill2();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ClickSkill3();
        }

        float delta = Time.deltaTime;
        if (isSk1CD)
        {
            sk1FillCount += delta;
            if (sk1FillCount >= sk1CDTime)
            {
                isSk1CD = false;
                SetActive(imgSk1CD, false);
                sk1FillCount = 0;
            }
            else
            {
                imgSk1CD.fillAmount = 1 - sk1FillCount / sk1CDTime;
            }

            sk1NumCount += delta;
            if (sk1NumCount >= 1)
            {
                sk1NumCount -= 1;
                sk1ShowNum -= 1;
                SetText(txtSk1CD, sk1ShowNum);
            }
        }

        if (isSk2CD)
        {
            sk2FillCount += delta;
            if (sk2FillCount >= sk2CDTime)
            {
                isSk2CD = false;
                SetActive(imgSk2CD, false);
                sk2FillCount = 0;
            }
            else
            {
                imgSk2CD.fillAmount = 1 - sk2FillCount / sk2CDTime;
            }

            sk2NumCount += delta;
            if (sk2NumCount >= 1)
            {
                sk2NumCount -= 1;
                sk2ShowNum -= 1;
                SetText(txtSk2CD, sk2ShowNum);
            }
        }

        if (isSk3CD)
        {
            sk3FillCount += delta;
            if (sk3FillCount >= sk3CDTime)
            {
                isSk3CD = false;
                SetActive(imgSk3CD, false);
                sk3FillCount = 0;
            }
            else
            {
                imgSk3CD.fillAmount = 1 - sk3FillCount / sk3CDTime;
            }

            sk3NumCount += delta;
            if (sk3NumCount >= 1)
            {
                sk3NumCount -= 1;
                sk3ShowNum -= 1;
                SetText(txtSk3CD, sk3ShowNum);
            }
        }
    }

    public void RegisterTouchEvts()
    {
        OnClickDown(imgTouch.gameObject, (PointerEventData evt) =>
        {
            startPos = evt.position;
            SetActive(imgDirPoint);
            imgDirBg.transform.position = evt.position;
        });

        OnClickUp(imgTouch.gameObject, (PointerEventData evt) =>
        {
            imgDirBg.transform.position = defaultPos;
            SetActive(imgDirPoint, false);
            imgDirPoint.transform.localPosition = Vector2.zero;

            // 方向信息传递
            currentDir = Vector2.zero;
            BattleSys.Instance.SetMoveDir(currentDir);
        });

        OnDrag(imgTouch.gameObject, (PointerEventData evt) =>
        {
            Vector2 dir = evt.position - startPos;
            float len = dir.magnitude;
            if (len > pointDis)
            {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                imgDirPoint.transform.position = startPos + clampDir;
            }
            else
            {
                imgDirPoint.transform.position = evt.position;
            }
            // 方向信息传递
            currentDir = dir.normalized;
            BattleSys.Instance.SetMoveDir(currentDir);
        });
    }

    public void ClickNormalAtk()
    {
        BattleSys.Instance.ReqReleaseSkill(0);
    }

    #region Skill1
    public Image imgSk1CD;
    public Text txtSk1CD;
    private bool isSk1CD;
    private float sk1CDTime;
    private int sk1ShowNum;
    private float sk1FillCount = 0;
    private float sk1NumCount = 0;
    public void ClickSkill1()
    {
        if (isSk1CD == false)
        {
            BattleSys.Instance.ReqReleaseSkill(1);
            isSk1CD = true;
            SetActive(imgSk1CD);
            imgSk1CD.fillAmount = 1;
            sk1ShowNum = (int)sk1CDTime;
            SetText(txtSk1CD, sk1ShowNum);
        }
    }
    #endregion

    #region Skill2
    public Image imgSk2CD;
    public Text txtSk2CD;
    private bool isSk2CD;
    private float sk2CDTime;
    private int sk2ShowNum;
    private float sk2FillCount = 0;
    private float sk2NumCount = 0;
    public void ClickSkill2()
    {
        if (isSk2CD == false)
        {
            BattleSys.Instance.ReqReleaseSkill(2);
            isSk2CD = true;
            SetActive(imgSk2CD);
            imgSk2CD.fillAmount = 1;
            sk2ShowNum = (int)sk2CDTime;
            SetText(txtSk2CD, sk2ShowNum);
        }
    }
    #endregion

    #region skill3
    public Image imgSk3CD;
    public Text txtSk3CD;
    private bool isSk3CD;
    private float sk3CDTime;
    private int sk3ShowNum;
    private float sk3FillCount = 0;
    private float sk3NumCount = 0;
    public void ClickSkill3()
    {
        if (isSk3CD == false)
        {
            BattleSys.Instance.ReqReleaseSkill(3);
            isSk3CD = true;
            SetActive(imgSk3CD);
            imgSk3CD.fillAmount = 1;
            sk3ShowNum = (int)sk3CDTime;
            SetText(txtSk3CD, sk3ShowNum);
        }
    } 
    #endregion
}