//==================================
//作者：李邵南
//日期：2017/3/9
//用途：奇缘系统界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

public class MiracleWnd : GUIBase
{
    public GameObject closeBtn;
    public GameObject addPlayer;
    public GameObject acceptBtn;
    /// <summary>
    /// 总共所需时间
    /// </summary>
    public UITimer totleTime;
    public UILabel timeLabel;
    public UILabel curFightLabel;
    public UILabel fightLabel;
    public UILabel yourName;
    /// <summary>
    /// 领取奖励按钮
    /// </summary>
    public UIButton addButton;
    /// <summary>
    /// 灰色按钮
    /// </summary>
    public UIButton getButton;
    public UILabel rewardLabel;
    public UILabel desLabel;
    public GameObject itemReward;
    /// <summary>
    /// 物品名字
    /// </summary>
    public GameObject[] nameLabel;
    /// <summary>
    /// 失败奖励
    /// </summary>
    public ItemUI defeatReward;
    /// <summary>
    /// 获胜奖励
    /// </summary>
    public ItemUI winReward;
    public GameObject myProp;
    public GameObject yourProp;
    void Awake() 
    {
        //是否为互斥窗口
        mutualExclusion = true;
        layer = GUIZLayer.NORMALWINDOW; 
    }
    protected override void OnOpen()
    {
        base.OnOpen();
        //请求任务协议
        GameCenter.miracleMng.C2S_ReqRoyalMiracleList();
        if (curFightLabel != null)
        {
            curFightLabel.text = GameCenter.mainPlayerMng.MainPlayerInfo.CurFightVal.ToString();
        }
        if (closeBtn != null) UIEventListener.Get(closeBtn.gameObject).onClick = BtnClose;
        if (addPlayer != null) UIEventListener.Get(addPlayer.gameObject).onClick = AddFriends;
        if (acceptBtn != null) UIEventListener.Get(acceptBtn.gameObject).onClick = AcceptThePrize;
        if (defeatReward != null) defeatReward.FillInfo(GameCenter.miracleMng.info1);
        RefreshMiracleWnd();
    }

    protected override void OnClose()
    {
        base.OnClose();
    }

    protected override void HandEvent(bool _bind)
    {
        base.HandEvent(_bind);
        if (_bind)
        {
            GameCenter.miracleMng.OnMiracleDataUpdateEvent += RefreshMiracleWnd;
        }
        else
        {
            GameCenter.miracleMng.OnMiracleDataUpdateEvent -= RefreshMiracleWnd;
        }
    }

    void BtnClose(GameObject _obj) 
    {
        //释放掉当前窗口
        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
    }
    void RefreshMiracleWnd() 
    {
        //改变自身图片显示
        if (GameCenter.mainPlayerMng.MainPlayerInfo.Prof == 1)
        {
            if (myProp != null)
            myProp.GetComponent<UISprite>().spriteName = "Pic_jj_rw_zs";
        }
        else if (GameCenter.mainPlayerMng.MainPlayerInfo.Prof == 2)
        {
            if (myProp != null)
            myProp.GetComponent<UISprite>().spriteName = "Pic_jj_rw_fs";
        }
        else if (GameCenter.mainPlayerMng.MainPlayerInfo.Prof == 3)
        {
            if (myProp != null)
            myProp.GetComponent<UISprite>().spriteName = "Pic_jj_rw_jx";
        }
        //改变对方显示图片
        if (GameCenter.miracleMng.profInfo == 1)
        {
            if (yourProp != null)
            yourProp.GetComponent<UISprite>().spriteName = "Pic_jj_rw_zs";
        }
        else if (GameCenter.miracleMng.profInfo == 2)
        {
            if (yourProp != null)
            yourProp.GetComponent<UISprite>().spriteName = "Pic_jj_rw_fs";
        }
        else if (GameCenter.miracleMng.profInfo == 3)
        {
            if (yourProp != null)
            yourProp.GetComponent<UISprite>().spriteName = "Pic_jj_rw_jx";
        }
        //系统正在进行中
        if (GameCenter.miracleMng.miracleStatus == AccessState.ACCEPTED)
        {
            if (totleTime != null)
            {
                totleTime.StartIntervalTimer(GameCenter.miracleMng.restTime - (int)Time.realtimeSinceStartup);
                //如果计时结束，则执行下面那个函数
                totleTime.onTimeOut = (x) =>
                    {
                        UpdateShow();
                    };
            }
            if (desLabel != null)
                desLabel.text = ConfigMng.Instance.GetUItext(131);
        }
        //任务获胜
        else if (GameCenter.miracleMng.miracleStatus == AccessState.SUCCEED)
        {
            //暂停计时
            if (totleTime != null) totleTime.StopTimer();
            if (desLabel != null)
                desLabel.text = ConfigMng.Instance.GetUItext(132);
            if (rewardLabel != null)
                rewardLabel.text = ConfigMng.Instance.GetUItext(139);
            if (totleTime != null)
            {
                timeLabel.text = "00:00";
            }
            if (getButton != null && addButton != null)
            {
                getButton.gameObject.SetActive(true);
                addButton.gameObject.SetActive(false);
            }
            if (acceptBtn != null)
            {
                acceptBtn.gameObject.SetActive(true);
            }
        }
        if (fightLabel != null)
        {
            fightLabel.text = GameCenter.miracleMng.scoreTarget.ToString();
        }
        if (winReward != null) winReward.FillInfo(GameCenter.miracleMng.info1);
        if (nameLabel != null)
        {
            for (int i = 0, max = nameLabel.Length; i < max; i++)
            {
                nameLabel[i].gameObject.SetActive(false);
            }
        }
        if (yourName != null)
        {
            if (GameCenter.miracleMng.playerID == 0)
                UpdateNameValue();
            else
            {
                //OtherPlayerInfo info = GameCenter.duplicateMng.GetInvitationPlayerData(GameCenter.miracleMng.playerID);
                yourName.text = GameCenter.miracleMng.targetName;
            }
        }
    }

    void UpdateShow() 
    {
       //任务失败
        if (GameCenter.miracleMng.miracleStatus == AccessState.ACCEPTED)
        {
            //暂停计时
            if (totleTime != null) totleTime.StopTimer();
            if (desLabel != null)
            {
                desLabel.text = ConfigMng.Instance.GetUItext(133);
            }
            if (getButton != null && addButton != null)
            {
                getButton.gameObject.SetActive(true);
                addButton.gameObject.SetActive(false);
            }
            if (rewardLabel != null)
                rewardLabel.text = ConfigMng.Instance.GetUItext(140);

            if (itemReward != null)
            {
                itemReward.gameObject.SetActive(true);
            }
            if (totleTime != null) 
            {
                timeLabel.text = "00:00";
            }
            if (acceptBtn != null)
            {
                acceptBtn.gameObject.SetActive(true);
            }
            if (winReward != null) winReward.FillInfo(GameCenter.miracleMng.info);
            if (nameLabel != null)
            {
                for (int i = 0, max = nameLabel.Length; i < max; i++)
                {
                    nameLabel[i].gameObject.SetActive(false);
                }
            }
        }
    }

    void AddFriends(GameObject _add) 
    {
          GameCenter.friendsMng.C2S_AddFriend(GameCenter.miracleMng.playerID);
    }
    //点击领取奖励
    void AcceptThePrize(GameObject _lq) 
    {
        if (GameCenter.miracleMng.miracleStatus == AccessState.SUCCEED)
        {
            GameCenter.miracleMng.C2S_ReqGetReward();
        }
        else if (GameCenter.miracleMng.miracleStatus == AccessState.ACCEPTED && GameCenter.miracleMng.restTime - (int)Time.realtimeSinceStartup <= 0) 
        {
            GameCenter.miracleMng.C2S_ReqGetReward();
        }
    }
    //通过服务端给的数据得到对应的名字
    void UpdateNameValue()
    {
        int Id = 0;
        if (int.TryParse(GameCenter.miracleMng.targetName, out Id))
        {
            int nameId = Id / 100;
            NameRef nameRef = ConfigMng.Instance.GetNameRef(nameId);
            string name = nameRef != null ? nameRef.Firstname : string.Empty;
            nameRef = ConfigMng.Instance.GetNameRef(Id % 100);
            name += (nameRef != null && nameRef.names.Count >= GameCenter.miracleMng.profInfo) ? nameRef.names[GameCenter.miracleMng.profInfo - 1] : string.Empty;
            yourName.text = name;
        }
        else 
        {
            yourName.text = GameCenter.miracleMng.targetName;
        }
    }
}
