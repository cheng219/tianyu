//==================================
//作者：鲁家旗
//日期：2016/3/7
//用途：法宝数据层
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

public class MagicWeaponInfo 
{
    #region 服务端数据
    protected magic_weapons_state serverData;
    #endregion
    #region 客户端数据
    protected MagicWeaponRef magicRefData = null;
    public MagicWeaponRef MagicRefData
    {
        get
        {
            if (magicRefData == null && serverData != null)
            {
                magicRefData = ConfigMng.Instance.GetMagicWeaponRef(serverData.type);
            }
            return magicRefData;
        }
    }
    #endregion


    #region 构造
    /// <summary>
    /// 通过后台数据来构造，从无到有
    /// </summary>
    /// <param name="_msg"></param>
    public MagicWeaponInfo(magic_weapons_state _data)
    {
        serverData = _data;
    }
    /// <summary>
    /// 通过静态配置来构造，从无到有
    /// </summary>
    /// <param name="_refData"></param>
    public MagicWeaponInfo(MagicWeaponRef _magicRefData)
    {
        magicRefData = _magicRefData;
    }
    /// <summary>
    /// 通过后台数据来修正，已有 
    /// </summary>
    /// <param name="_msg"></param>
    public void Update(magic_weapons_state _data)
    {
        if (serverData == null)
        {
            serverData = _data;
        }
        else
        {
            serverData.id = _data.id;
            serverData.active_state = _data.active_state;
            serverData.equ_state = _data.equ_state;
            serverData.strenth_star = _data.strenth_star;
            serverData.strenth_exp = _data.strenth_exp;
            serverData.strenth_lev = _data.strenth_lev;
            serverData.zhuling_exp = _data.zhuling_exp;
            serverData.zhuling_lev = _data.zhuling_lev;
            serverData.zhuling_star = _data.zhuling_star;
        }
    }
    #endregion

    #region 访问器
    /// <summary>
    /// 服务器唯一ID
    /// </summary>
    public int ConfigID
    { 
        
        get { return serverData == null ? 0 : serverData.id; }
    }
    /// <summary>
    /// 法宝对应的物品id
    /// </summary>
    public int MagicId
    {
        get
        {
            return MagicRefData == null? 0 : MagicRefData.itemId;
        }
    }
    /// <summary>
    /// 法宝图片
    /// </summary>
    public string IconName
    {
        get { return MagicRefData == null ? string.Empty : MagicRefData.icon; }
    }
    /// <summary>
    /// 法宝名字
    /// </summary>
    public string Name
    {
        get
        {
            if (MagicRefData != null)
            {
                EquipmentRef equipRef = ConfigMng.Instance.GetEquipmentRef(MagicRefData.itemId);
                return equipRef == null ? string.Empty : equipRef.name;
            }
            return string.Empty;
        }
    }
    /// <summary>
    /// 法宝的relationID
    /// </summary>
    public int Type
    {
        get
        {
            return MagicRefData == null ? 0 : MagicRefData.id;
        }
    }
    /// <summary>
    /// 激活状态
    /// </summary>
    public bool EquActive
    {
        get
        {
            return serverData == null ? false : serverData.active_state != 0 ;
        }
    }
    /// <summary>
    /// 佩戴状态
    /// </summary>
    public bool EquState
    {
        get
        {
            return serverData == null ?false : serverData.equ_state != 0;
        }
    }
    #region 法宝淬炼
    /// <summary>
    /// 当前淬炼数据
    /// </summary>
    public RefineRef RefineData
    {
        get
        {
            RefineRef refineData  = null;
            if (serverData != null)
            {
                refineData = ConfigMng.Instance.GetRefineRef(serverData.type, serverData.strenth_lev, serverData.strenth_star);
            }
            else if (MagicRefData != null)
            {
                refineData = ConfigMng.Instance.GetRefineRef(MagicRefData.id, 1, 0);
            }
            return refineData;
        }
    }
    /// <summary>
    /// 淬炼的下一级数据
    /// </summary>
    public RefineRef RefineNextData
    {
        get
        {
            if (serverData != null && serverData.strenth_star < 9)
            {
                RefineRef refineData = ConfigMng.Instance.GetRefineRef(serverData.type, serverData.strenth_lev, serverData.strenth_star + 1);
                return refineData;
            }
            else
            {
                RefineRef refineData = ConfigMng.Instance.GetRefineRef(serverData.type, serverData.strenth_lev + 1, serverData.strenth_star - serverData.strenth_star);
                return refineData;
            }
        }
    }

    /// <summary>
    /// 淬炼等级
    /// </summary>
    public int RefineLev
    {
        get
        {
            return serverData == null ? 0: serverData.strenth_lev;
        }
    }
    /// <summary>
    /// 淬炼星级
    /// </summary>
    public int RefineStar
    {
        get
        {
            return serverData == null ? 0 : serverData.strenth_star;
        }
    }
    ///<summary>
    ///淬炼标签等级名
    ///</summary>
    public string RefineStageTag
    {
        get 
        {
            return RefineData == null ? string.Empty : RefineData.labelName;
        }
    }
    /// <summary>
    /// 淬炼战斗力
    /// </summary>
    public int RefineFightPower
    {
        get
        {
            return RefineData == null ? 0 : RefineData.fighting; 
        }
    }
    /// <summary>
    /// 战力增加值
    /// </summary>
    public int RefineAddFightPower
    {
        get
        {
            return (RefineNextData == null ||RefineData == null) ? 0 : RefineNextData.fighting - RefineData.fighting;
        }
    }
    /// <summary>
    /// 淬炼属性
    /// </summary>
    public List<string> RefineAttributeType
    {
        get
        {
            List<string> list = new List<string>();
            for (int i = 0; i < RefineData.attributeId.Count; i++)
            {
                int id = RefineData.attributeId[i];
                ActorPropertyTag tag = (ActorPropertyTag)Enum.Parse(typeof(ActorPropertyTag), id+"");
                AttributeTypeRef attributeTypeRef = ConfigMng.Instance.GetAttributeTypeRef(tag);
                if (attributeTypeRef != null) list.Add(attributeTypeRef.stats);
            }
            return list;
        }
    }
    /// <summary>
    /// 静态表淬炼属性值
    /// </summary>
    public List<int> RefineAttributeNum
    {
        get
        {
            return RefineData == null ? new List<int>() : RefineData.attributeNum;
        }
    }

    /// <summary>
    /// 淬炼增加的属性值
    /// </summary>
    public List<int> RefineAddAttribute
    {
        get
        {
            List<int> list = new List<int>();
            for (int i = 0; i < RefineAttributeNum.Count ; i++)
            {
                if (RefineNextData != null && RefineData != null)
                    list.Add(RefineNextData.attributeNum[i] - RefineData.attributeNum[i]);
            }
            
            return list;
        }
    }
    /// <summary>
    /// 模型ID
    /// </summary>
    public int ItemID
    {
        get
        {
            return RefineData == null ? 0 : RefineData.model;
        }
    }
    /// <summary>
    /// 下一级模型ID
    /// </summary>
    public int NextItemID
    {
        get
        {
            return RefineNextData == null ? 0 : RefineNextData.model;
        }
    }
    /// <summary>
    /// 淬炼随机经验值
    /// </summary>
    public int RefineRandomExp
    {
        get
        {
            return serverData==null ? 0 : serverData.strenth_exp;
        }
    }
    /// <summary>
    /// 淬炼消耗品ID
    /// </summary>
    public List<int> ConsumId
    {
        get
        {
            return RefineData == null ? new List<int>() : RefineData.consume;
        }
    }
    /// <summary>
    /// 淬炼消耗铜钱ID
    /// </summary>
    public int ConsumeCoinId
    {
        get
        {
            return ConsumId.Count >= 2 ? ConsumId[0] : 5;
        }
    }
    /// <summary>
    /// 淬炼消耗材料物品ID
    /// </summary>
    public int ConsumeItemId
    {
        get
        {
            return ConsumId.Count >= 2 ? ConsumId[1] : 2300003;
        }
    }
    /// <summary>
    /// 淬炼消耗品名字
    /// </summary>
    public List<string> ConsumeNameList
    {
        get
        {
            List<string> list = new List<string>();
            for (int i = 0; i < RefineData.consume.Count; i++)
            {
                if (RefineData != null)
                    list.Add(ConfigMng.Instance.GetEquipmentRef(RefineData.consume[i]).name);
            }
            return list;
        }
    }
    /// <summary>
    /// 淬炼消耗品图片
    /// </summary>
    public List<string> ConsumeIconList
    {
        get
        {
            List<string> list = new List<string>();
            for (int i = 0; i < RefineData.consume.Count; i++)
            {
                if (RefineData != null)
                {
                    EquipmentRef equipRef = ConfigMng.Instance.GetEquipmentRef(RefineData.consume[i]);
                    if(equipRef != null)
                        list.Add(equipRef.item_res);
                }
            }
            return list;
        }
    }
    /// <summary>
    /// 淬炼消耗品数量
    /// </summary>
    public List<int> ConsumeNumList
    {
        get
        {
            return RefineData == null ? new List<int>(): RefineData.consumeNum;
        }
    }
    /// <summary>
    /// 淬炼消耗铜钱数量
    /// </summary>
    public int ConsumeCoinNum
    {
        get
        {
            return ConsumeNumList.Count >= 2 ? ConsumeNumList[0] : 2000;
        }
    }
    /// <summary>
    /// 淬炼消耗物品数量
    /// </summary>
    public int ConsumeItemNum
    {
        get
        {
            return ConsumeNumList.Count >= 2 ? ConsumeNumList[1] : 1;
        }
    }
    /// <summary>
    /// 淬炼材料消耗品总价格(铜币可以购买的情况下)
    /// </summary>
    //public int ConsumePrice
    //{
    //    get
    //    {
    //        long num = GameCenter.mainPlayerMng.MainPlayerInfo.UnBindCoinCount + GameCenter.mainPlayerMng.MainPlayerInfo.BindCoinCount;
    //        //材料不足，消耗总价格
    //        if (GameCenter.inventoryMng.GetNumberByType(ConsumId[1]) < ConsumeNumList[1])
    //        {
    //            if (num > ConsumeNumList[0])
    //            {
    //                return (int)(ConfigMng.Instance.GetEquipmentRef(ConsumId[1]).diamonPrice * ConsumeNumList[1]);
    //            }
    //            else if (num > 0 && num < ConsumeNumList[0])
    //            {
    //                return (int)((ConfigMng.Instance.GetEquipmentRef(ConsumId[1]).diamonPrice * ConsumeNumList[1]) +
    //                   ConfigMng.Instance.GetEquipmentRef(ConsumId[0]).diamonPrice * (ConsumeNumList[0]-num));
    //            }
    //            else
    //            {
    //                return (int)(ConfigMng.Instance.GetEquipmentRef(ConsumId[1]).diamonPrice * ConsumeNumList[1] +
    //                    ConfigMng.Instance.GetEquipmentRef(ConsumId[0]).diamonPrice * ConsumeNumList[0]);
    //            }
    //        }
    //        else if (num > 0 && num < ConsumeNumList[0])
    //        { 
    //            return (int)(ConfigMng.Instance.GetEquipmentRef(ConsumId[0]).diamonPrice * (ConsumeNumList[0]-num));
    //        }
    //        else if (num >= ConsumeNumList[0])
    //        {
    //            return 0;
    //        }
    //        else
    //            return (int)(ConfigMng.Instance.GetEquipmentRef(ConsumId[0]).diamonPrice * (ConsumeNumList[0]));
    //    }
    //}
    /// <summary>
    /// 淬炼材料消耗品总价格(铜币不可以购买的情况下)
    /// </summary>
    public int ConsumePrice
    {
        get
        { 
            int num = GameCenter.inventoryMng.GetNumberByType(ConsumeItemId);
            EquipmentRef equipRef = ConfigMng.Instance.GetEquipmentRef(ConsumeItemId);
            if (num == 0)
                return equipRef == null ? 0 : (int)(equipRef.diamonPrice * ConsumeItemNum);
            else if (num > 0 && num < ConsumeItemNum)
                return equipRef == null ? 0 : (int)(equipRef.diamonPrice * (ConsumeItemNum - num));
            else
                return 0;
        }
    }
    /// <summary>
    /// 是否达到淬炼最大等级
    /// </summary>
    bool RefineMaxLev
    {
        get
        {
            return (RefineLev == 9 && RefineStar == 9);
        }
    }
    #endregion

    #region 法宝注灵
    /// <summary>
    /// 注灵当前数据
    /// </summary>
    public AddSoulRef AddSoulData
    {
        get
        {
            AddSoulRef addSoulData = null;
            if(serverData != null)
                 addSoulData = ConfigMng.Instance.GetAddSoulRef(serverData.type, serverData.zhuling_lev, serverData.zhuling_star);
            else
                addSoulData = ConfigMng.Instance.GetAddSoulRef(MagicRefData.id, 1, 0);
            return addSoulData;
        }
    }
    /// <summary>
    /// 注灵的下一级数据
    /// </summary>
    public AddSoulRef AddSoulNextData
    {
        get
        {
            if (serverData.zhuling_star < 9)
            {
                AddSoulRef addsoulData = ConfigMng.Instance.GetAddSoulRef(serverData.type, serverData.zhuling_lev, serverData.zhuling_star + 1);
                return addsoulData;
            }
            else
            {
                AddSoulRef addsoulData = ConfigMng.Instance.GetAddSoulRef(serverData.type, serverData.zhuling_lev + 1, serverData.zhuling_star - serverData.zhuling_star);
                return addsoulData;
            }
        }
    }
    /// <summary>
    /// 注灵标签等级名
    /// </summary>
    public string AddSoulStageTag
    {
        get 
        {
            return AddSoulData == null ? string.Empty : AddSoulData.labelName;
        }
    }
    /// <summary>
    /// 注灵战斗力
    /// </summary>
    public int AddSoulFightPower
    {
        get 
        {
            return AddSoulData == null ? 0 : AddSoulData.fighting;
        }
    }
    /// <summary>
    /// 增加的战斗力
    /// </summary>
    public int AddSoulAddFightPower
    {
        get
        {
            return (AddSoulNextData == null||AddSoulData == null) ? 0 : AddSoulNextData.fighting - AddSoulData.fighting;
        }
    }
    /// <summary>
    /// 注灵属性
    /// </summary>
    public List<string> AddSoulAttributeType
    {
        get
        {
            List<string> list = new List<string>();
            for (int i = 0; i < AddSoulData.attributeId.Count; i++)
            {
                int id = (int)AddSoulData.attributeId[i];
                AttributeTypeRef attributeTypeRef = ConfigMng.Instance.GetAttributeTypeRef((ActorPropertyTag)Enum.ToObject(typeof(ActorPropertyTag), id));
                if(attributeTypeRef != null)
                    list.Add(attributeTypeRef.stats);
            }
            return list;
        }
    }

    /// <summary>
    /// 静态表注灵属性值
    /// </summary>
    public List<int> AddSoulAttributeNum
    {
        get
        {
            return AddSoulData == null ? new List<int>() : AddSoulData.attributeNum;
        }
    }

    /// <summary>
    /// 注灵增加的属性值
    /// </summary>
    public List<int> AddSoulAddAttribute
    {
        get
        {
            List<int> list = new List<int>();
            for (int i = 0; i < AddSoulAttributeNum.Count ; i++)
            {
                if (AddSoulNextData != null && AddSoulData != null)
                    list.Add(AddSoulNextData.attributeNum[i] - AddSoulData.attributeNum[i]);
            }
            return list;
        }
    }

    /// <summary>
    /// 注灵经验
    /// </summary>
    public int AddSoulExp
    {
        get
        {
            return serverData == null ? 0 : serverData.zhuling_exp;
        }
    }
    /// <summary>
    /// 注灵等级
    /// </summary>
    public int AddSoulLev
    {
        get
        {
            return serverData == null ? 0 : serverData.zhuling_lev;
        }
    }
    /// <summary>
    /// 注灵星级
    /// </summary>
    public int AddSoulStar
    {
        get
        {
            return serverData == null ? 0 : serverData.zhuling_star;
        }
    }
    /// <summary>
    /// 技能ID
    /// </summary>
    public int SkillID
    {
        get
        {
            return AddSoulData == null ? 0 : AddSoulData.skill.skillid; 
        }
    }
    /// <summary>
    /// 技能等级
    /// </summary>
    public int SkillLev
    {
        get
        {
            return AddSoulData == null ? 0 : AddSoulData.skill.skilllev;
        }
    }
    /// <summary>
    /// 技能图片
    /// </summary>
    public string SkillIcon
    {
        get
        {
            return MagicRefData == null ? string.Empty : MagicRefData.skillIcon;
        }
    }
    /// <summary>
    /// 技能描述
    /// </summary>
    public string SkillDes
    {
        get
        {
            return AddSoulData == null ? string.Empty : AddSoulData.skillDescribe;
        }
    }

    /// <summary>
    /// 注灵消耗品ID
    /// </summary>
    public List<int> AddSoulConsumId
    {
        get
        {
            return AddSoulData == null ? new List<int>():AddSoulData.consume;
        }
    }
    /// <summary>
    /// 注灵消耗铜钱ID
    /// </summary>
    public int AddSoulConsumeCoinId
    {
        get
        {
            return AddSoulConsumId.Count >= 2 ? AddSoulConsumId[0] : 5;
        }
    }
    /// <summary>
    /// 注灵消耗材料ID
    /// </summary>
    public int AddSoulConsumeItemId
    {
        get
        {
            return AddSoulConsumId.Count >= 2 ? AddSoulConsumId[1] : 2300004;
        }
    }
    /// <summary>
    /// 注灵消耗品名字
    /// </summary>
    public List<string> AddSoulConsumeNameList
    {
        get
        {
            List<string> list = new List<string>();
            for (int i = 0; i < AddSoulData.consume.Count; i++)
            {
                if (AddSoulData != null)
                {
                    EquipmentRef equipRef = ConfigMng.Instance.GetEquipmentRef(AddSoulData.consume[i]);
                    if(equipRef != null)
                        list.Add(equipRef.name);
                }
            }
            return list;
        }
    }
    /// <summary>
    /// 注灵消耗品图片
    /// </summary>
    public List<string> AddSoulConsumeIconList
    {
        get
        {
            List<string> list = new List<string>();
            for (int i = 0; i < AddSoulData.consume.Count; i++)
            {
                if (AddSoulData != null)
                {
                    EquipmentRef equipRef = ConfigMng.Instance.GetEquipmentRef(AddSoulData.consume[i]);
                    if (equipRef != null)
                        list.Add(equipRef.item_res);
                }
            }
            return list;
        }
    }
    /// <summary>
    /// 注灵消耗品数量
    /// </summary>
    public List<int> AddSoulConsumeNumList
    {
        get
        {
            return AddSoulData == null ? new List<int>(): AddSoulData.consumeNum;
        }
    }
    /// <summary>
    /// 注灵消耗铜钱数量
    /// </summary>
    public int AddSoulConsumeCoinNum
    {
        get
        {
            return AddSoulConsumeNumList.Count >= 2 ? AddSoulConsumeNumList[0] : 8000;
        }
    }
    /// <summary>
    /// 注灵消耗材料数量
    /// </summary>
    public int AddSoulConsumeItemNum
    {
        get
        {
            return AddSoulConsumeNumList.Count >= 2 ? AddSoulConsumeNumList[1] : 1;
        }
    }
    /// <summary>
    /// 注灵材料总价格(铜币可以购买的情况)
    /// </summary>
    //public int AddSoulConsumePrice
    //{
    //    get
    //    {
    //        if ((GameCenter.mainPlayerMng.MainPlayerInfo.UnBindCoinCount + GameCenter.mainPlayerMng.MainPlayerInfo.BindCoinCount) > AddSoulConsumeNumList[0])
    //        {
    //            return (int)(ConfigMng.Instance.GetEquipmentRef(AddSoulConsumId[1]).diamonPrice * AddSoulConsumeNumList[1]);
    //        }
    //        else
    //        {
    //            return (int)(ConfigMng.Instance.GetEquipmentRef(AddSoulConsumId[1]).diamonPrice * AddSoulConsumeNumList[1] +
    //                ConfigMng.Instance.GetEquipmentRef(AddSoulConsumId[0]).diamonPrice * AddSoulConsumeNumList[0]);
    //        }
    //    }
    //}
    /// <summary>
    /// 注灵材料总价格(铜币不可以购买的情况)
    /// </summary>
    public int AddSoulConsumePrice
    {
        get
        {
            int num = GameCenter.inventoryMng.GetNumberByType(AddSoulConsumeItemId);
            EquipmentRef equipRef = ConfigMng.Instance.GetEquipmentRef(AddSoulConsumeItemId);
            if(num == 0)
                return equipRef == null ? 0:(int)(equipRef.diamonPrice * AddSoulConsumeItemNum);
            else if (num > 0 && num < AddSoulConsumeItemNum)
                return equipRef == null ? 0 : (int)(equipRef.diamonPrice * (AddSoulConsumeItemNum - num));
            else
                return 0;
        }
    }
    /// <summary>
    /// 注灵是否达到满级
    /// </summary>
    bool AddSoulMaxLev
    {
        get
        {
            return (AddSoulLev == 9 && AddSoulStar == 9);
        }
    }
    #endregion
    
    /// <summary>
    /// 淬炼或注灵 材料是否充足，红点是否显示
    /// </summary>
    public bool ConsumeEnough
    {
        get
        {
            MainPlayerInfo info = GameCenter.mainPlayerMng.MainPlayerInfo;
            int consumNum = GameCenter.inventoryMng.GetNumberByType(ConsumeItemId);
            int addSoulConsum = GameCenter.inventoryMng.GetNumberByType(AddSoulConsumeItemId);
            if (info != null)
            {
                bool refineEnough = (!RefineMaxLev && consumNum >= ConsumeItemNum && (int)info.TotalCoinCount >= ConsumeCoinNum);
                bool addSoulEnough = (RefineLev >= 2 && !AddSoulMaxLev && addSoulConsum >= AddSoulConsumeItemNum && (int)info.TotalCoinCount >= AddSoulConsumeCoinNum);
                //Debug.Log("name:"+Name+ ",refineEnough:" + refineEnough + ",addSoulEnough:" + addSoulEnough);
                if (refineEnough || addSoulEnough)
                {
                    return true;
                }
            }
            return false;
        }
    }
    #endregion
}
    