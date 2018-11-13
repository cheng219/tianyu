//==================================
//作者：黄洪兴
//日期：2016/5/10
//用途：武道会主界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

public class BudokaiWnd : GUIBase
{
    public UILabel labLog;
    public GameObject rankGrid;
    public GameObject applyBtn;
    public UISpriteEx applySprite;
    public UIScrollView uicroll;
    public GameObject CloseBtn;
    public BudokaiRankContainer rankContainer;
    public UILabel rank;
    public UILabel myName;
    public UILabel power;
    public UILabel score;
    public GameObject myRankObj;


    void Awake()
    {
        mutualExclusion = true;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        GameCenter.activityMng.OnOpenBudokaiWnd();
        AskBudokaiInfo();
        if (CloseBtn != null)
            UIEventListener.Get(CloseBtn).onClick += delegate { BtnClose(); };
        if (applyBtn != null) UIEventListener.Get(applyBtn).onClick += Apply;
        GameCenter.activityMng.OnGoTGotApplyInfo += RefreshApplyBtn;
        GameCenter.activityMng.BudokaiRankUpdate += RefreshRank;
        GameCenter.activityMng.BudokaiLogUpdate += RefreshLog;
        InvokeRepeating("AskBudokaiInfo", 0.2f, 30f);
        RefreshApplyBtn();
        if (GameCenter.activityMng.isFirstTime)
        {
            OpenDes();
            PlayerPrefs.SetInt("BUDOKAI_TIME", DateTime.Now.DayOfYear);
            GameCenter.activityMng.isFirstTime = false;
        }
    }
    protected override void OnClose()
    {
        base.OnClose();
        GameCenter.activityMng.OnCloseBudokaiWnd();
        if (CloseBtn != null)
            UIEventListener.Get(CloseBtn).onClick -= delegate { BtnClose(); };
        if (applyBtn != null) UIEventListener.Get(applyBtn).onClick -= Apply;
        GameCenter.activityMng.OnGoTGotApplyInfo -= RefreshApplyBtn;
        GameCenter.activityMng.BudokaiRankUpdate -= RefreshRank;
        GameCenter.activityMng.BudokaiLogUpdate -= RefreshLog;
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
        }
        else
        {
        }
    }

    void DestroyChild(GameObject grid)
    {
        if (grid != null)
        {
            grid.transform.DestroyChildren();
        }
    }


    void OpenDes()
    {
        GameCenter.uIMng.GenGUI(GUIType.DESCRIPTION, true);
        GameCenter.descriptionMng.OpenDes(21);
    }
    void RefreshRank()
    {
        DestroyChild(rankGrid);
        if (GameCenter.activityMng.BudokaiRankInfoDic .Count<1)
            return;
        for (int i = 1; i < GameCenter.activityMng.CurRankPage+1; i++)
        {
         rankContainer.RefreshItems(GameCenter.activityMng.BudokaiRankInfoDic[i]);
        }
        if(rank!=null) rank.text = GameCenter.activityMng.PlayerRankInfo.Rank.ToString();
        if(myName!=null)myName.text = GameCenter.activityMng.PlayerRankInfo.Name.ToString();
        if(power!=null)power.text = GameCenter.activityMng.PlayerRankInfo.Power.ToString();
        if(score!=null)score.text = GameCenter.activityMng.PlayerRankInfo.Score.ToString();
        if (myRankObj != null) myRankObj.SetActive(true);
        
    }

    void RefreshLog()
    {
        if (GameCenter.activityMng.LogList == null)
            return;
        List<budo_log_list> logList = GameCenter.activityMng.LogList;
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        for (int i = 0, max = logList.Count; i < max; i++)
        {

            string log = ConfigMng.Instance.GetGlogStringByID(logList[i].rand_id);
            if (log == null || log == string.Empty)
                continue;
            string logText = string.Empty;
            if (!string.IsNullOrEmpty(log))
            {
                string[] words = logList[i].arg.Split(',');
                if (words.Length == 2)
                {
 //                   string[] words2 = { words[1], words[0] };
                    logText = UIUtil.Str2Str(log, words);
                    builder.Append(logText);
                }
            }
            if (i < max - 1)
            {
                builder.Append("\n");
                builder.Append("\n");
            }
         
        }
        if (labLog != null) labLog.text = builder.ToString();
    }



    void Apply(GameObject obj)
    {
        GameCenter.activityMng.C2S_AskBudokai(2);
    }


    void BtnClose()
    {
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }

    void RefreshApplyBtn()
    {

        if(!GameCenter.activityMng.Apply)
        {
            applyBtn.GetComponent<UIButton>().enabled = true;
            applyBtn.GetComponent<BoxCollider>().enabled = true;
            applySprite.IsGray = UISpriteEx.ColorGray.normal;
        }
        else
        {
            applyBtn.GetComponent<UIButton>().enabled = false;
            applyBtn.GetComponent<BoxCollider>().enabled = false;
            applySprite.IsGray = UISpriteEx.ColorGray.Gray;
        }

    }
    void AskBudokaiInfo()
    {
        GameCenter.activityMng.C2S_AskBudokaiRank(GameCenter.activityMng.CurRankPage);
        GameCenter.activityMng.C2S_AskBudokai(1);
    }

}
