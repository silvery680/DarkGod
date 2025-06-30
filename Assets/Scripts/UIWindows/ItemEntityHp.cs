/****************************************************
    File：ItemEntityHp.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/6/29 9:57:33
	Description ：血条物体
*****************************************************/

using UnityEngine;
using UnityEngine.UI;

public class ItemEntityHp : MonoBehaviour 
{
    #region UI Define
    public Image imgHpGray;
    public Image imgHpRed;

    public Animation criticalAni;
    public Text txtCritical;

    public Animation dodgeAni;
    public Text txtDodge;

    public Animation hpAni;
    public Text txtHp;
    #endregion

    private RectTransform rect;
    private Transform rootTrans;
    private int hpVal;
    private float scaleRate = 1.0F * Constants.ScreenStandardHeight / Screen.height;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    SetCritical(666);
        //}

        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    SetDodge();
        //}

        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    SetHurt(666);
        //}

        Vector3 screenPos = Camera.main.WorldToScreenPoint(rootTrans.position);
        rect.anchoredPosition = screenPos * scaleRate;

        UpdateMixBlend();
        imgHpGray.fillAmount = currentPrg;
    }

    private void UpdateMixBlend()
    {
        if (Mathf.Abs(currentPrg - targetPrg) < Constants.AccelerHPSpeed * Time.deltaTime)
        {
            currentPrg = targetPrg;
        }
        else if (currentPrg > targetPrg)
        {
            currentPrg -= Constants.AccelerHPSpeed * Time.deltaTime;
        }
        else
        {
            currentPrg += Constants.AccelerHPSpeed * Time.deltaTime;
        }
        
    }

    public void SetItemInfo(Transform trans, int hp)
    {
        rect = transform.GetComponent<RectTransform>();
        rootTrans = trans;
        hpVal = hp;
        imgHpGray.fillAmount = 1;
        imgHpRed.fillAmount = 1;
    }

    public void SetCritical(int critical)
    {
        criticalAni.Stop();
        txtCritical.text = "暴击" + critical;
        criticalAni.Play();
    }

    public void SetDodge()
    {
        dodgeAni.Stop();
        txtDodge.text = "闪避";
        dodgeAni.Play();
    }

    public void SetHurt(int hurt)
    {
        hpAni.Stop();
        txtHp.text = "-" + hurt;
        hpAni.Play();
    }

    private float currentPrg;
    private float targetPrg;
    public void SetHpVal(int oldVal, int newVal)
    {
        currentPrg = oldVal * 1.0f / hpVal;
        targetPrg = newVal * 1.0f / hpVal;
        imgHpRed.fillAmount = targetPrg; 
    }
}