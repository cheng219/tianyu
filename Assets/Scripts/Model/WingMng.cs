//===============================
//日期：2016/3/24
//作者：鲁家旗
//用途描述:翅膀管理类
//===============================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
public class WingMng
{
    #region 数据
    /// <summary>
    /// 翅膀数据 
    /// </summary>
    protected FDictionary wingDic = new FDictionary();
    /// <summary>
    /// 翅膀数据 
    /// </summary>
    public FDictionary WingDic
    {
        get
        {
            return wingDic;
        }
    }
    
    /// <summary>
    /// 当前使用的翅膀数据
    /// </summary>
    protected WingInfo curUseWingInfo = null;
    /// <summary>
    /// 当前使用的翅膀数据
    /// </summary>
    public WingInfo CurUseWingInfo
    {
        get
        {
            return curUseWingInfo;
        }
        protected set
        {
            if (value == null)
            {
                UpdateWing(false);
            }
            curUseWingInfo = value;
            if (curUseWingInfo != null)
            {
                UpdateWing(true);
            }
            if (OnCurUseWingInfoUpdate != null)
            {
                OnCurUseWingInfoUpdate();
            }
        }
    }
    /// <summary>
    /// 当前选中翅膀发生变化事件
    /// </summary>
    public System.Action OnCurUseWingInfoUpdate;
    /// <summary>
    /// 翅膀属性变化事件
    /// </summary>
    public System.Action OnWingUpdate;
    /// <summary>
    /// 翅膀等阶经验变化
    /// </summary>
    public System.Action OnGetWingChange;
    /// <summary>
    /// 翅膀个数变化
    /// </summary>
    public System.Action<GameObject> OnWingAdd;
    /// <summary>
    /// 升级抛出事件
    /// </summary>
    public System.Action OnAddLev;
    /// <summary>
    /// 翅膀模型不同时的ID
    /// </summary>
    public int curWingID = 0;
    /// <summary>
    /// 激活翅膀时需要展示模型抛出事件
    /// </summary>
    public System.Action<WingInfo> OnShowWingModel;
    /// <summary>
    /// 需要展示的翅膀数据
    /// </summary>
    public WingInfo needShowWingInfo;
    /// <summary>
    /// 需要展示的坐骑数据
    /// </summary>
    public MountInfo needShowMountInfo;
    /// <summary>
    /// 需要展示的宠物数据
    /// </summary>
    public MercenaryInfo needShowPetInfo;
    /// <summary>
    /// 展示类型
    /// </summary>
    public ModelType modelType = ModelType.NONE;
    /// <summary>
    /// 是否是试用翅膀
    /// </summary>
    public bool isNotShowTrialWingModel = false;
    #endregion

    #region 构造
    /// <summary>
    /// 返回一个全新的管理类对象实例
    /// </summary>
    /// <param name="_father"></param>
    /// <returns></returns>
    public static WingMng CreateNew(MainPlayerMng _father)
    {
        if (_father.wingMng == null)
        {
            WingMng wingMng = new WingMng();
            wingMng.Init(_father);
            return wingMng;
        }
        else
        {
            _father.wingMng.UnRegist();
            _father.wingMng.Init(_father);
            return _father.wingMng;
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="_father"></param>
    void Init(MainPlayerMng _father)
    {
        MsgHander.Regist(0xD417, S2C_GetWingData);
        MsgHander.Regist(0xD420, S2C_GetWingUpData);
        MsgHander.Regist(0xD443, S2C_GetWingState);
    }
    /// <summary>
    /// 注销
    /// </summary>
    void UnRegist()
    {
        MsgHander.UnRegist(0xD417, S2C_GetWingData);
        MsgHander.UnRegist(0xD420, S2C_GetWingUpData);
        MsgHander.UnRegist(0xD443, S2C_GetWingState);
        wingDic.Clear();
        curWingID = 0;
        curUseWingInfo = null;
        needShowWingInfo = null;
        needShowMountInfo = null;
        needShowPetInfo = null;
        modelType = ModelType.NONE;
        isNotShowTrialWingModel = false;
    }
    #endregion

   
    #region 协议
    #region S2C 服务端发往客户端的协议处理
    /// <summary>
    /// 返回翅膀列表
    /// </summary>
    /// <param name="_msg"></param>
    protected void S2C_GetWingData(Pt _msg)
    {
        pt_wing_list_info_d417 msg = _msg as pt_wing_list_info_d417;
        if (msg != null)
        {
            for (int i = 0; i < msg.wing_list.Count; i++)
            {
                wing_base_info data = msg.wing_list[i];
                if (wingDic.ContainsKey((int)data.wing_id))
                { 
                    WingInfo info = wingDic[(int)data.wing_id] as WingInfo;
                    if (info != null)
                    {
                        info.Update(data);
                    }
                }
                else
                {
                    WingInfo info = new WingInfo(data);
                    if(info.WingType != 2)
                        wingDic[info.WingId] = info;
                    if (info.WingState)
                    {
                        if (OnShowWingModel != null)
                            OnShowWingModel(info);
                        CurUseWingInfo = info;
                    }
                }
            }
            SetWingRedPoint();
        }
        //抛出升级后变化事件
        if (OnWingAdd != null)
        {
            OnWingAdd(null);
        }
    }
    public bool isRefreshEffect = false;
    /// <summary>
    /// 翅膀升级后返回协议
    /// </summary>
    protected void S2C_GetWingUpData(Pt _msg)
    {
        pt_update_wing_lev_d420 msg = _msg as pt_update_wing_lev_d420;
        if (msg != null)
        {
            if (wingDic.ContainsKey(msg.wing_id))
            {
                WingInfo info = wingDic[msg.wing_id] as WingInfo;
                info.Update(msg);
                if (info.WingItemId != info.WingNextItemId)
                {
                    curWingID = info.WingNextItemId;
                }
                if (info.WingItemId == curWingID)
                {
                    isRefreshEffect = true;
                    if (info.WingState)
                    {
                        CurUseWingInfo = info;
                    }
                    curWingID = 0;
                }
            }
            SetWingRedPoint();
        }
        //抛出升级后变化事件
        if (OnWingUpdate != null)
        {
            OnWingUpdate();
        }
        if (OnGetWingChange != null)
        {
            OnGetWingChange();
        }
    }
    /// <summary>
    /// 翅膀穿戴返回协议
    /// </summary>
    /// <param name="_msg"></param>
    public void S2C_GetWingState(Pt _msg)
    {
        pt_update_wing_state_d443 msg = _msg as pt_update_wing_state_d443;
        if (msg != null)
        {
            if (wingDic.ContainsKey(msg.wing_id))
            {
                WingInfo info = wingDic[msg.wing_id] as WingInfo;
                info.Update(msg);
                if (info.WingState)
                {
                    CurUseWingInfo = info;
                }
                else
                {
                    CurUseWingInfo = null;
                }
            }
            else
            {
                WingInfo info = new WingInfo(ConfigMng.Instance.GetWingRef(msg.wing_id,1));
                if (msg.state == 1)
                {
                    CurUseWingInfo = info;
                }
                else
                {
                    CurUseWingInfo = null;
                }
            }
        }
        if (OnWingUpdate != null)
        {
            OnWingUpdate();
        }
    }
    #endregion

    #region C2S 客户端发往服务端的协议处理
    /// <summary>
    /// 请求获得翅膀列表
    /// </summary>
    public void C2S_RequestGetWing()
    {
        pt_req_wing_list_d428 msg = new pt_req_wing_list_d428();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求升级翅膀 
    /// _state 1是升级翅膀 2是激活翅膀
    /// _quickBuy 0是正常升级 1是快捷购买
    /// </summary>
    public void C2S_RequestUpLev(WingState _state, int _id, bool _quickBuy)
    {
        pt_req_up_wing_lev_d419 msg = new pt_req_up_wing_lev_d419();
        msg.state =(int)_state;
        msg.wing_id = _id;
        msg.quick_state = _quickBuy ? 1 : 0;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求穿戴卸下翅膀 true 请求穿上翅膀 false 请求脱下翅膀
    /// </summary>
    /// <param name="_id"></param>
    /// <param name="_state"></param>
    public void C2S_RequestChangeWing(int _id, bool _state)
    {
        pt_req_change_wing_d429 msg = new pt_req_change_wing_d429();
        msg.wing_id = (uint)_id;
        msg.state = (uint)(_state ? 1 : 0);
        NetMsgMng.SendMsg(msg);
    }
    #endregion
    #endregion

    #region 辅助逻辑
    protected void UpdateWing(bool _equip)
    {
        if (CurUseWingInfo != null)
        {
            if (GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo != null)
            {
                GameCenter.mainPlayerMng.MainPlayerInfo.UpdateEquipment(CurUseWingInfo.WingItemId, _equip);
            }
        }
    }
    /// <summary>
    /// 设置翅膀红点
    /// </summary>
    public void SetWingRedPoint()
    {
        bool isRedPoint = false;
        if (wingDic.Count == 0)
        {
            WingRef data = ConfigMng.Instance.GetWingRef(1, 1);
            if (data != null)
            {
                WingInfo wingInfo = new WingInfo(data);
                if (wingInfo.WingActiveEnough)
                    isRedPoint = true;
                else
                    isRedPoint = false;
            }
        }
        else
        {
            foreach (WingInfo info in wingDic.Values)
            {
                if (info.WingConsumeEnough && info.WingLev < ConfigMng.Instance.GetWingMaxLev(info.WingId))//满级不显示红点
                {
                    isRedPoint = true;
                    break;
                }
                else
                    isRedPoint = false;
            }
        }
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.FAIRYFEATHER, isRedPoint);
    }
    /// <summary>
    /// 外部元宝铜币发生变化时调用
    /// </summary>
    public void SetWingRedPoint(ActorBaseTag tag, ulong value, bool _fromAbility)
    {
        if (tag == ActorBaseTag.BindCoin || tag == ActorBaseTag.UnBindCoin)
        {
            SetWingRedPoint();
        }
    }
    #endregion
}
/// <summary>
/// 翅膀状态
/// </summary>
public enum WingState
{ 
    /// <summary>
    /// 升级翅膀
    /// </summary>
    UPWINGLEV = 1,
    /// <summary>
    /// 激活翅膀
    /// </summary>
    ACTIVEWING = 2,
}
/// <summary>
/// 模型类型
/// </summary>
public enum ModelType
{ 
    NONE,
    /// <summary>
    /// 翅膀
    /// </summary>
    WING,
    /// <summary>
    /// 坐骑
    /// </summary>
    MOUNT,
    /// <summary>
    /// 幻化
    /// </summary>
    ILLUSION,
    /// <summary>
    /// 宠物
    /// </summary>
    PET,
}