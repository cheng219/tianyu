//==================================
//作者：朱素云
//日期：2016/3/28
//用途：坐骑管理类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class NewMountMng
{
    #region 数据

    public int rideType = 0; 
    public int skinId = 0;
    protected int state = 0;
    protected bool skinState = false;

    protected void MountUpdate()
    { 
        if (skinState && skinId != 0 && state == 1)
        {
            if (curMountInfo == null || curMountInfo.ConfigID != skinId)
            {
                curMountInfo = GetMountSkinById(skinId);
            }
        }
        else
        {
            if (curMountInfo == null || curMountInfo.ConfigID != rideType)
            {
                curMountInfo = GetMountById(rideType);
            }
        }
        if (curMountInfo != null)
        {
            curMountInfo.SetCurMount();
            if (!curMountInfo.IsRiding)
            {
                curMountInfo.UpdateRide();
            } 
        }
    }

    #region 坐骑
    protected int curLev = -1;
 
    /// <summary>
    /// 当前坐骑等级
    /// </summary>
    public int CurLev
    {
        get
        {
            return curLev;
        }
        protected set
        {
            if (curLev != value)
            {
                curLev = value;
                if (OnMountLevUpdate != null)
                {
                    OnMountLevUpdate();
                }
            }
        }
    } 
    /// <summary>
    /// 当前坐骑info
    /// </summary>
    public MountInfo curMountInfo = null;
    /// <summary>
    /// 服务端给的已经拥有的坐骑列表
    /// </summary>
    public FDictionary mountInfoList = new FDictionary(); 
    /// <summary>
    /// 根据坐骑id获取坐骑info
    /// </summary> 
    public MountInfo GetMountById(int _id)
    {
        MountInfo info = mountInfoList[_id] as MountInfo;
        return info;
    }

    /// <summary>
    /// 坐骑变化事件
    /// </summary>
    public System.Action OnCurMountUpdate; 
    /// <summary>
    /// 坐骑等级变化事件
    /// </summary>
    public System.Action OnMountLevUpdate;
    /// <summary>
    /// 所有坐骑
    /// </summary>
    public FDictionary AllMountDic = new FDictionary();
    #endregion

    #region 幻化 
    /// <summary>
    /// 服务端传来的幻化列表
    /// </summary>
    public FDictionary mountSkinList = new FDictionary();
    /// <summary>
    /// 根据幻兽id获取幻兽info
    /// </summary> 
    public MountInfo GetMountSkinById(int _id)
    {
        MountInfo info = mountSkinList[_id] as MountInfo;
        return info;
    }
    /// <summary>
    /// 当前幻化等级
    /// </summary>
    public int curSkinLev = 0;
    /// <summary>
    /// 幻化经验
    /// </summary>
    public int skinExp = 0; 
    /// <summary>
    /// 幻化列表数据变化
    /// </summary>
    public System.Action OnMountSkinListUpdate;
    /// <summary>
    /// 幻化数据更新
    /// </summary>
    public System.Action OnMountSkinUpdate;
    /// <summary>
    /// 所有幻化
    /// </summary>
    public FDictionary AllSkinDic = new FDictionary();
    protected List<MountRef> mountList = new List<MountRef>();
    /// <summary>
    /// 当前选中的皮肤
    /// </summary>
    public MountInfo curChooseSkin = null;
     
    #endregion

    #region 骑装
    protected Dictionary<EquipSlot, EquipmentInfo> mountEquipDic = new Dictionary<EquipSlot, EquipmentInfo>();
    public Dictionary<EquipSlot, EquipmentInfo> MountEquipDic
    {
        get
        {
            Dictionary<EquipSlot, EquipmentInfo> dic = new Dictionary<EquipSlot, EquipmentInfo>();
            foreach (EquipmentInfo info in mountEquipDic.Values)
            {
                if (info.StackCurCount != 0)
                    dic[info.Slot] = info;
            }
            return dic;
        }
    }
    public bool HaveTheSlotMountEquip(EquipSlot _slot)
    {
        return mountEquipDic.ContainsKey(_slot) && mountEquipDic[_slot] != null && mountEquipDic[_slot].StackCurCount > 0;
    }

    public System.Action OnMountEquipUpdate;

    /// <summary>
    /// 升品结果返回
    /// </summary>
    public System.Action OnUpgradeEquipmentUpdateEvent;
    /// <summary>
    /// 升级结果返回。true升级 false增加经验
    /// </summary>
    public System.Action<bool> OnStrengthEquipmentEvent;

    public System.Action OnSelectEquipmentUpdate;
    protected EquipmentInfo curSelectEquipmentInfo;
    public EquipmentInfo CurSelectEquipmentInfo
    {
        set
        {
            curSelectEquipmentInfo = value;
            if (OnSelectEquipmentUpdate != null)
                OnSelectEquipmentUpdate();
        }
        get
        {
            return curSelectEquipmentInfo;
        }
    }

    public EquipmentInfo GetEquipmentInfoByID(int _instanceID)
    {
        using (var e = mountEquipDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                if (e.Current.Value.InstanceID == _instanceID)
                    return e.Current.Value;
            }
        }
        return null;
    }
    #endregion

    #region
    /// <summary>
    ///  用于激活新模型的时候刷新模型展示界面
    /// </summary>
    public System.Action<MountInfo, ModelType> OnGetNewMountUpdate;
    #endregion

    /// <summary>
    /// 1 坐骑 2 幻化
    /// </summary> 
    public List<MountRef> MountList(int type)
    { 
        FDictionary mountTable = ConfigMng.Instance.MountRefTable;
        mountList.Clear();
        if (type == 1)
        {
            foreach (MountRef item in mountTable.Values)
            {
                if (item.kind == 1)
                {
                    if (!AllMountDic.ContainsKey(item.mountId))
                    {
                        AllMountDic[item.mountId] = new MountInfo(item);
                    }
                    mountList.Add(item);
                }
            }
            return mountList;
        }
        else if (type == 2)
        {
            foreach (MountRef item in mountTable.Values)
            {
                if (item.kind == 2)
                {
                    if (!AllSkinDic.ContainsKey(item.mountId))
                    {
                        AllSkinDic[item.mountId] = new MountInfo(item);
                    }
                    mountList.Add(item);
                }
            }
            mountList.Sort(SortMountInfo);
            return mountList;
        }
        return mountList;
    }

    protected int SortMountInfo(MountRef reward1, MountRef reward2)
    { 
        int state1 = 0;
        int state2 = 0;
        //已经拥有
        if (mountSkinList.ContainsKey(reward1.mountId)) state1 = 1; 

        //已经拥有
        if (mountSkinList.ContainsKey(reward2.mountId)) state2 = 1; 

        if (state1 < state2)
            return 1;
        if (state1 > state2)
            return -1;
        if (reward1.level > reward2.level)//状态相同按ID排序
            return 1;
        if (reward1.level < reward2.level)
            return -1;
        return 0;
    } 
    #endregion

    #region 构造

    public static NewMountMng CreateNew()
    {
        if (GameCenter.newMountMng == null)
        {
            NewMountMng newMountMng = new NewMountMng();
            newMountMng.Init();
            return newMountMng;
        }
        else
        {
            GameCenter.newMountMng.UnRegist();
            GameCenter.newMountMng.Init();
            return GameCenter.newMountMng;
        }
    }
    protected void Init()
    {
        MsgHander.Regist(0xE111, S2C_GetMountListInfo);
        MsgHander.Regist(0xE113, S2C_GetMountSkinListInfo);
        MsgHander.Regist(0xD439, S2C_GetMountPromoteLev);
        MsgHander.Regist(0xD440, S2C_GetMountAfterUpdate);
        MsgHander.Regist(0xD757, S2C_GetSkinLevUpdate);
        MsgHander.Regist(0xD575, S2C_OnGotEquipData);
        MsgHander.Regist(0xD576, S2C_OnUpdateEquipData);
    }
    protected void UnRegist()
    {
        MsgHander.UnRegist(0xE111, S2C_GetMountListInfo);
        MsgHander.UnRegist(0xE113, S2C_GetMountSkinListInfo);
        MsgHander.UnRegist(0xD439, S2C_GetMountPromoteLev);
        MsgHander.UnRegist(0xD440, S2C_GetMountAfterUpdate);
        MsgHander.UnRegist(0xD757, S2C_GetSkinLevUpdate);
        MsgHander.UnRegist(0xD575, S2C_OnGotEquipData);
        MsgHander.UnRegist(0xD576, S2C_OnUpdateEquipData);
        curMountInfo = null; 
        curSkinLev = 0;
        skinExp = 0;
        curLev = -1; 
        mountSkinList.Clear();
        mountInfoList.Clear();
        AllMountDic.Clear();
        AllSkinDic.Clear();
        mountList.Clear();
        rideType = 0; 
        skinId = 0;
        state = 0;
        skinState = false;
        mountEquipDic.Clear();
        curChooseSkin = null;
        curSelectEquipmentInfo = null;
    }
    #endregion

    #region 辅助逻辑
    /// <summary>
    /// 坐骑红点提示
    /// </summary> 
    public void SetRedRemind(ActorBaseTag tag, ulong value, bool _fromAbility)
    {
        if (tag == ActorBaseTag.BindCoin || tag == ActorBaseTag.UnBindCoin || tag == ActorBaseTag.CoinLimit)
        {
            SetRedRemind();
        }
        if (tag == ActorBaseTag.Diamond)
        {
            GameCenter.openServerRewardMng.SetTaroatRedRemind();
        }
    }
    /// <summary>
    /// 坐骑红点提示
    /// </summary>
    public void SetRedRemind()
    {
        if (mountInfoList.Count <= 0)
        {
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.MOUNT, false);
            return;
        }
        bool isCoinEnough = false;
        bool isItemEnough = false;
        RidePropertyRef MountPropertyRefData = ConfigMng.Instance.GetMountPropertyRef(CurLev);
        if (MountPropertyRefData != null && MountPropertyRefData.item != null)
        {
            for (int j = 0; j < MountPropertyRefData.item.Count; j++)
            {
                if (MountPropertyRefData.item[j].eid == 5)
                {
                    if (GameCenter.mainPlayerMng.MainPlayerInfo != null)
                    {
                        if (GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount >= (ulong)MountPropertyRefData.item[j].count)
                        {
                            isCoinEnough = true;
                        }
                    }
                }
                else
                {
                    int itemId = MountPropertyRefData.item[j].eid; 
                    if (GameCenter.inventoryMng.GetNumberByType(itemId) >= MountPropertyRefData.item[j].count)
                        isItemEnough = true;
                }
            }
        }
        if (isCoinEnough && isItemEnough)
        {
            if (CurLev < (ConfigMng.Instance.RidePropertyRefTable.Count - 1))
                GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.MOUNT, true);
            else
                GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.MOUNT, false);
        }
        else
        {
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.MOUNT, false);
        } 
    }

    #region 红点

    public void SetRedTipState()
    {
        bool strengthRed = false;
        List<EquipmentInfo> equipList = new List<EquipmentInfo>(mountEquipDic.Values);
        for (int i = 0, max = equipList.Count; i < max; i++)
        {
            if (!strengthRed && equipList[i] != null && (equipList[i].RealCanStrength || equipList[i].RealCanUpgrade))
            {
                strengthRed = true;//只设一次红点提示
                break;
            }
        }
        if (GameCenter.mainPlayerMng != null)
        {
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.RIDINGSUIT, strengthRed);
        }
    }
    #endregion
    #endregion 

    #region S2C
    /// <summary>
    /// 獲取坐騎
    /// </summary> 
    protected void S2C_GetMountListInfo(Pt _pt)
    {
        pt_ret_ride_info_e111 msg = _pt as pt_ret_ride_info_e111;
        if (msg != null)
        { 
            for (int i = 0; i < msg.ride_list.Count; i++)
            {
                int id = msg.ride_list[i].ride_id;
                //Debug.Log("坐骑id : " + msg.ride_list[i].ride_id + " , 状态ride_state：" + msg.ride_list[i].ride_state + "  lev : " + msg.ride_lev);
                if (mountInfoList.ContainsKey(id))
                {
                    MountInfo info = mountInfoList[id] as MountInfo; 
                    info.Update(msg.ride_list[i]); 
                }
                else
                {
                    mountInfoList[id] = new MountInfo(new MountData(msg.ride_list[i], msg.ride_lev), GameCenter.mainPlayerMng.MainPlayerInfo);
                    if (OnGetNewMountUpdate != null && curMountInfo == null) OnGetNewMountUpdate(mountInfoList[id] as MountInfo, ModelType.MOUNT);
                }
                if (msg.ride_list[i].ride_state > 0 && (rideType != id || state != msg.ride_list[i].ride_state))
                {
                    rideType = id;
                    state = msg.ride_list[i].ride_state;
                    MountUpdate();
                }  
            }
            if (rideType == 0 && msg.ride_list.Count > 0)//默认坐骑为最后一个
            {
                rideType = msg.ride_list[msg.ride_list.Count - 1].ride_id;
                state = msg.ride_list[msg.ride_list.Count - 1].ride_state;
                MountUpdate();
            }
            CurLev = msg.ride_lev; 
            SetRedRemind();
        } 
        //if (OnMountListUpdate != null) OnMountListUpdate(); 
    }
    /// <summary>
    /// 獲取幻化列表
    /// </summary> 
    protected void S2C_GetMountSkinListInfo(Pt _pt)
    {
        pt_ret_skin_list_e113 msg = _pt as pt_ret_skin_list_e113;
        if (msg != null)
        {
            for (int i = 0; i < msg.skin_list.Count; i++)
            {
                skinExp = msg.skin_exp;
                curSkinLev = msg.skin_lev; 
                skin_base_info data = msg.skin_list[i];   
                if (mountSkinList.ContainsKey((int)data.skin_id))
                {
                    MountInfo info = mountSkinList[(int)data.skin_id] as MountInfo;
                    info.Update(data);
                }
                else
                {
                    mountSkinList[(int)data.skin_id] = new MountInfo(new MountData(data), GameCenter.mainPlayerMng.MainPlayerInfo, true); 
                }
                if (data.skin_state > 0 && (skinId != data.skin_id || !skinState))
                {  
                    skinId = (int)data.skin_id;
                    skinState = true;
                    MountUpdate();
                }
            }
        }
        //抛出数据变化的事件
        if (OnMountSkinListUpdate != null)
        {
            OnMountSkinListUpdate();
        }
    }
    public void ShowMountSkinModel(EquipmentInfo _eq)
    {
        MountInfo info = null;
        List<MountRef> list = MountList(2);
        for (int i = 0, max = list.Count; i < max; i++)
        {
            if (_eq.EID == list[i].itemID)
            {
                info = new MountInfo(list[i]); 
                break;
            }
        }
        if (info != null)
        {
            if (mountSkinList.ContainsKey(info.ConfigID)) return;
            else
            {
                if (curChooseSkin == null || curChooseSkin.ItemID != _eq.EID)
                {
                    curChooseSkin = info;
                }
                if (OnGetNewMountUpdate != null) OnGetNewMountUpdate(info, ModelType.ILLUSION);
            }
        }
    }
    /// <summary>
    /// 坐骑培养反馈协议
    /// </summary> 
    protected void S2C_GetMountPromoteLev(Pt _pt)
    {
        pt_update_ride_lev_d439 msg = _pt as pt_update_ride_lev_d439;
        if (msg != null && curLev != (int)msg.state)
        { 
            CurLev = (int)msg.state;
            if (rideType != 0)
            {
                if (mountInfoList.ContainsKey(rideType))
                {
                    MountInfo info = mountInfoList[rideType] as MountInfo;
                    info.Update(msg);
                }
            } 
        } 
    }
    /// <summary>
    /// 上下马 ride_state = 1更新坐骑 ride_state= 2更新幻化
    /// </summary> 
    protected void S2C_GetMountAfterUpdate(Pt _pt)
    {
        pt_update_ride_state_d440 msg = _pt as pt_update_ride_state_d440;
        if (msg != null)
        {
            //if (msg.state > 0) Debug.logger.Log("收到上下马id：" + msg.ride_id + "  , msg.ride_state : " + msg.ride_state + " , msg.state : " + msg.state);
            if (msg.ride_state == (int)MountType.MOUNTLIST)
            {
                if (mountInfoList.ContainsKey(msg.ride_id))
                {
                    MountInfo info = mountInfoList[msg.ride_id] as MountInfo;
                    info.Update(msg);
                    if (rideType != msg.ride_id || state != msg.state)
                    {
                        rideType = msg.ride_id;
                        state = msg.state;
                        MountUpdate();
                    }
                }
                if (OnCurMountUpdate != null) OnCurMountUpdate();  
            }
            else
            {
                if (mountSkinList.ContainsKey(msg.ride_id))
                { 
                    MountInfo info = mountSkinList[msg.ride_id] as MountInfo;
                    if (info != null)
                    { 
                        info.Update(msg);
                    }
                    if (msg.state > 0)
                    {
                        GameCenter.messageMng.AddClientMsg(306); //幻化成功 
                    }
                    if (skinId != msg.ride_id || skinState != (msg.state == 1))
                    {
                        skinId = msg.ride_id;
                        skinState = msg.state == 1;
                        MountUpdate();
                    }
                }
                if (OnMountSkinUpdate != null) OnMountSkinUpdate();
            } 
        }  
    }
    /// <summary>
    /// 幻化等级经验更新
    /// </summary> 
    protected void S2C_GetSkinLevUpdate(Pt _pt)
    {
        pt_update_ride_skin_lev_d757 msg = _pt as pt_update_ride_skin_lev_d757;
        if (msg != null)
        {
            //Debug.Log("幻化等级经验更新 ： " + curSkinLev + " , 经验 ： " + skinExp);
            skinExp = msg.exp;
            curSkinLev = msg.lev;
        }
        if (OnMountSkinUpdate != null) OnMountSkinUpdate();
    }

    /// <summary>
    /// 服务端坐骑装备数据返回 by邓成
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_OnGotEquipData(Pt _info)
    {
        pt_mequip_list_reply_d575 pt_mequip_list_reply_d575 = _info as pt_mequip_list_reply_d575;
        if (pt_mequip_list_reply_d575 != null)
        {
            int count = pt_mequip_list_reply_d575.items.Count;
            for (int i = 0; i < count; i++)
            {
                EquipmentInfo info = new EquipmentInfo(pt_mequip_list_reply_d575.items[i]);
                if (!mountEquipDic.ContainsKey(info.Slot))
                    mountEquipDic[info.Slot] = info;
                else
                    mountEquipDic[info.Slot].Update(pt_mequip_list_reply_d575.items[i], EquipmentBelongTo.EQUIP);
            }
            if (OnMountEquipUpdate != null)
                OnMountEquipUpdate();
            SetRedTipState();
        }
    }

    /// <summary>
    /// 服务端坐骑装备数据返回 by邓成
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_OnUpdateEquipData(Pt _info)
    {
        //Debug.Log("S2C_OnUpdateEquipData");
        pt_mequip_list_update_d576 pt_mequip_list_update_d576 = _info as pt_mequip_list_update_d576;
        if (pt_mequip_list_update_d576 != null)
        {
            int count = pt_mequip_list_update_d576.items.Count;
            for (int i = 0; i < count; i++)
            {
                EquipmentInfo info = new EquipmentInfo(pt_mequip_list_update_d576.items[i]);
                //Debug.Log("pos:" + info.Postion + ",instance:" + info.InstanceID + ",StackCurCount:" + info.StackCurCount);
                if (!mountEquipDic.ContainsKey(info.Slot))
                {
                    mountEquipDic[info.Slot] = info;
                }
                else
                {
                    CheckUpdateResult(mountEquipDic[info.Slot], info);
                    mountEquipDic[info.Slot].Update(pt_mequip_list_update_d576.items[i], EquipmentBelongTo.EQUIP);
                }
            }
            if (OnMountEquipUpdate != null)
                OnMountEquipUpdate();
            SetRedTipState();
        }
    }

    protected void CheckUpdateResult(EquipmentInfo _oldOne,EquipmentInfo _newOne)
    {
        if(_oldOne.InstanceID != _newOne.InstanceID)return;
        if (_oldOne.StackCurCount == 0 || _newOne.StackCurCount == 0) return;
        if (_oldOne.Quality < _newOne.Quality)//升品
        {
            if (OnUpgradeEquipmentUpdateEvent != null)
                OnUpgradeEquipmentUpdateEvent();
            GameCenter.messageMng.AddClientMsg(485);
        }
        else
        {
            if (OnStrengthEquipmentEvent != null) 
                OnStrengthEquipmentEvent(_oldOne.UpgradeLv < _newOne.UpgradeLv);
            if (_oldOne.UpgradeLv < _newOne.UpgradeLv) GameCenter.messageMng.AddClientMsg(484);
        }
    }
    #endregion

    #region C2S

    /// <summary>
    /// 坐骑列表请求协议(1 坐骑列表 2 皮肤列表) 
    /// </summary>
    public void C2S_GetMountList(MountType type)
    {
        //Debug.Log(" C2S_GetMountList  " + (int)type);
        pt_req_ride_info_d430 msg = new pt_req_ride_info_d430();
        msg.state = (int)type;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 請求上、下馬，穿脱皮肤
    /// </summary>
    public void C2S_ReqRideMount(ChangeMount _state, int _id, MountReqRideType _ridestate)
    {
        if (_state == ChangeMount.RIDEMOUNT)
        {
            SceneUiType sceneUiType = GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType;
            if (GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneType == SceneType.ARENA || sceneUiType == SceneUiType.RAIDERARK || sceneUiType == SceneUiType.BATTLEFIGHT)
            {
                //Debug.Log(" 竞技场  ");
                if (_ridestate == MountReqRideType.SELT)
                {
                    GameCenter.messageMng.AddClientMsg(483);
                }
                return;
            }
        }
        if (_state == ChangeMount.DOWNRIDE || _state == ChangeMount.RIDEMOUNT)
        {
            if (!mountInfoList.ContainsKey(_id))
            {
                _id = rideType;
            } 
        } 
        if (_state == ChangeMount.PUTSKIN)//只有骑着坐骑时才能穿皮肤
        {
            if (state != 1)
            {
                GameCenter.messageMng.AddClientMsg(366);
                return;
            } 
        }
        //Debug.Log("  请求 ： " + _state + "   , id : " + _id + "    type: " + GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneType + "   , uitype : " + GameCenter.mainPlayerMng.MainPlayerInfo.CurSceneUiType);
        pt_req_change_ride_d432 msg = new pt_req_change_ride_d432();
        msg.state = (int)_state;
        msg.id = _id;
        msg.req_state = (int)_ridestate;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 請求培養坐騎
    /// </summary>
    public void C2S_ReqPromoteMount(int _isAutoBuy)
    { 
        pt_req_up_ride_lev_d431 msg = new pt_req_up_ride_lev_d431(); 
        msg.quick_buy = _isAutoBuy;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求激活坐骑
    /// </summary>
    public void C2S_ReqActivateMount() 
    {
        C2S_GetMountList(MountType.ACTIVATE);
    }
    /// <summary>
    /// 强化骑装
    /// </summary>
    public void C2S_ReqStrengthMountEquip(int _itemID,int _type,bool _isQuickBuy)
    {
        //Debug.Log("C2S_ReqStrengthMountEquip _itemID:" + _itemID + ",_isQuickBuy:" + _isQuickBuy);
        pt_mequip_strengthen_d572 msg = new pt_mequip_strengthen_d572();
        msg.item_id = (uint)_itemID;
        msg.type = (byte)_type;
        msg.quick_buy = (byte)(_isQuickBuy ? 1 : 0);
        NetMsgMng.SendMsg(msg);
    }

    /// <summary>
    /// 骑装升品
    /// </summary>
    public void C2S_ReqUpgradeMountEquip(int _itemID)
    {
        //Debug.Log("C2S_ReqUpgradeMountEquip _itemID:" + _itemID);
        pt_mequip_upgrade_d573 msg = new pt_mequip_upgrade_d573();
        msg.item_id = (uint)_itemID;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 穿戴骑装
    /// </summary>
    public void C2S_ReqWieldMountEquip(int _instance)
    {
        //Debug.Log("C2S_ReqWieldMountEquip _instance:" + _instance);
        pt_mequip_wield_d570 msg = new pt_mequip_wield_d570();
        msg.item_id = (uint)_instance;
        NetMsgMng.SendMsg(msg);
    }

    /// <summary>
    /// 脱骑装
    /// </summary>
    public void C2S_ReqUnWieldMountEquip(int _instance)
    {
        //Debug.Log("C2S_ReqUnWieldMountEquip _instance:" + _instance);
        pt_mequip_unwield_d571 msg = new pt_mequip_unwield_d571();
        msg.item_id = (uint)_instance;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求骑装数据
    /// </summary>
    public void C2S_ReqMountEquipList()
    {
        //Debug.Log("C2S_ReqMountEquipList");
        pt_mequip_list_req_d574 msg = new pt_mequip_list_req_d574();
        NetMsgMng.SendMsg(msg);
    }
    #endregion
}

public enum MountType
{ 
    NONE,
    /// <summary>
    /// 坐騎
    /// </summary>
    MOUNTLIST = 1,
    /// <summary>
    /// 幻化
    /// </summary>
    SKINLIST = 2, 
    /// <summary>
    /// 激活
    /// </summary>
    ACTIVATE = 3,
}
public enum ChangeMount
{ 
    /// <summary>
    /// 上馬
    /// </summary>
    RIDEMOUNT = 1,
    /// <summary>
    /// 下馬 
    /// </summary>
    DOWNRIDE = 0,
    /// <summary>
    /// 穿皮肤
    /// </summary>
    PUTSKIN = 2,
    /// <summary>
    /// 脱皮肤
    /// </summary>
    DOWNSKIN = 3,
    /// <summary>
    /// 坐骑化形
    /// </summary>
    ILLUSION = 4,
}
/// <summary>
/// 请求上下马类型 0 是自己手动点下马  1是放技能 被攻击  采集时候
/// </summary>
public enum MountReqRideType
{
    /// <summary>
    /// 手动在界面上下马
    /// </summary>
    SELT = 0,
    /// <summary>
    /// 自动上下马
    /// </summary>
    AUTO = 1,
}
 
