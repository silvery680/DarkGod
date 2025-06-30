/****************************************************
    File：DynamicWnd.cs
	Author：groudhog
    E-Mail: silvery680@gmail.com
    Time：2025/2/20 10:18:41
	Description ：动态UI元素界面
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWnd : WindowRoot 
{
    public Animation tipsAni;
    public Text txtTips;
    public Transform hpItemRoot;

    private bool isTipsShow = false;
    private Queue<string> tipsQue = new Queue<string>();
    private Dictionary<string, ItemEntityHp> itemDic = new Dictionary<string, ItemEntityHp>();

    private string preTips = "";

    protected override void InitWnd()
    {
        base.InitWnd();

        SetActive(txtTips, false);
    }
    private void Update()
    {
        if (tipsQue.Count > 0 && isTipsShow == false)
        {
            lock(tipsQue)
            {
                string tips = tipsQue.Dequeue();
                isTipsShow = true;
                SetTips(tips);
            }
        }
    }

    #region Tips相关
    public void AddTips(string tips)
    {
        // 防止多个线程同时访问
        lock (tipsQue)
        {
            if (tipsQue.Count == 0 || !String.Equals(tips, preTips))
            {
                preTips = tips;
                tipsQue.Enqueue(tips);
            }
        }
    }

    private void SetTips(string tips)
    {
        SetActive(txtTips);
        SetText(txtTips, tips);

        AnimationClip clip = tipsAni.GetClip("TipsShowAni");
        tipsAni.Play();

        // 延时关闭激活状态
        StartCoroutine(AniPlayDone(clip.length, () =>
        {
            SetActive(txtTips, false);
            isTipsShow = false;
        }));
    }

    private IEnumerator AniPlayDone(float sec, Action cb)
    {
        yield return new WaitForSeconds(sec);
        if (cb != null)
        {
            cb();
        }
    } 
    #endregion

    public void AddHpItemInfo(string key, Transform trans, int hp)
    {
        ItemEntityHp item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            return;
        }
        else
        {
            GameObject go = resSvc.LoadPrefab(PathDefine.HPItemPrefab, true);
            go.transform.SetParent(hpItemRoot);
            go.transform.localPosition = new Vector3(-1000, 0, 0);
            ItemEntityHp ieh = go.GetComponent<ItemEntityHp>();
            ieh.SetItemInfo(trans, hp);
            itemDic.Add(key, ieh);
        }
    }

    public void RemoveHpItemInfo(string key)
    {
        ItemEntityHp item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            Destroy(item.gameObject);
            itemDic.Remove(key);
        }
    }

    public void SetDodge(string key)
    {
        ItemEntityHp item = null;
        if (itemDic.TryGetValue(key,out item))
        {
            item.SetDodge();
        }
    }

    public void SetCritical(string key, int critical)
    {
        ItemEntityHp item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetCritical(critical);
        }
    }

    public void SetHurt(string key, int hurt)
    {
        ItemEntityHp item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetHurt(hurt);
        }
    }

    public void SetHpVal(string key, int oldVal, int newVal)
    {
        ItemEntityHp item = null;
        if (itemDic.TryGetValue(key, out item))
        {
            item.SetHpVal(oldVal, newVal);
        }
    }
}