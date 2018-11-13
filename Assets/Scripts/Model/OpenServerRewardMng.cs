//==============================================
//作者：朱素云
//日期：2016/7/14
//用途：开服贺礼管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class OpenServerRewardMng
{ 
    private OpenServerRewardInfoData serverData = null;
    public OpenServerRewardInfoData ServerData
    {
        get
        {
            return serverData;
        }
    }



    public Action OnGetOpenGiftResult;
    public Action OnGetAllOpenServerInfo;

    /// <summary>
    /// 开服贺礼的活动是否打开
    /// </summary>
    public bool CanOpen = false; 

    public bool isAccord=false;
    protected OpenServerType curOpenType = OpenServerType.none;
    public OpenServerType curOpenServerType
    {
        get
        {
            return curOpenType;
        }
        set
        {
            if (curOpenType != value)
            {
                curOpenType = value;
                if (OnCurOpenTypeUpdate != null)
                {
                    OnCurOpenTypeUpdate();
                }
            }
        }
    }
    public System.Action OnCurOpenTypeUpdate;

    #region 构造
    /// <summary>
    /// 返回该管理类的唯一实例
    /// </summary>
    /// <returns></returns>
    public static OpenServerRewardMng CreateNew(MainPlayerMng _main)
    {
        if (_main.openServerRewardMng == null)
        {
            OpenServerRewardMng OpenServerRewardMng = new OpenServerRewardMng();
            OpenServerRewardMng.Init(_main);
            return OpenServerRewardMng;
        }
        else
        {
            _main.openServerRewardMng.UnRegist(_main);
            _main.openServerRewardMng.Init(_main);
            return _main.openServerRewardMng;
        }
    }
    /// <summary>
    /// 注册
    /// </summary>
    protected virtual void Init(MainPlayerMng _main)
    { 
        MsgHander.Regist(0xD911, S2C_GetAllOpenServerRewardInfo);
        MsgHander.Regist(0xD913, S2C_GetOpenServerRewardResult);
        MsgHander.Regist(0xD961, S2C_GetLotteryInfo);
        MsgHander.Regist(0xD963, S2C_GetLotteryRecord);
        MsgHander.Regist(0xD965, S2C_GetLotteryResult);
        MsgHander.Regist(0xC128, S2C_GetTarotInfo);
        MsgHander.Regist(0xC129, S2C_GetTarotReward);
        MsgHander.Regist(0xD990, S2C_GetDailyFirstRechargeInfo);
        MsgHander.Regist(0xD991, S2C_CloseDailyFirstRecharge);
        MainPlayerMng.OnCreateNew += FirstIn; 
        //if (!PlayerPrefs.HasKey("LAST_TIME"))
        //{
        //    DateTime lastDate = DateTime.Now;
        //    string str = lastDate.Year + "/" + lastDate.Month + "/" + lastDate.Day + GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID;
        //    PlayerPrefs.SetString("LAST_TIME", str);
        //    isTodayFirstLogin = true;
        //}
        //else
        //{
        //    string time = PlayerPrefs.GetString("LAST_TIME");
        //    Debug.Log("############################     " + time.ToString());
        //    string[] day = time.Split('/');
        //    int[] days = new int[day.Length];
        //    for (int i = 0, max = day.Length; i < max; i++)
        //    {
        //        int.TryParse(day[i], out days[i]); 
        //    }
        //    DateTime lastDate = new DateTime(days[0], days[1], days[2]);
        //    DateTime date = DateTime.Now.Date; 
        //    TimeSpan ts = date - lastDate;
        //    if (ts.TotalDays >= 1)
        //    {  
        //        isTodayFirstLogin = true;
        //        string str = date.Year + "/" + date.Month + "/" + date.Day + GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID; ;
        //        //Debug.Log("TotalDays >= 1   " + str);
        //        PlayerPrefs.SetString("LAST_TIME", str);
        //    }
        //}
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected virtual void UnRegist(MainPlayerMng _main)
    {

        //GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= ChangeAutoUseSkill;
        MsgHander.UnRegist(0xD911, S2C_GetAllOpenServerRewardInfo);
        MsgHander.UnRegist(0xD913, S2C_GetOpenServerRewardResult);
        MsgHander.UnRegist(0xD961, S2C_GetLotteryInfo);
        MsgHander.UnRegist(0xD963, S2C_GetLotteryRecord);
        MsgHander.UnRegist(0xD965, S2C_GetLotteryResult);
        MsgHander.UnRegist(0xC128, S2C_GetTarotInfo);
        MsgHander.UnRegist(0xC129, S2C_GetTarotReward);
        MsgHander.UnRegist(0xD990, S2C_GetDailyFirstRechargeInfo);
        MsgHander.UnRegist(0xD991, S2C_CloseDailyFirstRecharge);
        GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= RefreshOpen;
        MainPlayerMng.OnCreateNew -= FirstIn; 
        CanOpen = false;
        serverData = null;
        isAccord = false;
        lotteryData = null;
        lottryResult.Clear();
        lottryRecord.Clear();
        isRotateOver = false;
        curOpenServerType = OpenServerType.none;
        isRotating = false; 
        taroatRewards.Clear();
        wdfTaroatData = null;
        rebackPercent = 0;
        reminTime = 0; 
        isOpenFirstRecharge = false;
    }

    void SetOpenServerceOpen()
    { 
         
    }

    #endregion

    #region 通信S2C



    /// <summary>
    /// 获得所有开服贺礼的数据
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetAllOpenServerRewardInfo(Pt _pt)
    {
        pt_reply_open_server_gift_info_d911 pt = _pt as pt_reply_open_server_gift_info_d911;
        if (pt != null && CanOpen)
        {
            if (pt.rest_time <= 0 || pt.wares.Count < 1)
            { 
                serverData = null; 
                //if (isAccord && lotteryData == null)
                //{
                //    GameCenter.wdfActiveMng.isGiftOpen = false;
                //    GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.OPENSERVER, false);
                //    if (GameCenter.uIMng.CurOpenType == GUIType.OPENSERVER)
                //    {
                //        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                //    }
                //    GameCenter.messageMng.AddClientMsg(427);
                //}
            }
            else
            {
                //Debug.Log("获得开服贺礼的数据 ");
                serverData = new OpenServerRewardInfoData(pt);
                GameCenter.wdfActiveMng.isGiftOpen = true;
                if (OnGetAllOpenServerInfo != null)
                    OnGetAllOpenServerInfo();
            }
            SetOpenServerOpenState();
        } 
    }

    /// <summary>
    /// 获得领取奖励结果
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetOpenServerRewardResult(Pt _pt)
    {
       // Debug.Log("S2C_GetOpenServerRewardResult");
        pt_reply_buy_open_server_gift_d913 pt = _pt as pt_reply_buy_open_server_gift_d913;
        if (pt != null)
        {
            //Debug.Log("获得领奖结果为"+pt.result);
            if (OnGetOpenGiftResult != null)
                OnGetOpenGiftResult();
            if (pt.result == 1)
                C2S_AskAllOpenServerRewardInfo();
        }

    }









    #endregion

    #region C2S
    /// <summary>
    /// 请求所有开服贺礼数据
    /// </summary>
    /// <param name="num"></param>
    public void C2S_AskAllOpenServerRewardInfo()
    {
        //Debug.Log("请求所有开服贺礼数据 ");
        pt_req_open_server_gift_info_d910 msg = new pt_req_open_server_gift_info_d910();
        NetMsgMng.SendMsg(msg); 
    }

    /// <summary>
    /// 购买对应ID的开服贺礼礼包
    /// </summary>
    /// <param name="num"></param>
    public void C2S_AskBuyOpenServerReward(int _id,int _num)
    {
        pt_req_buy_open_server_gift_d912 msg = new pt_req_buy_open_server_gift_d912();
        msg.ware_id = (uint)_id;
        msg.amount = (uint)_num;
        NetMsgMng.SendMsg(msg);
        //Debug.Log("请求购买ID为" + _id + "数量为" + msg.amount);
    }





    #endregion

    #region 抽奖活动
    protected bool isRotating = false;
    public bool isRotate
    {
        get
        {
            return isRotating;
        }
        set
        {
            if (isRotating != value)
                isRotating = value;
        }
    }
    /// <summary>
    /// 抽奖数据
    /// </summary>
    public WdfLotteryData lotteryData = null;
    /// <summary>
    /// 抽奖结果
    /// </summary>
    public List<lucky_wheel_reward_info> lottryResult = new List<lucky_wheel_reward_info>();
    public void ReSetResult()
    { 
        lottryResultOne.Clear();
        for (int i = 0; i < lottryResult.Count; i++)
        {
            lottryResultOne.Add(lottryResult[i]);
        }
        lottryResult.Clear();
    }
    public List<lucky_wheel_reward_info> lottryResultOne = new List<lucky_wheel_reward_info>();
    /// <summary>
    /// 抽奖记录
    /// </summary>
    public List<lucky_wheel_record> lottryRecord = new List<lucky_wheel_record>();
    public System.Action OnLotteryDataUpdate;
    public System.Action OnLotteryResultUpdate;
    public System.Action OnLotteryRecordUpdate; 
    protected bool isRotateOver = false;
    public bool IsRotateOver
    {
        set
        {
            if (isRotateOver != value)
            {
                isRotateOver = value;
                if (OnRotateOverUpdate != null) OnRotateOverUpdate();
            }
        }
        get
        {
            return isRotateOver;
        }
    }
    public System.Action OnRotateOverUpdate;
    #region S2C
    /// <summary>
    /// 获取转盘数据
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetLotteryInfo(Pt _pt)
    { 
        pt_lucky_wheel_info_d961 pt = _pt as pt_lucky_wheel_info_d961;
        if (pt != null)
        { 
            if (pt.rest_time <= 0)
            {
                lotteryData = null;
                //if (ServerData == null)
                //{
                //    GameCenter.wdfActiveMng.isGiftOpen = false;
                //    GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.OPENSERVER, false);
                //    if (GameCenter.uIMng.CurOpenType == GUIType.OPENSERVER)
                //    {
                //        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                //    }
                //    GameCenter.messageMng.AddClientMsg(427);
                //}
            }
            else
            {
                //Debug.Log("获取转盘数据 ");
                GameCenter.wdfActiveMng.isGiftOpen = true;
                if (lotteryData == null)
                {
                    lotteryData = new WdfLotteryData(pt);
                }
                else
                {
                    lotteryData.Update(pt);
                }
            }
            SetOpenServerOpenState();
            if (OnLotteryDataUpdate != null) OnLotteryDataUpdate();
        }
    }
    /// <summary>
    /// 获取抽奖记录
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetLotteryRecord(Pt _pt)
    {
        pt_lucky_wheel_record_d963 pt = _pt as pt_lucky_wheel_record_d963;
        if (pt != null)
        { 
            lottryRecord.Clear();
            lottryRecord = pt.record_list; 
            if (OnLotteryRecordUpdate != null) OnLotteryRecordUpdate();
        }
    }
    /// <summary>
    /// 获取抽奖结果
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetLotteryResult(Pt _pt)
    {
        pt_lucky_wheel_reward_d965 pt = _pt as pt_lucky_wheel_reward_d965;
        if (pt != null)
        { 
            lottryResult.Clear();
            lottryResult = pt.reward_list;
            if (lotteryData != null)
            {
                lotteryData.Update(pt); 
            }
            if (OnLotteryResultUpdate != null) OnLotteryResultUpdate();
        }
    }
    #endregion

    #region C2S
    /// <summary>
    /// 请求转盘数据
    /// </summary>
    public void C2S_AskLotteryInfo()
    { 
        pt_req_lucky_wheel_info_d960 msg = new pt_req_lucky_wheel_info_d960();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求抽奖记录
    /// </summary>
    public void C2S_AskLotteryRecord()
    { 
        pt_req_lucky_wheel_record_d962 msg = new pt_req_lucky_wheel_record_d962();
        NetMsgMng.SendMsg(msg);
    }
    public void AddDiamondRemind()
    {
        MessageST mst1 = new MessageST();
        mst1.messID = 137;
        mst1.delYes = delegate
        {
            GameCenter.uIMng.SwitchToUI(GUIType.RECHARGE);
        };
        GameCenter.messageMng.AddClientMsg(mst1);
    }
    /// <summary>
    /// 请求抽奖
    /// </summary>
    public void C2S_AskLottery(int _type)
    { 
        if (lotteryData != null)
        {
            if (_type == 1 && (int)GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < lotteryData.price1)
            {
                AddDiamondRemind();
                return;
            }
            if (_type == 2)
            {
                if (!lotteryData.isLotteriedTenTimes)
                {
                    if ((int)GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < lotteryData.price2 / 2)
                    {
                        AddDiamondRemind();
                        return;
                    }
                }
                else if ((int)GameCenter.mainPlayerMng.MainPlayerInfo.TotalDiamondCount < lotteryData.price2)
                {
                    AddDiamondRemind();
                    return;
                }
            }
        } 
        isRotate = true;
        pt_req_lucky_wheel_lottery_d964 msg = new pt_req_lucky_wheel_lottery_d964();
        msg.type = (byte)_type;
        NetMsgMng.SendMsg(msg);
    }
    #endregion

    #endregion


    #region 塔罗牌 
    public WdfTaroatData wdfTaroatData = null;
    public List<EquipmentInfo> taroatRewards = new List<EquipmentInfo>();
    public System.Action OnTarotDataUpdate;
    public System.Action OnTaroatRewardUpdate;
    #region S2C
    /// <summary>
    /// 获取塔罗牌数据
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetTarotInfo(Pt _pt)
    {
        pt_tarot_info_c128 pt = _pt as pt_tarot_info_c128;
        if (pt != null)
        {
            //Debug.Log(" 获取塔罗牌数据:  " + pt.surplus_times + "  ,num : " + pt.surplus_num);
            if (pt.surplus_num <= 0)
            {
                wdfTaroatData = null; 
            }
            else
            {
                GameCenter.wdfActiveMng.isGiftOpen = true;
                if (wdfTaroatData == null)
                {
                    wdfTaroatData = new WdfTaroatData(pt);
                }
                else
                {
                   wdfTaroatData.Update(pt);
                }
            }
            SetTaroatRedRemind();
            SetOpenServerOpenState();
            if (OnTarotDataUpdate != null) OnTarotDataUpdate();
        }
    }
    /// <summary>
    /// 获取塔罗牌奖励
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetTarotReward(Pt _pt)
    {
        pt_tarot_reward_list_c129 pt = _pt as pt_tarot_reward_list_c129;
        if (pt != null)
        {
            taroatRewards.Clear();
            //Debug.Log("获取塔罗牌奖励 :  " + pt.tarot_reward.Count);
            for (int i = 0, max = pt.tarot_reward.Count; i < max; i++)
            {
                taroatRewards.Add(new EquipmentInfo((int)pt.tarot_reward[i].type, (int)pt.tarot_reward[i].num, EquipmentBelongTo.PREVIEW));
            }
            if (OnTaroatRewardUpdate != null) OnTaroatRewardUpdate();
        }
    }
    #endregion

    #region C2S
    /// <summary>
    /// 1请求塔罗牌数据 2 请求领取塔罗牌奖励
    /// </summary>
    public void C2S_AskTaroatInfo(int _type) 
    {
        //Debug.Log(" C2S_AskTaroatInfo "+ _type);
        pt_req_tarot_c127 msg = new pt_req_tarot_c127();
        msg.req_type = (uint)_type;
        NetMsgMng.SendMsg(msg);
    } 

    #endregion
    #endregion

    #region 每日首充返利
     
    public int restTime = 0;
    /// <summary>
    /// 活动剩余时间
    /// </summary>
    public int reminTime
    {
        get
        {
            return restTime;
        }
        set
        {
            if (restTime != value)
            {
                restTime = value;
                if (OnDailyRechargeUpdate != null) OnDailyRechargeUpdate();
            }
        }
    }
    public System.Action OnDailyRechargeUpdate;
    /// <summary>
    /// 返利百分比
    /// </summary>
    public int rebackPercent = 0;
    /// <summary>
    /// 是否打开每日充值返利界面，在七天奖励，每日登陆之后打开
    /// </summary>
    public bool isOpenFirstRecharge = false;
     
    #region S2C
     /// <summary>
    /// 获取每日首充返利数据
    /// </summary> 
    protected void S2C_GetDailyFirstRechargeInfo(Pt _pt)
    {
        pt_daily_recharge_benifit_info_d990 pt= _pt as pt_daily_recharge_benifit_info_d990;
        if (pt != null)
        {
            reminTime = (int)pt.rest_time;
            rebackPercent = (int)pt.percent;
            //Debug.Log("isTodayFirstLogin : " + isTodayFirstLogin + "  , reminTime : " + reminTime + "  , rebackPercent : " + rebackPercent);
        }
    }

    /// <summary>
    /// 获取每日首充返利后活动结束
    /// </summary> 
    protected void S2C_CloseDailyFirstRecharge(Pt _pt)
    { 
        pt_daily_recharge_benifit_succ_d991 pt = _pt as pt_daily_recharge_benifit_succ_d991;
        if (pt != null)
        {
            reminTime = 0; 
        }
    }

    #endregion
     
    #endregion


    #region 辅助逻辑

    void FirstIn()
    {
        if (GameCenter.mainPlayerMng != null)
        {
            if (GameCenter.mainPlayerMng.MainPlayerInfo != null)
            {
                if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= 30)
                {
                    CanOpen = true;
                }
                else
                {
                    GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += RefreshOpen; 
                }
            }
        }
        C2S_AskAllOpenServerRewardInfo(); 
        C2S_AskLotteryInfo();
        C2S_AskTaroatInfo(1); 
    }

    //void RefreshOpen()
    //{
    //    if (lotteryData != null)
    //    {
    //        CanOpen = true;
    //    }
    //}

    void RefreshOpen(ActorBaseTag _act,ulong _long, bool _b)
    {
        if (_act == ActorBaseTag.Level)
        {
            if (GameCenter.mainPlayerMng != null)
            {
                if (GameCenter.mainPlayerMng.MainPlayerInfo != null)
                {
                    if (GameCenter.mainPlayerMng.MainPlayerInfo.CurLevel >= 30)
                    {
                        CanOpen = true;
                        C2S_AskAllOpenServerRewardInfo();
                    }
                }
            }
        }
    }

    void SetOpenServerOpenState()
    {
        if (serverData != null || lotteryData != null || wdfTaroatData != null)
        {
            GameCenter.wdfActiveMng.isGiftOpen = true;
        }
        else
        {
            GameCenter.wdfActiveMng.isGiftOpen = false;
            GameCenter.mainPlayerMng.SetServerActiveOpen(FunctionType.OPENSERVER, false);
            if (GameCenter.uIMng.CurOpenType == GUIType.OPENSERVER)
            {
                GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                GameCenter.messageMng.AddClientMsg(427);
            } 
        } 
    }
    /// <summary>
    /// 关闭塔罗牌活动
    /// </summary>
    public void CloseTaroatActive()
    {
        wdfTaroatData = null;
        SetOpenServerOpenState();
    }
 
    /// <summary>
    /// 设置塔罗牌活动红点(开服活动只有塔罗牌设有红点)
    /// </summary>
    public void SetTaroatRedRemind()
    {
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.OPENSERVER, isTaroatRed());
        if (OnTarotDataUpdate != null) OnTarotDataUpdate();
    }
    public bool isTaroatRed()
    {
        bool isRed = false;
        if (wdfTaroatData != null)
        { 
            if ((int)GameCenter.mainPlayerMng.MainPlayerInfo.DiamondCount >= wdfTaroatData.consume)
            {
                if (wdfTaroatData.needVipLev <= 0)
                {
                    isRed = true;
                }
                else if (GameCenter.vipMng.VipData != null && GameCenter.vipMng.VipData.vLev >= wdfTaroatData.needVipLev)
                {
                    isRed = true;
                }
            } 
        }
        return isRed;
    }
    #endregion
}


public class OpenServerRewardInfoData
{
    /// <summary>
    /// 剩余时间
    /// </summary>
    public int remainTime;

   /// <summary>
   /// 礼包数据集合
   /// </summary>
    public List<OpenServerRewardInfoItemData> rewardItems = new List<OpenServerRewardInfoItemData>();
    public OpenServerRewardInfoData(pt_reply_open_server_gift_info_d911 _pt)
    {
        remainTime = (int)_pt.rest_time;
        for (int i = 0; i < _pt.wares.Count; i++)
        {
            rewardItems.Add(new OpenServerRewardInfoItemData(_pt.wares[i]));
            //Debug.Log("礼包名字" + _pt.wares[i].name);
        }
        rewardItems.Sort(SortCompare);
    }

    private int SortCompare(OpenServerRewardInfoItemData AF1, OpenServerRewardInfoItemData AF2)
    {
        int res = 0;
        if (AF1.id > AF2.id)
        {
            res = 1;
        }
        else if (AF1.id < AF2.id)
        {
            res = -1;
        }
        return res;
    }

}


public class OpenServerRewardInfoItemData

{
   /// <summary>
   /// 礼包ID
   /// </summary>
    public uint id;
    /// <summary>
    /// 礼包名字
    /// </summary>
    public string giftName;
    /// <summary>
    /// 礼包物品
    /// </summary>
    public EquipmentInfo item_type;

    /// <summary>
    /// 原价
    /// </summary>
    public uint orig_price;
    /// <summary>
    /// 现价
    /// </summary>
    public uint cur_price;
    /// <summary>
    /// 限购数
    /// </summary>
    public uint max_buy_amount;


    public OpenServerRewardInfoItemData(open_server_gift_ware_info _info)
    {
        id = _info.id;
        giftName = _info.name;
        item_type = new EquipmentInfo( (int)_info.item_type,EquipmentBelongTo.PREVIEW);
        orig_price = _info.orig_price;
        cur_price = _info.cur_price;
        max_buy_amount = _info.max_buy_amount;
    }

}


/// <summary>
/// 抽奖数据
/// </summary>
public class WdfLotteryData
{
    /// <summary>
    /// 抽奖活动剩余时间
    /// </summary>
    public int restTime;
    /// <summary>
    /// 单抽价格
    /// </summary>
    public int price1;
    /// <summary>
    /// 十抽价格
    /// </summary>
    public int price2;
    /// <summary>
    /// 奖池金额
    /// </summary>
    public int allRewarCount;
    /// <summary>
    /// 首次10抽标志 0为未进行过10抽 1为已经进行过10抽 
    /// </summary>
    public bool isLotteriedTenTimes;
    /// <summary>
    /// 基础转盘奖励
    /// </summary>
    public List<lucky_wheel_reward_info> basicReward = new List<lucky_wheel_reward_info>();
    /// <summary>
    /// 进阶转盘奖励
    /// </summary>
    public List<lucky_wheel_reward_info> advanceReward = new List<lucky_wheel_reward_info>();


    public WdfLotteryData(pt_lucky_wheel_info_d961 _info)
    {
        basicReward.Clear();
        advanceReward.Clear();
        this.restTime = (int)_info.rest_time;
        this.price1 = (int)_info.price1;
        this.price2 = (int)_info.price2;
        this.allRewarCount = (int)_info.jackpot;
        this.isLotteriedTenTimes = _info.flag == 1; 
        for (int i = 0, max = _info.wheel_info.Count; i < max; i++)
        { 
            if (_info.wheel_info[i].wheel_type == 1)
            {
                basicReward.Add(_info.wheel_info[i]);
            }
            if (_info.wheel_info[i].wheel_type == 2)
            {
                advanceReward.Add(_info.wheel_info[i]);
            }
        }
        basicReward.Sort(SortInfo);
        advanceReward.Sort(SortInfo);
    }

    public void Update(pt_lucky_wheel_info_d961 _info)
    {
        basicReward.Clear();
        advanceReward.Clear();
        this.restTime = (int)_info.rest_time;
        this.price1 = (int)_info.price1;
        this.price2 = (int)_info.price2;
        this.allRewarCount = (int)_info.jackpot;
        this.isLotteriedTenTimes = _info.flag == 1; 
        for (int i = 0, max = _info.wheel_info.Count; i < max; i++)
        { 
            if (_info.wheel_info[i].wheel_type == 1)
            {
                basicReward.Add(_info.wheel_info[i]);
            }
            if (_info.wheel_info[i].wheel_type == 2)
            {
                advanceReward.Add(_info.wheel_info[i]);
            }
        }
        basicReward.Sort(SortInfo);
        advanceReward.Sort(SortInfo);
    }

    public void Update(pt_lucky_wheel_reward_d965 _info)
    { 
        this.allRewarCount = (int)_info.jackpot;
        this.isLotteriedTenTimes = _info.flag == 1;
        if (OnRewardCountUpdate != null) OnRewardCountUpdate();
    }

    public System.Action OnRewardCountUpdate;

    public WdfLotteryData()
    {

    }

    protected int SortInfo(lucky_wheel_reward_info _data1, lucky_wheel_reward_info _data2) 
    {
        if (_data1.id > _data2.id)
        {
            return 1;
        }
        if (_data1.id < _data2.id)
        {
            return -1;
        }
        return 0;
    } 
}

/// <summary>
/// 塔罗牌数据
/// </summary>
public class WdfTaroatData
{
    /// <summary>
    /// 剩余时间
    /// </summary>
    public int restTime;
    /// <summary>
    /// 活动剩余次数
    /// </summary>
    public int activeCount;

    protected CornucopiaRef cornucopiaRefdata = null;
    protected CornucopiaRef CornucopiaRefData
    {
        get
        {
            if (cornucopiaRefdata == null || cornucopiaRefdata.id != (ConfigMng.Instance.GetCornucopiaRefCount() - activeCount + 1))
            {
                cornucopiaRefdata = ConfigMng.Instance.GetCornucopiaRef(activeCount);
            }
            return cornucopiaRefdata;
        }
    }
    /// <summary>
    /// 需要的vip等级
    /// </summary>
    public int needVipLev
    {
        get
        {
            return CornucopiaRefData == null ? 0 : CornucopiaRefData.vip;
        }
    }
    /// <summary>
    /// 需要消耗
    /// </summary>
    public int consume
    {
        get
        {
            return CornucopiaRefData == null ? 0 : CornucopiaRefData.consume;
        }
    }
    /// <summary>
    /// 获得80%
    /// </summary>
    public int getNumMax
    {
        get
        {
            return CornucopiaRefData == null ? 0 : CornucopiaRefData.probability80;
        }
    }
    /// <summary>
    /// 20%可获得
    /// </summary>
    public int getNumMin
    {
        get
        {
            return CornucopiaRefData == null ? 0 : CornucopiaRefData.probability20;
        }
    }


    public WdfTaroatData(pt_tarot_info_c128 _info)
    {
        this.restTime = (int)_info.surplus_times;
        this.activeCount = (int)_info.surplus_num;
    }

    public void Update(pt_tarot_info_c128 _info)
    {
        this.restTime = (int)_info.surplus_times;
        this.activeCount = (int)_info.surplus_num;
    }

    public WdfTaroatData()
    {

    }
}
