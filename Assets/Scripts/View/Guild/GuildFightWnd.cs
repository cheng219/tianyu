//==================================
//作者：黄洪兴
//日期：2016/5/14
//用途：仙域争霸界面类
//=================================

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GuildFightWnd : GUIBase
{
    public UITexture bgmTex;
    public GameObject unopenedObj;
    public UILabel sessionNum;
    public GameObject closeBtn;
    public GameObject previousSessionBtn;
    public GameObject nextSessionBtn;
    public GameObject starBtn;
    public GameObject explainBtn;
    public List<GuildFightItemUI> group1;
    public List<GuildFightItemUI> group2;
    public List<GuildFightItemUI> group3;
    public UILabel KingGroup;
    int num;
    void Awake()
    {
        mutualExclusion = false;
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        if (bgmTex != null) if (bgmTex != null) ConfigMng.Instance.GetBigUIIcon("Pic_jjc_bg", delegate(Texture2D texture)
        {
            bgmTex.mainTexture = texture;
        });
        GameCenter.guildFightMng.C2S_AskGuildFightNum();
        Refresh();
    }
    protected override void OnClose()
    {
        base.OnClose();
    }
    void OnDestroy()
    {
        ConfigMng.Instance.RemoveBigUIIcon("Pic_jjc_bg");
    }
    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            if (closeBtn != null) UIEventListener.Get(closeBtn).onClick += CloseThis;
            if (previousSessionBtn != null) UIEventListener.Get(previousSessionBtn).onClick += OnPreviousSessionBtn;
            if (nextSessionBtn != null) UIEventListener.Get(nextSessionBtn).onClick += OnNextSessionBtn;
            if (starBtn != null) UIEventListener.Get(starBtn).onClick += OnStarBtn;
            GameCenter.guildFightMng.OnGuildFightNumUpdate += AskGuildFightInfo;
            GameCenter.guildFightMng.OnGetGuildFightInfo += Refresh;

        }
        else
        {
            if (closeBtn != null) UIEventListener.Get(closeBtn).onClick -= CloseThis;
            if (previousSessionBtn != null) UIEventListener.Get(previousSessionBtn).onClick -= OnPreviousSessionBtn;
            if (nextSessionBtn != null) UIEventListener.Get(nextSessionBtn).onClick -= OnNextSessionBtn;
            if (starBtn != null) UIEventListener.Get(starBtn).onClick -= OnStarBtn;
            GameCenter.guildFightMng.OnGuildFightNumUpdate -= AskGuildFightInfo;
            GameCenter.guildFightMng.OnGetGuildFightInfo -= Refresh;
  
        }
    }
    //void Update()
    //{
       
    //}

    void Refresh()
    {

        if (sessionNum!=null)
        {
            if (GameCenter.guildFightMng.GuildFightIndex < 1)
                sessionNum.text = ConfigMng.Instance.GetUItext(280);
            sessionNum.text = ConfigMng.Instance.GetUItext(181) + GameCenter.guildFightMng.GuildFightIndex + ConfigMng.Instance.GetUItext(281);
        }

        if (GameCenter.guildFightMng.GuildListOne != null)
        {
            if (GameCenter.guildFightMng.GuildListOne.Count > 0 && GameCenter.guildFightMng.GuildListOne.Count <= group1.Count)
            {
                int count = 0;
                for (int i = 0; i < GameCenter.guildFightMng.GuildListOne.Count; i++)
                {
                    group1[i].FillInfo(GameCenter.guildFightMng.GuildListOne[i]);
                    count++;
                }
                if (count < group1.Count)
                {
                    for (int j = count; j < group1.Count; j++)
                    {
                        group1[j].FillInfo(null);
                    }
                }
            }
            else
            {
                for (int j = 0; j < group1.Count; j++)
                {
                    group1[j].FillInfo(null);
                }
            }
        }

        if (GameCenter.guildFightMng.GuildListTwo != null)
        {
            if (GameCenter.guildFightMng.GuildListTwo.Count > 0 && GameCenter.guildFightMng.GuildListTwo.Count <= group2.Count)
            {
                int count = 0;
                for (int i = 0; i < GameCenter.guildFightMng.GuildListTwo.Count; i++)
                {
                    group2[i].FillInfo(GameCenter.guildFightMng.GuildListTwo[i]);
                    count++;
                }
                if (count < group2.Count)
                {
                    for (int j = count; j < group2.Count; j++)
                    {
                        group2[j].FillInfo(null);
                    }
                }
            }
            else
            {
                for (int j = 0; j < group2.Count; j++)
                {
                    group2[j].FillInfo(null);
                }
            }
        }
        
        if (GameCenter.guildFightMng.GuildListThree != null)
        {
            if (GameCenter.guildFightMng.GuildListThree.Count > 0 && GameCenter.guildFightMng.GuildListThree.Count <= group3.Count)
            {
                int count = 0;
                for (int i = 0; i < GameCenter.guildFightMng.GuildListThree.Count; i++)
                {
                    group3[i].FillInfo(GameCenter.guildFightMng.GuildListThree[i]);
                    count++;
                }
                //foreach (var item in GameCenter.guildFightMng.GuildListThree)
                //{
                //    group3[i].text = item.Key;
                //    i++;
                //}
                if (count < group3.Count)
                {
                    for (int j = count; j < group3.Count; j++)
                    {
                        group3[j].FillInfo(null);
                    }
                }
            }
            else
            {
                for (int j = 0; j < group3.Count; j++)
                {
                    group3[j].FillInfo(null);
                }
            }
        }
       
        if (GameCenter.guildFightMng.Champion != null )
        {
            KingGroup.text = GameCenter.guildFightMng.Champion;
        }
        else
        {
            KingGroup.text = string.Empty;
        }

        if (unopenedObj != null)
        {
            unopenedObj.SetActive(!GameCenter.guildFightMng.isOpen);
        }



     
    }
    void AskGuildFightInfo()
    {
        GameCenter.guildFightMng.C2S_AskGuildFightInfo(GameCenter.guildFightMng.GuildFightNum+1);
    }



    void CloseThis(GameObject obj)
    {
        //GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        GameCenter.uIMng.ReleaseGUI(GUIType.GUILDFIGHT);
    }

    void OnPreviousSessionBtn(GameObject obj)
    {
        if (GameCenter.guildFightMng.GuildFightIndex <=1)
            return;
        GameCenter.guildFightMng.C2S_AskGuildFightInfo(GameCenter.guildFightMng.GuildFightIndex -1);

    }
    void OnNextSessionBtn(GameObject obj)
    {
        if (GameCenter.guildFightMng.GuildFightIndex > GameCenter.guildFightMng.GuildFightNum)
            return;
        if (GameCenter.guildFightMng.GuildFightIndex < 1)
            return;
        GameCenter.guildFightMng.C2S_AskGuildFightInfo(GameCenter.guildFightMng.GuildFightIndex+1);
    }
    void OnStarBtn(GameObject obj)
    {
        if (GameCenter.activityMng.GetActivityState(ActivityType.FAIRYAFIGHT) != ActivityState.ONGOING)
            GameCenter.messageMng.AddClientMsg(173);
        else
            GameCenter.guildFightMng.C2S_AskEnterGuildFight();
    }

}
