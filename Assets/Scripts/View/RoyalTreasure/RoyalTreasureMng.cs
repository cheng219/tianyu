//======================================================
//作者:鲁家旗
//日期:2017/1/19
//用途:皇室宝箱管理类
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;
public class RoyalTreasureMng
{
    #region 数据
    /// <summary>
    /// 自身所拥有的宝箱数据
    /// </summary>
    public FDictionary royalTreasureDic = new FDictionary();
    /// <summary>
    /// 宝箱信息变化事件
    /// </summary>
    public System.Action OnRoyalBoxUpdate;
    /// <summary>
    /// 获得宝箱奖励抛出事件
    /// </summary>
    public System.Action<List<EquipmentInfo>> OnGetRoyalReward;
    /// <summary>
    /// 领奖完毕抛出事件
    /// </summary>
    public System.Action OnGetRewardOver;
    /// <summary>
    /// 剩余加速次数
    /// </summary>
    public int restActiveTimes = 0;
    /// <summary>
    /// 是否有宝箱正在开启
    /// </summary>
    public bool isOpeningBox = false;
    /// <summary>
    /// 当前领奖的宝箱
    /// </summary>
    public RoyalTreaureData curGetRewardBoxData;
    /// <summary>
    /// 宝箱显示的时间差
    /// </summary>
    public float timeGap;

    public System.Action OnGetNewTreasureBoxEvent;
    #endregion

    #region 构造
    public static RoyalTreasureMng CreateNew()
    {
        if (GameCenter.royalTreasureMng == null)
        {
            RoyalTreasureMng royalTreasureMng = new RoyalTreasureMng();
            royalTreasureMng.Init();
            return royalTreasureMng;
        }
        else
        {
            GameCenter.royalTreasureMng.UnRegist();
            GameCenter.royalTreasureMng.Init();
            return GameCenter.royalTreasureMng;
        }
    }
    void Init()
    {
        //Debug.Log("MsgHander.Regist(0xD944, S2C_CollectRoyalBoxData)");
        MsgHander.Regist(0xD941, S2C_ReplyBoxData);
        MsgHander.Regist(0xD944, S2C_CollectRoyalBoxData);
    }
    void UnRegist()
    {
        //Debug.Log("MsgHander.UnRegist(0xD944, S2C_CollectRoyalBoxData)");
        MsgHander.UnRegist(0xD941, S2C_ReplyBoxData);
        MsgHander.UnRegist(0xD944, S2C_CollectRoyalBoxData);
    }
    #endregion

    #region 协议
    protected void S2C_ReplyBoxData(Pt _msg)
    {
        //Debug.Log("S2C_ReplyBoxData");
        pt_reply_royal_box_list_d941 msg = _msg as pt_reply_royal_box_list_d941;
        if (msg != null)
        {
            royalTreasureDic.Clear();
            restActiveTimes = (int)msg.rest_acc_times;
            isOpeningBox = false;
            for (int i = 0; i < msg.box_list.Count; i++)
            {
                royal_box_info data = msg.box_list[i];
                //Debug.Log(ConfigMng.Instance.GetRoyalBoxRef((int)data.type).boxItemID);
                if (data.active == 1)//当前有一个宝箱正在开启
                {
                    //Debug.Log("已经开启剩余时间=" + data.rest_time);
                    timeGap = Time.realtimeSinceStartup;
                    if(data.rest_time>0.00)
                       isOpeningBox = true;
                    RoyalBoxRef royalBoxRef = ConfigMng.Instance.GetRoyalBoxRef((int)data.type);
                    //发送消息推送(只发送一次，就在刚开启的时候发送)
                    if (royalBoxRef != null && data.rest_time == royalBoxRef.time)
                    {
                        DateTime newServerTime = GameCenter.instance.CurServerTime;
                        DateTime endTime = newServerTime.AddSeconds((int)data.rest_time);
                        string time = string.Format("{0:D2}:{1:D2}:{2:D2}", endTime.Hour, endTime.Minute, endTime.Second);  
                        GameCenter.messageMng.SendPushInfo(2, 1, time);
                    }
                }
                if (!royalTreasureDic.ContainsKey(data.id))
                {
                    RoyalTreaureData royalData = new RoyalTreaureData(data);
                    royalTreasureDic[data.id] = royalData;
                }
                //if(royalTreasureDic.Count>=4)//宝箱位只有4个超过4个提示宝箱格已满
                //{
                //    GameCenter.messageMng.AddClientMsg(486);
                //}
            }
        }
        if (OnRoyalBoxUpdate != null)
        {
            OnRoyalBoxUpdate();
        }
    }
    protected void S2C_CollectRoyalBoxData(Pt _msg)
    {
        //Debug.Log("收到拾取宝箱协议");
        pt_add_royal_box_d944 msg = _msg as pt_add_royal_box_d944;
        if (msg!=null)
        {
            RoyalBoxRef royalBoxRef = ConfigMng.Instance.GetRoyalBoxRef((int)msg.type);
            if(royalBoxRef!= null)
            {
                
                //EquipmentRef equipmentRef = ConfigMng.Instance.GetEquipmentRef(royalBoxRef.boxItemID);
                EquipmentInfo equip = new EquipmentInfo(royalBoxRef.boxItemID, EquipmentBelongTo.PREVIEW);
                if (equip != null)
                {
                    string name = equip.ItemStrColor+ equip.ItemName + "[-]";
                    //Debug.Log("掉落的宝箱的名字:"+name);
                    MessageST mst = new MessageST();
                    mst.messID = 506;
                    mst.words = new string[] { name };
                    GameCenter.messageMng.AddClientMsg(mst);
                    //Debug.logger.Log("拾取到宝箱的提示" + mst.words);
                    if (OnGetNewTreasureBoxEvent != null)
                        OnGetNewTreasureBoxEvent();
                }
                else
                {
                    Debug.LogError("找不到ID为" + royalBoxRef.boxItemID + "的物品配置");
                }

            }
            else
            {
                Debug.LogError("找不到ID为"+ (int)msg.type+"的宝箱配置");
            }
        }
    }
    /// <summary>
    /// 请求宝箱数据
    /// </summary>
    public void C2S_ReqRoyalBoxList()
    {
        pt_req_royal_box_list_d940 msg = new pt_req_royal_box_list_d940();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求激活宝箱
    /// </summary>
    /// <param name="_id"></param>
    public void C2S_ReqActiveRoyalBox(int _id)
    {
        pt_active_royal_box_d942 msg = new pt_active_royal_box_d942();
        msg.id = (uint)_id;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求领取宝箱奖励
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_type"></param>
    public void C2S_ReqGetRoyalReward(int _id,int _type)
    {
        pt_get_royal_box_reward_d943 msg = new pt_get_royal_box_reward_d943();
        msg.id =(uint)_id;
        msg.type = (byte)_type;
        NetMsgMng.SendMsg(msg);
    }
    #endregion
}
public class RoyalTreaureData
{
    protected royal_box_info serverdata;

    public RoyalTreaureData(royal_box_info _data)
    {
        serverdata = _data;
    }
    public void UpdateRoyalData(royal_box_info _data)
    {
        if (serverdata != null)
        {
            serverdata.id = _data.id;
            serverdata.rest_time = _data.rest_time;
            serverdata.type = _data.type;
            serverdata.active = _data.active;
        }
        else
        {
            serverdata = _data;
        }
    }

    /// <summary>
    /// 服务器ID
    /// </summary>
    public int ID
    {
        get
        {
            return serverdata != null ? (int)serverdata.id : 0;
        }
    }
    /// <summary>
    /// 宝箱物品ID
    /// </summary>
    public int ItemID
    {
        get
        {
            return serverdata != null ? (int)serverdata.type : 0;
        }
    }
    /// <summary>
    /// 宝箱信息
    /// </summary>
    public EquipmentInfo RoyalTreasueInfo 
    {
        get
        {
            EquipmentInfo royalTreasueInfo = null;
            if (serverdata != null)
            {
                royalTreasueInfo = new EquipmentInfo((int)serverdata.type, EquipmentBelongTo.PREVIEW);
            }
            return royalTreasueInfo;
        }
    }
    /// <summary>
    /// 当前宝箱的状态(0未开启 1已开启)
    /// </summary>
    public bool curState
    {
        get
        {
            return serverdata != null ? serverdata.active == 0 : false;
        }
    }
    /// <summary>
    /// 剩余时间
    /// </summary>
    public int restTime
    {
        get
        {
            return serverdata != null ? (int)serverdata.rest_time : 0;
        }
    }
}
/// <summary>
/// 宝箱开启状态枚举
/// </summary>
public enum RoyalTreasureOpenState
{
    NONE,
    /// <summary>
    /// 未开启
    /// </summary>
    NOTOPEN,
    /// <summary>
    /// 开启中
    /// </summary>
    OPENING,
    /// <summary>
    /// 已开启未领奖
    /// </summary>
    HAVEOPEN,
    /// <summary>
    /// 已开启已领奖
    /// </summary>
    HAVEOPENGETREWARD,
}