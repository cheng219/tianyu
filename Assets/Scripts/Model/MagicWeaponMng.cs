//===============================
//日期：2016/3/7
//作者：鲁家旗
//用途描述:法宝管理类
//===============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;
public class MagicWeaponMng
{
    #region 数据

    /// <summary>
    /// 法宝数据
    /// </summary>
    public FDictionary magicInfoDic = new FDictionary();
    /// <summary>
    /// 当前使用的法宝发生变化事件
    /// </summary>
    public System.Action OnCurMagicWeaponInfoUpdate;
    protected MagicWeaponInfo curMagicWeaponInfo = null;
    /// <summary>
    /// 当前使用的法宝的数据
    /// </summary>
    public MagicWeaponInfo CurMagicWeaponInfo
    {
        get
        {
            return curMagicWeaponInfo;
        }
        protected set
        {
            if (value == null)
            {
                UpdateMagicWeaponShow(false);
            }
            curMagicWeaponInfo = value;
            if (curMagicWeaponInfo != null)
            {
                UpdateMagicWeaponShow(true);
            }
            if (OnCurMagicWeaponInfoUpdate != null)
            {
                OnCurMagicWeaponInfoUpdate();
            }
        }
    }
    /// <summary>
    /// 法宝属性变化的事件
    /// </summary>
    public System.Action OnMagicTypeUpdate;
    /// <summary>
    /// 新的淬炼等阶
    /// </summary>
    public int newRefineLev = 0;
    /// <summary>
    /// 新的注灵等阶
    /// </summary>
    public int newAddSoulLev = 0;
    /// <summary>
    /// 新的淬炼星阶
    /// </summary>
    public int newRefineStar = 0;
    /// <summary>
    /// 新的注灵星阶
    /// </summary>
    public int newAddSoulStar = 0;
    protected bool isFirst = true;

    /// <summary>
    /// 进度条变化抛出事件 播放特效(因所有数据都在一个协议返回，加个变量控制只有在升级的时候才抛出)
    /// </summary>
    protected bool isAddExp = false;
    public System.Action OnProgressChange;

    /// <summary>
    /// 法宝最高等级
    /// </summary>
    public int maxLev = 0;
    /// <summary>
    /// 法宝最高星级
    /// </summary>
    public int maxStar = 0;
    #endregion

    #region 构造
    /// <summary>
    /// 返回一个全新的管理类对象实例
    /// </summary>
    /// <returns></returns>
    public static MagicWeaponMng CreateNew(MainPlayerMng _father)
    {
        if (_father == null)
        {
            GameSys.LogError("法宝管理类必须从属于一个玩家");
            return null;
        }
        if (_father.magicWeaponMng == null)
        {
            MagicWeaponMng magicWeaponMng = new MagicWeaponMng();
            magicWeaponMng.Init(_father);
            return magicWeaponMng;
        }
        else
        {
            _father.magicWeaponMng.UnRegist(_father);
            _father.magicWeaponMng.Init(_father);
            return _father.magicWeaponMng;
        }
    }
   
    /// <summary>
    /// 初始化（包含协议注册）
    /// </summary>
    protected void Init(MainPlayerMng _father)
    {
        foreach (MagicWeaponRef data in ConfigMng.Instance.MagicWeaponRefTabel.Values)
        {
            magicInfoDic[data.id] = new MagicWeaponInfo(data);
        }
        MsgHander.Regist(0xD309, S2C_GetMagicData);
    }

    /// <summary>
    /// 注销（包含清空和还原数据）
    /// </summary>
    protected void UnRegist(MainPlayerMng _father)
    {
        MsgHander.UnRegist(0xD309, S2C_GetMagicData);
        magicInfoDic.Clear();
        newRefineLev = 0;
        newAddSoulLev = 0;
        newRefineStar = 0;
        newAddSoulStar = 0;
        isFirst = true;
        isAddExp = false;
    }
    #endregion

    #region 协议
    #region S2C 服务端发往客户端的协议处理
    protected void S2C_GetMagicData(Pt _msg)
    {
        //Debug.logger.Log("接收pt_magic_weapons_state_d309 协议");
        pt_magic_weapons_state_d309 msg = _msg as pt_magic_weapons_state_d309;
        if (msg != null)
        {
            for (int i = 0,max=msg.magic_weapons.Count; i < max; i++)
            {
                magic_weapons_state data = msg.magic_weapons[i];
                //Debug.Log("类型 " + data.type + "激活 " + data.active_state + "佩戴 " + data.equ_state + "等级 " + data.strenth_lev + "星级 " + data.strenth_star);
                //保存最新的等级
                newRefineLev = data.strenth_lev;
                newAddSoulLev = data.zhuling_lev;
                newRefineStar = data.strenth_star;
                newAddSoulStar = data.zhuling_star;
                if (magicInfoDic.ContainsKey(data.type))
                {
                    MagicWeaponInfo info = magicInfoDic[data.type] as MagicWeaponInfo;
                    if (info != null)
                    {
                        info.Update(data);
                        if (info.EquState)
                        {
                            CurMagicWeaponInfo = info;
                        }
                        else if (CurMagicWeaponInfo != null && !CurMagicWeaponInfo.EquState)
                        {
                            CurMagicWeaponInfo = null;
                        }
                    }
                    else
                    {
                        Debug.Log("法宝信息为空！");
                    }
                }
                else
                {
                    //如果原列表中没有这个数据，直接以服务端数据构造，添加到列表中去
                    MagicWeaponInfo info = new MagicWeaponInfo(data);
                    if (info.EquState)
                    {
                        CurMagicWeaponInfo = info;
                    }
                    magicInfoDic[info.Type] = info;
                }
            }
            if (isFirst)
            {
                maxLev = MaxMagicLev();
                maxStar = MaxMagicStar();
                SetMagicRedPoint();
                isFirst = false;
            }
        }
        //进度变化事件
        if (isAddExp && OnProgressChange != null)
        {
            OnProgressChange();
            isAddExp = false;
        }
        //抛出法宝属性变化的事件
        if (OnMagicTypeUpdate != null)
            OnMagicTypeUpdate();
    }
    #endregion
    #region C2S 客户端发往服务端的协议处理
    /// <summary>
    /// 向服务器获取法宝列表
    /// </summary>
    public void C2S_RequestGetMagic()
    {
        pt_req_magic_weapons_state_d310 msg = new pt_req_magic_weapons_state_d310();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 向服务器请求激活法宝
    /// </summary>
    /// <param name="_id"></param>
    public void C2S_RequestActiveMagic(int _id )
    {
        pt_req_magic_weapon_active_d303 msg = new pt_req_magic_weapon_active_d303(); 
        msg.id = _id;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 向服务器请求佩戴法宝
    /// </summary>
    public void C2S_RequestWearMagic(int _id )
    {
        pt_req_magic_weapon_equ_d308 msg = new pt_req_magic_weapon_equ_d308();
        msg.id = _id;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 向服务器请求卸下法宝
    /// </summary>
    /// <param name="_id"></param>
    public void C2S_RequestUnloadMagic(int _id)
    {
        pt_req_magic_weapon_dump_d307 msg = new pt_req_magic_weapon_dump_d307();
        msg.id = _id;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 向服务器请求法宝淬炼
    /// </summary>
    /// <param name="_id"></param>
    public void C2S_RequestAddMagicStar(int _id ,bool _type)
    {
        pt_req_magic_weapon_add_star_d306 msg = new pt_req_magic_weapon_add_star_d306();
        msg.id = _id;
        msg.quik_buy = _type ? 1 : 0;
        NetMsgMng.SendMsg(msg);
        isAddExp = true;
    }
    /// <summary>
    /// 向服务器请求法宝注灵
    /// </summary>
    /// <param name="_id"></param>
    public void C2S_RequestAddMagicSoul(int _id, bool _type)
    {
        pt_req_magic_weapon_add_mana_d305 msg = new pt_req_magic_weapon_add_mana_d305();
        msg.id = _id;
        msg.quik_buy = _type ? 1 :0;
        NetMsgMng.SendMsg(msg);
        isAddExp = true;
    }
    #endregion
    #endregion

    #region 辅助逻辑
    /// <summary>
    /// 3D穿戴法宝
    /// </summary>
    /// <param name="_equip"></param>
    protected void UpdateMagicWeaponShow(bool _equip)
    {
        if (CurMagicWeaponInfo != null)
        {
            if (GameCenter.mainPlayerMng != null && GameCenter.mainPlayerMng.MainPlayerInfo != null)
            {
                GameCenter.mainPlayerMng.MainPlayerInfo.UpdateEquipment(CurMagicWeaponInfo.ItemID, _equip);
            }
        }
    }
    /// <summary>
    /// 是否显示红点
    /// </summary>
    public void SetMagicRedPoint()
    {
        bool needRedTip = false;
        foreach (MagicWeaponInfo info in magicInfoDic.Values)
        {
            if (info.EquActive && info.ConsumeEnough)
            {
                //设置红点,有一个满足条件就显示红点
                needRedTip = true;
                break;
                
            }
        }
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.MAGIC, needRedTip);
    }
    public void SetMagicRedPoint(ActorBaseTag tag, ulong value, bool _fromAbility)
    {
        if (tag == ActorBaseTag.BindCoin || tag == ActorBaseTag.UnBindCoin)
        {
            SetMagicRedPoint();
        }
    }

    List<RefineRef> list = new List<RefineRef>();
    void SortMaigcData()
    {
        FDictionary table = ConfigMng.Instance.GetRefineRefTable();
        foreach (RefineRef data in table.Values)
        {
            if (data.relationID == 1)
            {
                list.Add(data);
            }
        }
        list.Sort(SortMagic);
    }
    /// <summary>
    /// 法宝最高等阶
    /// </summary>
    /// <returns></returns>
    public int MaxMagicLev()
    {
        SortMaigcData();
        return list[list.Count - 1].stage;
    }
    /// <summary>
    /// 法宝最高星阶
    /// </summary>
    /// <returns></returns>
    public int MaxMagicStar()
    {
        SortMaigcData();
        return list[list.Count - 1].star;
    }
    /// <summary>
    /// 排序
    /// </summary>
    public int SortMagic(RefineRef _data1, RefineRef _data2)
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
    #endregion
}
