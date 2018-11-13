//============================
//作者：唐源
//日期：2017/2/3
//用途：副本管理类
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;
using System.Text;
public class DuplicateMng {
    #region 构造
    public static DuplicateMng CreateNew()
    {
        if (GameCenter.duplicateMng == null)
        {
            DuplicateMng duplicateMng  = new DuplicateMng();
            duplicateMng.Init();
            return duplicateMng;
        }
        else
        {
            GameCenter.duplicateMng.UnRegist();
            GameCenter.duplicateMng.Init();
            return GameCenter.duplicateMng;
        }
    }

    protected void Init()
    {
        //MsgHander.Regist(0xD435, S2C_AllEndList);
        //MsgHander.Regist(0xD437, S2C_AllEndReWard);
        //MsgHander.Regist(0xD449, S2C_EndLessItemUpdate);
        //MsgHander.Regist(0xD451, S2C_SweepRewardAll);

        MsgHander.Regist(0xD453, S2C_CopyItemList);
        MsgHander.Regist(0xD455, S2C_BuyCopyInNum);
        MsgHander.Regist(0xD457, S2C_CopyInSceneStar);
        MsgHander.Regist(0xD460, S2C_CopyItemTeams);

        MsgHander.Regist(0xD465, S2C_ReqCopyInTeamClose);
        MsgHander.Regist(0xD463, S2C_ReqCopyInChange);
        MsgHander.Regist(0xD466, S2C_ReqCopyInPerpare);
        MsgHander.Regist(0xD467, S2C_ReqSettlementRewardData);
        MsgHander.Regist(0xD469, S2C_ReqSettlementFlopData);
        MsgHander.Regist(0xD470, S2C_ReqOpenCopyForce);

        //MsgHander.Regist(0xD485, S2C_ArenaServerDataInfo);
        MsgHander.Regist(0xD491, S2C_ReqArenaRewardData);
        //MsgHander.Regist(0xD489, S2C_ArenaServerReward);
        MsgHander.Regist(0xD47a, S2C_GameStop);
        //MsgHander.Regist(0xD715, S2C_ActivityDataInfo);
        MsgHander.Regist(0xD716, S2C_MagicTowers);

        MsgHander.Regist(0xD746, S2C_InvitationPlayer);
        MsgHander.Regist(0xD749, S2C_CopyInFriend);
        MsgHander.Regist(0xD753, S2C_ReqOpenCoppyTime);

        //MsgHander.Regist(0xD756, S2C_FairyDomainToprotect);
        MsgHander.Regist(0xD771, S2C_TeamAddInvitationPlayer);
        MsgHander.Regist(0xC147, S2C_GetTowerChallengeNum);
    }

    protected void UnRegist()
    {
        IsGameStop = false;
        //isFastClearance = false;
        //MsgHander.UnRegist(0xD435, S2C_AllEndList);
        //MsgHander.UnRegist(0xD437, S2C_AllEndReWard);
        //MsgHander.UnRegist(0xD449, S2C_EndLessItemUpdate);
        //MsgHander.UnRegist(0xD451, S2C_SweepRewardAll);

        MsgHander.UnRegist(0xD453, S2C_CopyItemList);
        MsgHander.UnRegist(0xD455, S2C_BuyCopyInNum);
        MsgHander.UnRegist(0xD457, S2C_CopyInSceneStar);
        MsgHander.UnRegist(0xD460, S2C_CopyItemTeams);

        MsgHander.UnRegist(0xD465, S2C_ReqCopyInTeamClose);
        MsgHander.UnRegist(0xD463, S2C_ReqCopyInChange);
        MsgHander.UnRegist(0xD466, S2C_ReqCopyInPerpare);
        MsgHander.UnRegist(0xD467, S2C_ReqSettlementRewardData);
        MsgHander.UnRegist(0xD469, S2C_ReqSettlementFlopData);
        MsgHander.UnRegist(0xD470, S2C_ReqOpenCopyForce);

        //MsgHander.UnRegist(0xD485, S2C_ArenaServerDataInfo);
        MsgHander.UnRegist(0xD491, S2C_ReqArenaRewardData);
        //MsgHander.UnRegist(0xD489, S2C_ArenaServerReward);
        MsgHander.UnRegist(0xD47a, S2C_GameStop);
        //MsgHander.UnRegist(0xD715, S2C_ActivityDataInfo);
        MsgHander.UnRegist(0xD716, S2C_MagicTowers);

        MsgHander.UnRegist(0xD746, S2C_InvitationPlayer);
        MsgHander.UnRegist(0xD749, S2C_CopyInFriend);
        MsgHander.UnRegist(0xD753, S2C_ReqOpenCoppyTime);
        //MsgHander.UnRegist(0xD756, S2C_FairyDomainToprotect);
        MsgHander.UnRegist(0xD771, S2C_TeamAddInvitationPlayer);
        MsgHander.UnRegist(0xC147, S2C_GetTowerChallengeNum);

        copyDic.Clear();
        copyTeams.Clear();
        openCopyForceTip = false;
        CopySettlementDataInfo = null;
        //ArenaServerDataInfo = null;
        //activityDic.Clear();
        //sweepListItem.Clear();
        friendmsg.Clear();
        //curId = 1;
        copyGroupID = 0;
        againCopyID = 0;
        againSceneID = 0;
        lcopyGroupRef = null;
        isChallengeMagicTower = false;
        isShowBuyCopyTip = true;
        isShowBuySweepItem = true;
    }
    #endregion
    #region 单人与多人副本
    #region 副本跳转
    /// <summary>
    /// 当前选择的副本
    /// </summary>
    public OneCopySType CurSelectOneCopyType = OneCopySType.NONE;
    /// <summary>
    /// 进入副本界面，选中副本
    /// </summary>
    public void OpenCopyWndSelected(SubGUIType _type, OneCopySType _oneCopyType)
    {
        CurSelectOneCopyType = _oneCopyType;
        GameCenter.uIMng.SwitchToSubUI(_type);
    }
    /// <summary>
    /// 根据副本类型选择要进入的副本 addby zsy
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_oneCopyType"></param>
    public void OpenCopyWndSelected(CopysType _type, OneCopySType _oneCopyType = OneCopySType.NONE)
    {
        CurSelectOneCopyType = _oneCopyType;
        CopyType = _type;
        switch (_type)
        { 
            case CopysType.ONESCOPY:
                GameCenter.uIMng.SwitchToSubUI(SubGUIType.BCopyTypeOne);
                break;
            case CopysType.TWOSCOPY:
                GameCenter.uIMng.SwitchToSubUI(SubGUIType.BCopyType);
                break;
            case CopysType.MAGICTOWER:
                GameCenter.uIMng.SwitchToUI(GUIType.MagicTowerWnd);
                break;
        }
        //GameCenter.uIMng.SwitchToSubUI(_type);
    }
    #endregion
    #region 事件, 变量与访问器

    /// <summary>
    /// 今日镇魔塔是否挑战
    /// </summary>
    public bool isChallengeMagicTower = false;
    public System.Action OnMagicChallengeUpdate;

    Dictionary<int, CopyInItemDataInfo> copyDic = new Dictionary<int, CopyInItemDataInfo>();
    /// <summary>
    /// 多人副本数据
    /// </summary>
    public Dictionary<int, CopyInItemDataInfo> CopyDic
    {
        get
        {
            return copyDic;
        }
    }
    Dictionary<int, CopySceneTeamPlayerInfo> copyTeams = new Dictionary<int, CopySceneTeamPlayerInfo>();
    /// <summary>
    /// 多人副本组队数据
    /// </summary>
	public Dictionary<int, CopySceneTeamPlayerInfo> CopyTeams
    {
        get
        {
            return copyTeams;
        }
    }
    CopysType copyType = CopysType.ONESCOPY;
    /// <summary>
    /// 1=单人副本,2=多人副本,3= 镇魔塔
    /// </summary>
    public enum CopysType
    {
        ONESCOPY = 1,
        TWOSCOPY = 2,
        MAGICTOWER = 3,
    }
    /// <summary>
    /// 副本类型
    /// </summary>
    public CopysType CopyType
    {
        get
        {
            return copyType;
        }
        set
        {
            if (copyType != value)
            {
                copyType = (CopysType)value;
                if (OnCopyTypeChange != null) OnCopyTypeChange();
            }
        }
    }
    /// <summary>
    /// 当前副本组ID，共跳转到副本界面并选中某个副本组
    /// </summary>
    public int copyGroupID = 0;

    /// <summary>
    /// 化身数据
    /// </summary>
    public List<OtherPlayerInfo> OpenInvitationPlayerData
    {
        get
        {
            List<OtherPlayerInfo> Data = new List<OtherPlayerInfo>();
            for (int i = 0; i < openInvitationPlayerData.Count; i++)
            {
                if (!copyTeams.ContainsKey(openInvitationPlayerData[i].ServerInstanceID))
                {
                    Data.Add(openInvitationPlayerData[i]);
                }
            }
            return Data;
        }
    }
    List<OtherPlayerInfo> openInvitationPlayerData = new List<OtherPlayerInfo>();
    public OtherPlayerInfo GetInvitationPlayerData(int id)
    {
        for (int i = 0; i < openInvitationPlayerData.Count; i++)
        {
            if (id == openInvitationPlayerData[i].ServerInstanceID)
            {
                return openInvitationPlayerData[i];
            }
        }
        return null;
    }
    /// <summary>
    /// 多人准备数据变化
    /// </summary>
    public System.Action OnSelectChange;
    /// <summary>
    /// 化身数据变化
    /// </summary>
    public System.Action OpenInvitationPlayer;
    /// <summary>
    /// 副本类型变化
    /// </summary>
    public System.Action OnCopyTypeChange;
    /// <summary>
    /// 多人界面队员数据
    /// </summary>
    public System.Action<int> OnCopyItemTeamData;
    /// <summary>
    /// 副本数据变化
    /// </summary>
    public System.Action OnCopyItemChange;
    /// <summary>
    /// 多人界面关闭
    /// </summary>
    public System.Action OnCopyItemUIClose;
    /// <summary>
    /// 镇魔塔开启
    /// </summary>
    public System.Action<int, int> OnMagicTowerUIOpen;
    /// <summary>
    /// 镇魔塔点击招募队友
    /// </summary>
    public System.Action OnClickAddFreiend;
    /// <summary>
    /// 是否是镇魔塔招募队友
    /// </summary>
    public bool isMagicTowrAddFri = false;
    public bool isClickAddFri = false;
    #endregion
    #region 副本和扫荡 快捷购买
    /// <summary>
    /// 获取相应Vip等级和购买次数所需的元宝
    /// </summary>
    int GetDiamoNum(int _buyNum, CopyGroupRef _data)
    {
        if (_data == null) return 0;
        VIPRef refData = ConfigMng.Instance.GetVIPRef(GameCenter.vipMng.VipData.vLev);
        CopyTimes times = null;
        if (refData != null)
        {
            for (int i = 0; i < refData.copyPurchasetimes.Count; i++)
            {
                times = refData.copyPurchasetimes[i];
                if (times.copyID == _data.id && times.copyTimes > 0)
                {
                    break;
                }
            }
        }
        //Debug.Log("该VIP一共有多少购买次数 ：" + times.copyTimes + " 还剩几次购买 : " + _buyNum + "  当前是第几次购买 : " + (times.copyTimes - _buyNum + 1));
        //times.copyTimes(该VIP一共有多少次购买次数，配表读取)
        //_buyNum(还剩多少次购买次数，服务端记录)
        //setpId(本次是第几次购买)
        int setpId = times.copyTimes - _buyNum + 1;
        StepConsumptionRef stepConsumptionRef = ConfigMng.Instance.GetStepConsumptionRef(setpId);
        return stepConsumptionRef != null ? stepConsumptionRef.copyNumber[0].count : 5;
    }
    public CopyGroupRef lcopyGroupRef = null; 
    /// <summary>
    /// 是否显示购买副本的提示
    /// </summary>
    public bool isShowBuyCopyTip = true;
    /// <summary>
    /// 是否显示购买扫荡卷轴的提示
    /// </summary>
    public bool isShowBuySweepItem = true;
    /// <summary>
    /// 弹出快捷购买提示
    /// </summary>
    public void PopTip(CopyInItemDataInfo _serData, CopyGroupRef _data, int _type, bool _isSweep, bool _isUseSweepItem)
    {
        if (_serData == null || _data == null) return;
        bool isShowTip = true;
        if (_serData.buyNum > 0)//可购买次数大于0，弹出快捷购买次数
        {
            if (GameCenter.endLessTrialsMng.sweepType == EndLessTrialsMng.SweepType.COPY)
            { 
                isShowTip = isShowBuyCopyTip;
            }
            if (isShowTip)
            {
                int needGold = GetDiamoNum(_serData.buyNum, _data);
                MessageST msg = new MessageST();
                object[] pa = { 1 };
                msg.pars = pa;
                msg.delPars = delegate(object[] ob)
                {
                    if (ob.Length > 0)
                    {
                        bool b = (bool)ob[0];
                        if (b)
                        {
                            isShowBuyCopyTip = false;
                        }
                    }
                };
                msg.messID = 499;
                msg.words = new string[1] { needGold.ToString() };
                msg.delYes = delegate
                {
                    //元宝是否充足
                    if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount >= (ulong)needGold)
                    {
                        if (_isSweep && _isUseSweepItem)//是否是扫荡
                        { 
                            ShowBuySweepItemPop(_data.id, _type); 
                        }
                        else
                        {
                            GameCenter.duplicateMng.C2S_BuyCopyInItem(_data.id, 1);
                            GameCenter.duplicateMng.C2S_ToCopyItem(_data.id, _type);
                        }
                    }
                    else
                    {
                        MessageST mst = new MessageST();
                        mst.messID = 137;
                        mst.delYes = delegate
                        {
                            GameCenter.uIMng.ReleaseGUI(GUIType.SWEEPCARBON);
                            // 跳转到充值界面
                            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                        };
                        GameCenter.messageMng.AddClientMsg(mst);
                    }
                };
                GameCenter.messageMng.AddClientMsg(msg);
            }
            else//不用弹购买提示
            {
                if (_isUseSweepItem && isShowBuySweepItem)
                {
                    ShowBuySweepItemPop(_data.id, _type);
                }
                else
                {
                    GameCenter.duplicateMng.C2S_BuyCopyInItem(_data.id, 1);
                    GameCenter.duplicateMng.C2S_ToCopyItem(_data.id, _type);
                }
            }
        }
        else//可购买次数小于0跳转到充值界面
        {
            MessageST msg = new MessageST();
            msg.messID = 500;
            msg.delYes = delegate
            {
                GameCenter.uIMng.ReleaseGUI(GUIType.SWEEPCARBON);
                GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
            };
            GameCenter.messageMng.AddClientMsg(msg);
        }
    }


    public void ShowBuySweepItemPop(int _id, int _type)
    {
        if (GameCenter.inventoryMng.GetNumberByType(2210001) <= 0)//新增扫荡卷轴消耗 
        {
            EquipmentInfo eqinfo = new EquipmentInfo(2210001, EquipmentBelongTo.PREVIEW);
            MessageST mst = new MessageST();
            object[] pa = { 1 };
            mst.pars = pa;
            mst.delPars = delegate(object[] ob)
            {
                if (ob.Length > 0)
                {
                    bool b = (bool)ob[0];
                    if (b)
                    {
                        isShowBuySweepItem = false;
                    }
                }
            };
            mst.messID = 543;
            mst.words = new string[1] { eqinfo.DiamondPrice.ToString() };
            mst.delYes = delegate
            {
                if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount >= eqinfo.DiamondPrice)
                {
                    GameCenter.duplicateMng.C2S_BuyCopyInItem(_id, 1);
                    GameCenter.endLessTrialsMng.C2S_SweepReward(1, _type);
                }
                else
                {
                    MessageST mst1 = new MessageST();
                    mst1.messID = 137;
                    mst1.delYes = delegate
                    {
                        GameCenter.uIMng.ReleaseGUI(GUIType.SWEEPCARBON);
                        GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                    };
                    GameCenter.messageMng.AddClientMsg(mst1);
                }
            };
            GameCenter.messageMng.AddClientMsg(mst);
        }
        else
        {
            GameCenter.duplicateMng.C2S_BuyCopyInItem(_id, 1);
            GameCenter.endLessTrialsMng.C2S_SweepReward(1, _type);
        }
    }
    #endregion
    /// <summary>
	/// 1=单人副本,2=多人副本(红点显示)
	/// </summary>
    public bool IsTipCopyTypeRedShow(int type)
    {
        foreach (CopyInItemDataInfo data in copyDic.Values)
        {
            CopyGroupRef refdata = ConfigMng.Instance.GetCopyGroupRef(data.id);
            if (refdata != null && refdata.sort == type && data.num > 0 && refdata.lv <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)
            {
                if (refdata.sort == 1)
                {
                    //int funcType = 0;
                    FunctionType funcType = FunctionType.None;
                    switch (refdata.id)
                    {
                        case 1: funcType = FunctionType.COPY; break;
                        case 2: funcType = FunctionType.DOUSHUAIASGARD; break;
                        case 3: funcType = FunctionType.FIVEMANOR; break;
                        case 5: funcType = FunctionType.TRUEORFALSEMONKEY; break;
                        case 6: funcType = FunctionType.WHITEPURGATORY; break;
                    }
                    if (GameCenter.mainPlayerMng.FunctionIsOpen(funcType))
                        return true;
                }
                else
                    return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 副本进入按钮红点显示
    /// </summary>
	void SetAllCopyRedPoint()
    {
        foreach (CopyInItemDataInfo data in copyDic.Values)
        {
            CopyGroupRef refdata = ConfigMng.Instance.GetCopyGroupRef(data.id);
            if (refdata != null && data.num > 0 && refdata.lv <= GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel)
            {
                if (refdata.sort == 1)
                {
                    FunctionType funcType = FunctionType.None;
                    switch (refdata.id)
                    {
                        case 1: funcType = FunctionType.COPY; break;
                        case 2: funcType = FunctionType.DOUSHUAIASGARD; break;
                        case 3: funcType = FunctionType.FIVEMANOR; break;
                        case 5: funcType = FunctionType.TRUEORFALSEMONKEY; break;
                        case 6: funcType = FunctionType.WHITEPURGATORY; break;
                    }
                    if (GameCenter.mainPlayerMng.FunctionIsOpen(funcType))
                    {
                        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.COPY, true);
                        return;
                    }
                }
                else
                {
                    GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.COPY, true);
                    return;
                }
            }
        }
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.COPY, false);
    }

    /// <summary>
    /// 好友数据
    /// </summary>
    public List<OtherPlayerInfo> GetOtherPlayerInfoS()
    {
        List<OtherPlayerInfo> lists = new List<OtherPlayerInfo>();
        OtherPlayerData playerEty = null;
        //===========fix list禁止用foreach，应用for
        for (int i = 0; i < GameCenter.friendsMng.friendList.Count; i++)
        {
            if (GameCenter.friendsMng.friendList[i].IsOnline)
            {
                FriendsInfo info = GameCenter.friendsMng.friendList[i];
                playerEty = new OtherPlayerData();
                playerEty.serverInstanceID = info.configId;
                playerEty.name = info.name;
                playerEty.prof = info.prof;
                playerEty.baseValueDic[ActorBaseTag.FightValue] = (ulong)info.fight;
                playerEty.baseValueDic[ActorBaseTag.Level] = (ulong)info.lev;
                lists.Add(new OtherPlayerInfo(playerEty));
            }
        }
        return lists;
    }
    /// <summary>
    /// 显示选中的化身在多人准备界面
    /// </summary>
    public void AddInvitationToCopyTeams(List<int> uIds)
    {
        for (int j = 0; j < uIds.Count; j++)
        {
            copyTeams[uIds[j]] = new CopySceneTeamPlayerInfo(uIds[j], 1, true);
        }
        List<int> uIdall = new List<int>();
        foreach (CopySceneTeamPlayerInfo data in copyTeams.Values)
        {
            if (data.isAvatar)
            {
                uIdall.Add(data.pId);
            }
        }
        C2S_InCopyInvitationPlayer(uIdall, true);
        if (OnSelectChange != null) OnSelectChange();
    }
    /// <summary>
    /// 移除化身数据
    /// </summary>
    public void ReMoveInvitationToCopyTeams(int uId)
    {
        CopySceneTeamPlayerInfo pInfo = null;
        if (copyTeams.TryGetValue(uId, out pInfo))
        {
            if (pInfo.isAvatar) copyTeams.Remove(uId);
        }
        List<int> uIds = new List<int>();
        uIds.Add(uId);
        C2S_InCopyInvitationPlayer(uIds, false);
        if (OnSelectChange != null) OnSelectChange();
    }

    void ClearInviTations()
    {
        List<OtherPlayerInfo> clearIds = new List<OtherPlayerInfo>();
        for (int i = 0; i < openInvitationPlayerData.Count; i++)
        {
            if (!copyTeams.ContainsKey(openInvitationPlayerData[i].ServerInstanceID))
            {
                clearIds.Add(openInvitationPlayerData[i]);
            }
        }
        for (int i = 0; i < clearIds.Count; i++)
        {
            openInvitationPlayerData.Remove(clearIds[i]);
        }
    }

    int againCopyID = 0, againSceneID = 0;
    /// <summary>
    /// 单人副本再次挑战
    /// </summary>
    public void AgainToCopyItem()
    {

        if (copyDic.ContainsKey(againCopyID) && copyDic[againCopyID].num > 0)
        {
            C2S_ToCopyItem(againCopyID, againSceneID);
        }
        else if(copyDic.ContainsKey(againCopyID))
        {
            CopyInItemDataInfo _serData = copyDic[againCopyID];
            CopyGroupRef refdata = ConfigMng.Instance.GetCopyGroupRef(_serData.id);
            if (_serData.buyNum > 0)//可购买次数大于0，弹出快捷购买次数
            {
                //Debug.Log("快捷购买");
                //CopyGroupRef copyGroupRef =
                int needGold = GetDiamoNum(_serData.buyNum, refdata);
                MessageST msg = new MessageST();
                msg.messID = 530;
                msg.words = new string[1] { needGold.ToString() };
                msg.delYes = delegate
                {
                    //Debug.Log("lcopyGroupRef.id" + GameCenter.duplicateMng.lcopyGroupRef.id);
                    //元宝是否充足
                    if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount >= (ulong)needGold)
                    {


                             C2S_BuyCopyInItem(refdata.id, 1);
                             C2S_ToCopyItem(againCopyID, againSceneID);
                    }
                    else
                    {
                        MessageST mst = new MessageST();
                        mst.messID = 137;
                        mst.delYes = delegate
                        {
                            // 跳转到充值界面
                            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
                        };
                        GameCenter.messageMng.AddClientMsg(mst);
                    }
                };
                GameCenter.messageMng.AddClientMsg(msg);
            }
            else
            {

                GameCenter.messageMng.AddClientMsg(167);
            }

        }
        else
        {
            Debug.Log("副本字典中不包含ID为："+ againCopyID+"副本数据");
        }
    }
    /// <summary>
    /// 多人准备队伍解散
    /// </summary>
    public void TeamDissolve()
    {
        CopyTeams.Clear();
        if (OnCopyItemUIClose != null) OnCopyItemUIClose();
    }
    /// <summary>
    /// 多人准备队员退出
    /// </summary>
    public void TeammateOut(int _teammateId)
    {
        if (CopyTeams.ContainsKey(_teammateId))
        {
            CopyTeams.Remove(_teammateId);
            if (OnSelectChange != null) OnSelectChange();
        }
    }

    #region  协议接受
    //打开镇魔塔准备界面
    void S2C_MagicTowers(Pt _info)
    {
        pt_quell_demon_win_d716 info = _info as pt_quell_demon_win_d716;
        if (info != null)
        {
            if (OnMagicTowerUIOpen != null) OnMagicTowerUIOpen(info.integral,info.time);
        }
    }
    /// <summary>
    /// 副本数据
    /// </summary>
    void S2C_CopyItemList(Pt _info)
    {
        //Debug.logger.Log("S2C_CopyItemList");
        pt_single_many_copy_info_d453 info = _info as pt_single_many_copy_info_d453;
        if (info == null) return;
        for (int i = 0; i < info.single_many_list.Count; i++)
        {
            st.net.NetBase.single_many_list data = info.single_many_list[i];
            //Debug.Log("副本ID  " + data.copy_id +"副本剩余挑战次数  " + data.challenge_num + "可购买次数  " + data.buy_num);
            if (copyDic.ContainsKey((int)data.copy_id))
            {
                copyDic[(int)data.copy_id].UpdateData(data);
            }
            else
            {
                //                Debug.logger.Log("add dungeon " + data.copy_id);
                copyDic[(int)data.copy_id] = new CopyInItemDataInfo(data);
            }
        }
        FDictionary copyGroupRefTable = ConfigMng.Instance.CopyGroupRefTable();
        foreach (CopyGroupRef refData in copyGroupRefTable.Values)
        {
            if (!copyDic.ContainsKey(refData.id))
            {
                copyDic[refData.id] = new CopyInItemDataInfo(refData.id);
            }
        }
        SetAllCopyRedPoint();
        if (OnCopyTypeChange != null) OnCopyTypeChange();
    }
    /// <summary>
    /// 副本购买次数
    /// </summary>
    void S2C_BuyCopyInNum(Pt _info)
    {
        pt_update_single_num_d455 info = _info as pt_update_single_num_d455;
        if (info == null) return;
        if (copyDic.ContainsKey(info.copy_id))
        {
            copyDic[info.copy_id].UpdateData(info.copy_id, info.challenge_num, info.buy_num);
        }
        else
        {
            copyDic[info.copy_id] = new CopyInItemDataInfo();
            copyDic[info.copy_id].UpdateData(info.copy_id, info.challenge_num, info.buy_num);
        }
        SetAllCopyRedPoint();
        if (OnCopyItemChange != null) OnCopyItemChange();
    }
    /// <summary>
    /// 副本星级
    /// </summary>
    void S2C_CopyInSceneStar(Pt _info)
    {
        pt_uptate_single_many_star_d457 info = _info as pt_uptate_single_many_star_d457;
        if (info == null) return;
        if (copyDic.ContainsKey(info.copy_id))
        {
            if (copyDic[info.copy_id].copyScenes.ContainsKey(info.scene_type))
            {
                copyDic[info.copy_id].copyScenes[info.scene_type].UpdateData(info.star_num);
            }
            else
            {
                copyDic[info.copy_id].copyScenes[info.scene_type] = new CopyInItemDataInfo.CopySceneData(info.copy_id, info.star_num);
            }
        }
        if (OnCopyItemChange != null) OnCopyItemChange();
    }

    /// <summary>
    /// 通知队员化身进入多人准备
    /// </summary>
    void S2C_TeamAddInvitationPlayer(Pt _info)
    {
        pt_update_recruit_robot_to_member_d771 info = _info as pt_update_recruit_robot_to_member_d771;
        //Debug.Log("pt_update_recruit_robot_to_member_d771   " + info.recruit_uid_list.Count);
        if (info == null) return;
        int len = info.recruit_uid_list.Count;
        if (len <= 0)
        {
            List<int> uIdall = new List<int>();
            foreach (CopySceneTeamPlayerInfo data in copyTeams.Values)
            {
                if (data.isAvatar)
                {
                    uIdall.Add(data.pId);
                }
            }
            for (int j = 0; j < uIdall.Count; j++)
            {
                copyTeams.Remove(uIdall[j]);
            }
        }
        for (int j = 0; j < len; j++)
        {
            copyTeams[info.recruit_uid_list[j]] = new CopySceneTeamPlayerInfo(info.recruit_uid_list[j], 1, true);
        }
        if (OnSelectChange != null) OnSelectChange();
    }

    //待招募化身数据列表返回
    void S2C_InvitationPlayer(Pt _info)
    { 
        pt_update_recruit_robot_list_d746 info = _info as pt_update_recruit_robot_list_d746;
        if (info == null) return;
        ClearInviTations();
        OtherPlayerData other = null;
        for (int i = 0; i < info.recruit_robot_list.Count; i++)
        {
            st.net.NetBase.recruit_robot_list data = info.recruit_robot_list[i];
            other = new OtherPlayerData();
            other.serverInstanceID = data.uid;
            other.name = data.name;
            other.prof = data.prof;
            other.baseValueDic[ActorBaseTag.Level] = (ulong)data.lev;
            other.baseValueDic[ActorBaseTag.FightValue] = (ulong)data.fight_score;
            OtherPlayerInfo otherInfo = new OtherPlayerInfo(other);
            if (!openInvitationPlayerData.Contains(otherInfo)) openInvitationPlayerData.Add(otherInfo);
        }
        if (OpenInvitationPlayer != null && GameCenter.teamMng.isInTeam) OpenInvitationPlayer();
    }

    /// <summary>
    /// 队员进入多人准备
    /// </summary>
    void S2C_CopyItemTeams(Pt _info)
    {
        //Debug.Log("队友进入多人准备");
        //		copyTeams.Clear();
        pt_many_copy__member_challengenum_d460 info = _info as pt_many_copy__member_challengenum_d460;
        //Debug.Log("队友进入多人准备  pt_many_copy__member_challengenum_d460  " + info.member_challengenum.Count);
        if (info == null) return;
        bool isInvatide = false;//请求队友是否进入多人组队
        if (!copyDic.ContainsKey(GameCenter.curMainPlayer.id))//如果我还没有进入则弹出邀请界面
        {
            isInvatide = true; 
        }
        if (copyDic.ContainsKey(info.copy_id))
        {
            copyDic[info.copy_id].curCopyScene = info.copy_type;
        }
        else
        {
            copyDic[info.copy_id] = new CopyInItemDataInfo();
            copyDic[info.copy_id].id = info.copy_id;
            copyDic[info.copy_id].curCopyScene = info.copy_type;
        }
        for (int i = 0; i < info.member_challengenum.Count; i++)
        {
            st.net.NetBase.member_challengenum_list data = info.member_challengenum[i];
            if (copyTeams.Count >= 3) continue;
            if (!copyTeams.ContainsKey((int)data.uid))
            {
                copyTeams[(int)data.uid] = new CopySceneTeamPlayerInfo(data);
            }
            else
            {
                copyTeams[(int)data.uid].pNum = (int)data.challenge_num;
                copyTeams[(int)data.uid].isPerpare = data.prepare == 1;
            }
        }
        SetAllCopyRedPoint();
        if (isInvatide && OnCopyItemTeamData != null) OnCopyItemTeamData(info.copy_id);
        if (OnCopyItemChange != null) OnCopyItemChange();
    }
    /// <summary>
    /// 队员多人准备关闭
    /// </summary>
    void S2C_ReqCopyInTeamClose(Pt _info)
    {
        //pt_update_quit_many_copy_ui_d465 info = _info as pt_update_quit_many_copy_ui_d465;
        GameCenter.teamMng.InvitationTeammateOut();
        copyTeams.Clear();
        if (OnCopyItemUIClose != null)
        {
            OnCopyItemUIClose();
            GameCenter.uIMng.ReleaseGUI(GUIType.COPYMULTIPLEWND);
        }
    }
    /// <summary>
    /// 副本难度变更
    /// </summary>
    void S2C_ReqCopyInChange(Pt _info)
    {
        pt_update_many_copy_difficulty_d463 info = _info as pt_update_many_copy_difficulty_d463;
        if (copyDic.ContainsKey((int)info.copy_id))
        {
            copyDic[(int)info.copy_id].curCopyScene = (int)info.copy_type;
        }
        if (OnCopyItemChange != null) OnCopyItemChange();
    }
    /// <summary>
    /// 队员是否准备
    /// </summary>
    void S2C_ReqCopyInPerpare(Pt _info)
    {
        //Debug.Log("是否准备");
        pt_update_prepare_state_d466 info = _info as pt_update_prepare_state_d466;
        for (int i = 0; i < info.update_prepare.Count; i++)
        {
            if (copyTeams.ContainsKey((int)info.update_prepare[i].uid)) copyTeams[(int)info.update_prepare[i].uid].UpdateData(info.update_prepare[i].uid, info.update_prepare[i].prepare);
        }
        if (OnCopyItemChange != null) OnCopyItemChange();
    }
    // 邀请好友询问队列
    static List<int> friendmsg = new List<int>();

    /// <summary>
    /// 邀请好友询问
    /// </summary>
    void S2C_CopyInFriend(Pt _info)
    {
        Debug.Log("邀请询问：S2C_CopyInFriend");
        pt_ans_recruit_friend_d749 info = _info as pt_ans_recruit_friend_d749;
        if (info == null) return;
        if (GameCenter.teamMng.isInTeam) return;
        if (friendmsg.Contains(info.uid))
        {
            return;
        }
        friendmsg.Add(info.uid);
        MessageST msg = new MessageST();
        msg.messID = 251;
        msg.words = new string[3] { info.name, ConfigMng.Instance.GetLevelDes(info.lev), info.fight_score.ToString() };
        msg.delYes = delegate {
            C2S_CopyInFriendReturn(info.uid, 1);
        };
        msg.delNo = delegate {
            C2S_CopyInFriendReturn(info.uid, 0);
        };
        GameCenter.messageMng.AddClientMsg(msg);
    }
    //void S2C_ReqOpenCoppyTime(Pt _info)
    //{
    //    //pt_copy_pick_item_time_d753 info = _info as pt_copy_pick_item_time_d753;

    //    if (OnOpenCoppyTime != null) OnOpenCoppyTime();
    //}

    //void S2C_ReqOpenCopyForce(Pt _info)
    //{
    //    //		pt_copy_loser_d470 info = new pt_copy_loser_d470();
    //    openCopyForceTip = true;
    //    if (OnOpenCopyForce != null) OnOpenCopyForce();
    //}
    #endregion

    #region 协议发送

    /// <summary>
    /// 多人准备好友邀请回应
    /// </summary>
    public void C2S_CopyInFriendReturn(int _pid, int _answer)
    {
        Debug.Log("多人准备好友邀请回应:C2S_CopyInFriendReturn");
        if (GameCenter.teamMng.isInTeam)
        {
            GameCenter.messageMng.AddClientMsg(404);
            return;
        }  
        pt_ask_recruit_friend_reply_d750 info = new pt_ask_recruit_friend_reply_d750();
        info.ans_uid = _pid;
        info.ans_state = _answer;
        NetMsgMng.SendMsg(info);
        if (friendmsg.Contains(_pid)) friendmsg.Remove(_pid);
    }

    /// <summary>
    /// 多人准备好友邀请
    /// </summary>
    public void C2S_ReqCopyInFriend(int uid)
    {
        //Debug.Log("多人准备好友邀请：C2S_ReqCopyInFriend");
        if (copyTeams.Count >= 3)
        {
            GameCenter.messageMng.AddClientMsg(84);
            return;
        }

        pt_req_recruit_friend_d747 info = new pt_req_recruit_friend_d747();
        info.oth_uid = uid;
        NetMsgMng.SendMsg(info);
    }

    /// <summary>
    /// 多人准备
    /// </summary>
    public void C2S_ReqCopyInPerpare(uint perpare)
    {
        pt_member_click_prepare_d461 info = new pt_member_click_prepare_d461();
        info.state = perpare;
        NetMsgMng.SendMsg(info);
    }
    /// <summary>
    /// 多人副本组里副本改变
    /// </summary>
    public void C2S_ReqCopyInChange(int id, int type)
    {
        pt_change_many_copy_difficulty_d462 info = new pt_change_many_copy_difficulty_d462();
        info.copy_id = (uint)id;
        info.copy_type = (uint)type;
        NetMsgMng.SendMsg(info);
    }
    /// <summary>
    /// 多人准备界面关闭(0为队员点击关闭 ，1为队长点击关闭)
    /// </summary>
    public void C2S_ReqCopyInTeamClose(int _state)
    {
        pt_req_quit_many_copy_ui_d464 info = new pt_req_quit_many_copy_ui_d464();
        info.state = _state;
        NetMsgMng.SendMsg(info);
    }
    /// <summary>
    /// 多人进副本或者镇魔塔
    /// </summary>
    public void C2S_ReqCopyInTeamData(int id, int type)
    {
        copyTeams.Clear();
        pt_req_team_many_copy_d459 info = new pt_req_team_many_copy_d459();
        info.copy_id = id;
        info.copy_type = type;
        NetMsgMng.SendMsg(info);
    }
    /// <summary>
    /// 副本数据
    /// </summary>
    public void C2S_ReqCopyItemList()
    {
        //Debug.logger.Log("C2S_ReqCopyItemList");
        pt_req_single_many_info_d452 info = new pt_req_single_many_info_d452();
        NetMsgMng.SendMsg(info);
    }

    /// <summary>
    /// 进副本
    /// </summary>
    public void C2S_ToCopyItem(int id, int type)
    {
        if (CopyType == CopysType.ONESCOPY)
        {
            againCopyID = id;
            againSceneID = type;
        }
        pt_req_fly_single_many_copy_d456 info = new pt_req_fly_single_many_copy_d456();
        info.seq = NetMsgMng.CreateNewSerializeID();
        info.copy_id = id;
        info.scene_type = type;
        NetMsgMng.SendMsg(info);
    }
    /// <summary>
    /// 购买副本次数
    /// </summary>
    public void C2S_BuyCopyInItem(int id, int num)
    {
        pt_req_buy_single_num_d454 info = new pt_req_buy_single_num_d454();
        info.copy_id = id;
        info.buy_num = num;
        NetMsgMng.SendMsg(info);
    }
    /// <summary>
    /// 招募化身
    /// </summary>
    public void C2S_InvitationPlayer()
    {
        pt_recruit_robot_info_d744 info = new pt_recruit_robot_info_d744();
        NetMsgMng.SendMsg(info);
    }

    /// <summary>
    /// 选中的化身
    /// </summary>
    public void C2S_InCopyInvitationPlayer(List<int> uIds, bool isAdd)
    {
        pt_recruit_robot_member_d745 info = new pt_recruit_robot_member_d745();
        info.add_or_remove = isAdd ? 0 : 1;
        info.recruit_robot_member = uIds;
        NetMsgMng.SendMsg(info);
    }
    /// <summary>
    /// 镇魔塔发送世界招募信息
    /// </summary>
    public void C2S_ReqWoldRecruit()
    {
        pt_leader_req_world_recruit_d790 info = new pt_leader_req_world_recruit_d790();
        NetMsgMng.SendMsg(info);
    }
    #endregion
    #endregion
    #region 副本结算
    /// <summary>
    /// 副本失败
    /// </summary>
    public bool openCopyForceTip = false;
    /// <summary>
    /// 副本失败事件
    /// </summary>
    public System.Action OnOpenCopyForce;
    /// <summary>
    /// 打开结算
    /// </summary>
    public System.Action OnOpenCopySettlement;
    /// <summary>
    /// 打开结算
    /// </summary>
    public System.Action OnOpenCopySettlementFlop;
    /// <summary>
    /// 打开结算
    /// </summary>
    public System.Action OnOpenArenaSettlement;

    /// <summary>
    /// 结算前等待捡东西
    /// </summary>
    public System.Action OnOpenCoppyTime;
    /// <summary>
    /// 结算数据
    /// </summary>
    public CopySettlementDataInfo CopySettlementDataInfo = new CopySettlementDataInfo();
    /// <summary>
    /// 时间格式
    /// </summary>
    public string ItemTime(int time, bool isNow = false)
    {
        if (isNow)
        {
            DateTime date = GameHelper.ToChinaTime(new DateTime(1970, 1, 1)).AddSeconds((double)time);
            //TimeSpan sp = DateTime.Now - date;
            TimeSpan sp = GameCenter.instance.CurServerTime - date;
            if (sp.Days > 7)
            {
                return ConfigMng.Instance.GetUItext(41);
            }
            else if (sp.Days >= 1 && sp.Days <= 7)
            {
                return ConfigMng.Instance.GetUItext(40, new string[1] { sp.Days.ToString() });
            }
            else if (sp.Hours >= 1 && sp.Days < 1)
            {
                return ConfigMng.Instance.GetUItext(39, new string[1] { sp.Hours.ToString() });
            }
            else if (sp.Minutes >= 1)
            {
                return ConfigMng.Instance.GetUItext(38, new string[1] { sp.Minutes.ToString() });
            }
            else if (sp.Seconds < 60)
            {
                return ConfigMng.Instance.GetUItext(37);
            }
            return string.Empty;
        }

        int tmp = time;
        int s = tmp % 60;
        tmp /= 60;
        int m = tmp % 60;
        int h = tmp / 60;
        if (h > 0)
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
        }
        else
        {
            return string.Format("{0:D2}:{1:D2}", m, s);
        }
        //return "0:0";
    }

    #region 协议
    void S2C_ReqArenaRewardData(Pt _info)
    {
        pt_pk_win_d491 info = _info as pt_pk_win_d491;
        CopySettlementDataInfo = new CopySettlementDataInfo();
        CopySettlementDataInfo.state = info.state;
        CopySettlementDataInfo.rank = info.rank;
        CopySettlementDataInfo.upRank = info.up_rank;
        CopySettlementDataInfo.showKo = info.cd_state <= 0;
        for (int i = 0; i < info.reward.Count; i++)
        {
            st.net.NetBase.reward_list data = info.reward[i];
            CopySettlementDataInfo.items.Add(new EquipmentInfo((int)data.type, (int)data.num, EquipmentBelongTo.PREVIEW));
        }
        if (OnOpenArenaSettlement != null) OnOpenArenaSettlement();
    }

    void S2C_ReqOpenCoppyTime(Pt _info)
    {
        //pt_copy_pick_item_time_d753 info = _info as pt_copy_pick_item_time_d753;

        if (OnOpenCoppyTime != null) OnOpenCoppyTime();
    }

    void S2C_ReqOpenCopyForce(Pt _info)
    {
        //		pt_copy_loser_d470 info = new pt_copy_loser_d470();
        openCopyForceTip = true;
        if (OnOpenCopyForce != null)
        {
            OnOpenCopyForce();
            GameCenter.uIMng.SwitchToUI(GUIType.FORCE);
        }        
    }

    void S2C_ReqSettlementRewardData(Pt _info)
    {
        pt_win_list_d467 info = _info as pt_win_list_d467;
        CopySettlementDataInfo = new CopySettlementDataInfo();
        CopySettlementDataInfo.star = info.star_num;
        CopySettlementDataInfo.time = info.time;
        CopySettlementDataInfo.bossCount = info.kill_boss_num;
        CopySettlementDataInfo.coppyId = info.scene_type;//用于多人副本
        for (int i = 0; i < info.reward_list.Count; i++)
        {
            st.net.NetBase.reward_list data = info.reward_list[i];
            CopySettlementDataInfo.items.Add(new EquipmentInfo((int)data.type, (int)data.num, EquipmentBelongTo.PREVIEW));
        }
        for (int i = 0; i < info.team_reward_list.Count; i++)//多人副本非化身奖励
        {
            st.net.NetBase.team_reward_list data = info.team_reward_list[i];
            CopySettlementDataInfo.teamItems.Add(new EquipmentInfo((int)data.type, (int)data.num, EquipmentBelongTo.PREVIEW));
        }
        if (OnOpenCopySettlement != null) OnOpenCopySettlement();
    }

    void S2C_ReqSettlementFlopData(Pt _info)
    {
        pt_lucky_brand_list_d469 info = _info as pt_lucky_brand_list_d469;
        CopySettlementDataInfo = new CopySettlementDataInfo();
        CopySettlementDataInfo.clickFlop = info.bradn_id;

        CopySettlementDataInfo.flopItems[(int)info.bradn_id] = new EquipmentInfo((int)info.brand_reward[0].type, (int)info.brand_reward[0].num, EquipmentBelongTo.PREVIEW);
        int id = 1;
        for (int i = 0; i < info.lucky_brand.Count; i++)
        {
            st.net.NetBase.lucky_brand_list data = info.lucky_brand[i];
            if (id == info.bradn_id)
            {
                id++;
            }
            CopySettlementDataInfo.flopItems[id] = new EquipmentInfo((int)data.type, (int)data.num, EquipmentBelongTo.PREVIEW);
            id++;
        }
        if (OnOpenCopySettlementFlop != null) OnOpenCopySettlementFlop();
    }

    /// <summary>
    /// 获取镇魔塔挑战次数
    /// </summary> 
    void S2C_GetTowerChallengeNum(Pt _info)
    {
        pt_quell_demon_info_c147 pt = _info as pt_quell_demon_info_c147;
        if(pt != null)
        { 
            isChallengeMagicTower = pt.challenge_state == 1;
            if (OnMagicChallengeUpdate != null) OnMagicChallengeUpdate();
        }
    }

    /// <summary>
    /// 结算开牌
    /// </summary>
    public void C2S_ReqSettlementFlop(int brandId)
    {
        pt_req_open_lucky_brand_d468 info = new pt_req_open_lucky_brand_d468();
        info.brand_id = brandId;
        NetMsgMng.SendMsg(info);
    }
    /// <summary>
    /// 通知服务端东西捡完了
    /// </summary>
    public void C2S_CoppyOver()
    {
        pt_req_item_pick_over_d754 info = new pt_req_item_pick_over_d754();
        NetMsgMng.SendMsg(info);
    }
    /// <summary>
    /// 退出
    /// </summary>
    public void C2S_OutCopy()
    {
        ////如果结算副本是无尽试炼则打开一下无尽试炼窗口
        //if (GameCenter.endLessTrialsMng.OpenEndless)
        //{
        //    GameCenter.uIMng.SwitchToUI(GUIType.ENDLESSWND);
        //    GameCenter.endLessTrialsMng.OpenEndless = false;
        //}
        GameCenter.teamMng.InvitationTeammateOut();
        pt_req_copy_out_d471 info = new pt_req_copy_out_d471();
        NetMsgMng.SendMsg(info);
    }

    public void OutCopyWnd()
    {
        MessageST mst = new MessageST();
        mst.messID = 44;
        mst.delYes = (x) =>
        {
            C2S_OutCopy();
        };
        GameCenter.messageMng.AddClientMsg(mst);
    }

    /// <summary>
    /// 请求镇魔塔是否有挑战次数
    /// </summary>
    public void C2S_GetTowerChallengeNum()
    {
        pt_req_quell_demon_info_c146 info = new pt_req_quell_demon_info_c146();
        NetMsgMng.SendMsg(info);
    }
    #endregion
    #region 暂停功能
    const int OneCopyGameStopTotalNum = 3;//单人副本暂停总次数
                                          /// <summary>
                                          /// 单人副本暂停次数
                                          /// </summary>
    public int gameStopNum = 0;
    /// <summary>
    /// 暂停事件
    /// </summary>
    public System.Action OnGameStop;
    public bool IsGameStop = false;
    void S2C_GameStop(Pt _info)
    {
        pt_copy_pause_d47a info = _info as pt_copy_pause_d47a;
        IsGameStop = info.state == 1;
        Time.timeScale = info.state == 1 ? 0 : 1;
        if (CopyType == CopysType.ONESCOPY && info.state == 0) gameStopNum++;
        if (OnGameStop != null) OnGameStop();
    }
    /// <summary>
    /// 请求暂停
    /// </summary>
    public void C2S_GameStop(byte stege)
    {
        if (gameStopNum >= OneCopyGameStopTotalNum && CopyType == CopysType.ONESCOPY)
        {
            GameCenter.messageMng.AddClientMsg(317);
            return;
        }
        pt_copy_pause_d47a info = new pt_copy_pause_d47a();
        info.state = stege;
        NetMsgMng.SendMsg(info);
    } 
    #endregion
    #endregion
}
/// <summary>
/// 副本类型
/// </summary>
public enum OneCopySType
{
    NONE = 0,
    /// <summary>
    /// 奈何桥
    /// </summary>
    NAIHEQIAO = 1,
    /// <summary>
    /// 兜率仙宫
    /// </summary>
    DOUSHUAIXIANGONG = 2,
    /// <summary>
    /// 五庄观
    /// </summary>
    WUZHUANGGUAN = 3,
    /// <summary>
    /// 真假美猴王
    /// </summary>
    MONKEY = 5,
    /// <summary>
    /// 白骨寒狱
    /// </summary>
    BAIGUHANYU = 6,
    /// <summary>
    /// 多人副本奈何桥
    /// </summary>
    TWONAIHEQIAO = 7,
    /// <summary>
    /// 多人副本白骨寒狱
    /// </summary>
    TWOBAIGUHANYU = 8,
}
