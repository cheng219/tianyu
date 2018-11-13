//===================
//作者：鲁家旗
//日期：2016/3/24
//用途：翅膀数据层
//===================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using st.net.NetBase;
public class WingInfo 
{
    #region 服务端数据
    protected wing_base_info serverData;
    #endregion

    #region 客户端数据
    protected WingRef wingRefData = null;
    public WingRef WingRefData
    {
        get
        {
            if (serverData != null && (wingRefData == null || wingRefData.lev != serverData.lev))
            {
                  wingRefData = ConfigMng.Instance.GetWingRef((int)serverData.wing_id,(int)serverData.lev);
            }
            return wingRefData;
        }
    }
    protected WingRef wingNextRefData = null;
    /// <summary>
    /// 翅膀的下一级数据
    /// </summary>
    public WingRef WingNextData
    {
        get
        {
            if (serverData != null)
            {
                if (serverData.lev < ConfigMng.Instance.GetWingMaxLev((int)serverData.wing_id))
                {
                    wingNextRefData = ConfigMng.Instance.GetWingRef((int)serverData.wing_id, (int)serverData.lev + 1);
                }
                else
                {
                    wingNextRefData = ConfigMng.Instance.GetWingRef((int)serverData.wing_id, ConfigMng.Instance.GetWingMaxLev((int)serverData.wing_id));
                }
            }
            return wingNextRefData;
        }
    }
    #endregion

    #region 构造
    /// <summary>
    /// 通过服务端构造
    /// </summary>
    public WingInfo(wing_base_info _data)
    {
        serverData = _data;
    }
    /// <summary>
    /// 通过静态配置构造
    /// </summary>
    /// <param name="_wingRefData"></param>
    public WingInfo(WingRef _wingRefData)
    {
        wingRefData = _wingRefData;
    }
    /// <summary>
    /// 通过后台数据来修正
    /// </summary>
    public void Update(wing_base_info _data)
    {
        if (serverData != null)
        {
            serverData.wing_id = _data.wing_id;
            serverData.put_state = _data.put_state;
            serverData.lev = _data.lev;
            serverData.exp = _data.exp;
            serverData.passivity_skill = _data.passivity_skill;
        }
        else
        {
            serverData = _data;
        }
    }
    /// <summary>
    /// 升级后更新数据
    /// </summary>
    /// <param name="_msg"></param>
    public void Update(pt_update_wing_lev_d420 _msg)
    {
        if (serverData != null)
        {
            serverData.wing_id = (uint)_msg.wing_id;
            if (serverData.lev != (uint)_msg.lev)
            {
                if (GameCenter.wingMng.OnAddLev != null)
                    GameCenter.wingMng.OnAddLev();
            }
            serverData.lev = (uint)_msg.lev;
            serverData.exp = (uint)_msg.exp;
        }
    }
    /// <summary>
    /// 穿戴后更新数据
    /// </summary>
    /// <param name="_msg"></param>
    public void Update(pt_update_wing_state_d443 _msg)
    {
        if (serverData != null)
        {
            serverData.wing_id = (uint)_msg.wing_id;
            serverData.put_state = (uint)_msg.state;
        }
    }
    #endregion

    #region 访问器
    
    /// <summary>
    /// 服务端翅膀ID
    /// </summary>
    public int WingId
    {
        get
        {
            return serverData == null ? 0 : (int)serverData.wing_id;
        }
    }
    /// <summary>
    /// 翅膀等级
    /// </summary>
    public int WingLev
    {
        get
        {
            return serverData == null ? 1 :(int)serverData.lev;
        }
    }
    /// <summary>
    /// 翅膀名称
    /// </summary>
    public string WingName
    {
        get
        {
            return WingRefData == null ? string.Empty : WingRefData.name;
        }
    }
    /// <summary>
    /// 当前翅膀的ID
    /// </summary>
    public int WingItemId
    {
        get
        {
            WingRef refData = null;
            if (serverData != null)
            {
                refData = ConfigMng.Instance.GetWingRef((int)serverData.wing_id, (int)serverData.lev);
            }
            else if(WingRefData != null)
            {
                refData = ConfigMng.Instance.GetWingRef(WingRefData.id, ConfigMng.Instance.GetWingMaxLev(WingRefData.id));
            }
            if (refData == null)
            {
                return 0;
            }
            return refData.itemui;
        }
    }
    /// <summary>
    /// 翅膀类型
    /// </summary>
    public int WingType
    {
        get
        {
            return WingRefData == null ? 1 : WingRefData.type;
        }
    }
    /// <summary>
    /// 翅膀的下一级模型ID
    /// </summary>
    public int WingNextItemId
    {
        get
        {
            return WingNextData == null ? 0 : WingNextData.itemui;
        }
    }

    /// <summary>
    /// 翅膀不同阶段所对应的ID
    /// </summary>
    public List<int> WingItemList
    {
        get
        {
            List<int> list = ConfigMng.Instance.GetWingItemLev((int)serverData.wing_id);
            List<int> idList = new List<int>();
            for(int i=0;i< list.Count;i++)
            {
                WingRef refData = ConfigMng.Instance.GetWingRef((int)serverData.wing_id, list[i]);
                if (refData != null)
                {
                    idList.Add(refData.itemui);
                }
            }
            return idList;
        }
    }
    /// <summary>
    /// 翅膀不同阶段的所有起始等级
    /// </summary>
    public List<int> AllWingItemLev
    {
        get
        {
           List<int> list = ConfigMng.Instance.GetWingItemLev((int)serverData.wing_id);
           return list;
        }
    }
    /// <summary>
    /// 翅膀附加属性
    /// </summary>
    public List<ItemValue> WingAttributeList
    {
        get
        {
            return WingRefData == null ? new List<ItemValue>(): WingRefData.property_list;
        }
    }
    /// <summary>
    /// 属性名
    /// </summary>
    public List<string> WingAttributeName
    {
        get
        {
            List<string> list = new List<string>();
            for (int i = 0; i < WingAttributeList.Count; i++)
            {
                AttributeTypeRef attribute = ConfigMng.Instance.GetAttributeTypeRef((ActorPropertyTag)Enum.ToObject(typeof(ActorPropertyTag), WingAttributeList[i].eid));
                if(attribute != null)
                    list.Add(attribute.stats);
            }
            return list;
        }
    }
    /// <summary>
    /// 属性值
    /// </summary>
    public List<string> WingAttributeNum
    {
        get
        {
            List<string> list = new List<string>();
            for (int i = 0; i < WingAttributeList.Count; i++)
            {
                list.Add(WingAttributeList[i].count.ToString());
            }
            return list;
        }
    }
   /// <summary>
   /// 翅膀最大等级的一条数据
   /// </summary>
    public WingRef refData
    {
        get
        {
            return ConfigMng.Instance.GetWingRef((int)serverData.wing_id, ConfigMng.Instance.GetWingMaxLev((int)serverData.wing_id));
        }
    }
    /// <summary>
    /// 技能图片
    /// </summary>
    public string WingSkillIcon
    {
        get
        {
            SkillMainConfigRef skill = ConfigMng.Instance.GetSkillMainConfigRef(refData.passivity_skill.skillid);
            return skill == null ? string.Empty : skill.skillIcon;
        }
    }
    /// <summary>
    /// 开启条件1 翅膀前置等级达到多少
    /// </summary>
    public List<int> Condition_1
    {
        get 
        {
            return WingRefData == null ? new List<int>() : WingRefData.condition_1;
        }
    }
    /// <summary>
    /// 开启条件2 消耗物品ID Num
    /// </summary>
    public List<int> Condition_2
    {
        get
        {
            return WingRefData == null ? new List<int>():WingRefData.condition_2;
        }
    }

    #region 翅膀激活
    
    /// <summary>
    /// 翅膀的下一级附加属性
    /// </summary>
    public List<ItemValue> WingNextAttributeList
    {
        get
        {
            return WingNextData == null ? new List<ItemValue>() : WingNextData.property_list;
        }
    }
    /// <summary>
    /// 下一级属性值
    /// </summary>
    public List<string> WingNextAttributeNum
    {
        get
        {
            List<string> list = new List<string>();
            for (int i = 0; i < WingNextAttributeList.Count; i++)
            {
                list.Add(WingNextAttributeList[i].count.ToString());
            }
            return list;
        }
    }
    /// <summary>
    /// 翅膀模型
    /// </summary>
    public string WingModel
    {
        get
        {
            return WingRefData == null ? string.Empty : WingRefData.model;
        }
    }
    /// <summary>
    /// 翅膀的技能文字提升
    /// </summary>
    public string WingUIDes
    {
        get
        {
            return WingRefData == null ? string.Empty : WingRefData.skill_des;
        }
    }
    /// <summary>
    /// 技能未激活描述
    /// </summary>
    public string WingSkillNotActiveDes
    {
        get
        {
            return WingRefData == null ? string.Empty : WingRefData.not_active_skill;
        }
    }
    /// <summary>
    /// 技能ID
    /// </summary>
    public int skillId
    {
        get
        {
            return WingRefData == null ? 0 : WingRefData.passivity_skill.skillid;
        }
    }
    /// <summary>
    /// 技能等级
    /// </summary>
    public int skillLev
    {
        get
        {
            return WingRefData == null ? 0 : WingRefData.passivity_skill.skilllev;
        }
    }
    /// <summary>
    /// 技能概率
    /// </summary>
    public int skillLarge
    {
        get
        { 
            PassiveSkillRef skill = ConfigMng.Instance.GetPassiveSkillRef(WingRefData.passivity_skill.skillid);
            return skill == null ? 0: skill.probability;
        }
    }

    /// <summary>
    /// 技能激活描述
    /// </summary>
    public string WingSkillDes
    {
        get
        {
            SkillMainConfigRef skill = ConfigMng.Instance.GetSkillMainConfigRef(WingRefData.passivity_skill.skillid);
            return skill == null ? string.Empty : skill.skillDes;
        }
    }
    /// <summary>
    /// 升级服务端所传经验
    /// </summary>
    public int WingExp
    {
        get
        {
            return serverData == null ? 0 : (int)serverData.exp;
        }
    }
    /// <summary>
    /// 升级所需经验
    /// </summary>
    public int WingNeedExp
    {
        get
        {
            return WingRefData == null ? 0 : WingRefData.exp;
        }
    }
    /// <summary>
    /// 总的经验条
    /// </summary>
    public float CurTotalExp
    {
        get
        {
            return WingRefData == null ? 0 : WingRefData.progress_exp;
        }
    }
    /// <summary>
    /// 淬炼消耗物品列表
    /// </summary>
    public List<ItemValue> WingPromoteList
    {
        get
        {
            return WingRefData == null ? new List<ItemValue>(): WingRefData.up_need_item;
        }
    }
    /// <summary>
    /// 淬炼消耗物品名
    /// </summary>
    public List<string> WingPromoteName
    {
        get
        {
            List<string> list = new List<string>();
            for (int i = 0; i < WingPromoteList.Count; i++)
            {
                EquipmentRef equip = ConfigMng.Instance.GetEquipmentRef(WingPromoteList[i].eid);
                if(equip != null)
                    list.Add( equip.name);
            }
            return list;
        }
    }
    /// <summary>
    /// 淬炼消耗物品图片
    /// </summary>
    public List<string> WingPromoteIcon
    {
        get
        {
            List<string> list = new List<string>();
            for (int i = 0; i < WingPromoteList.Count; i++)
            {
                EquipmentRef equip = ConfigMng.Instance.GetEquipmentRef(WingPromoteList[i].eid);
                if (equip != null)
                    list.Add(equip.item_res);
            }
            return list;
        }
    }
    /// <summary>
    /// 淬炼消耗物品数量
    /// </summary>
    public List<int> WingPromoteNum
    {
        get
        {
            List<int> list = new List<int>();
            for (int i = 0; i < WingPromoteList.Count; i++)
            {
                list.Add(WingPromoteList[i].count);
            }
            return list;
        }
    }
    /// <summary>
    /// 翅膀的穿戴状态 0为装备 1为以装备 true为以装备 false 为未装备
    /// </summary>
    public bool WingState
    {
        get
        {
            return serverData.put_state == 1;
        }
        set
        {
            serverData.put_state = (uint)(value ? 1 : 0);
        }
    }
    /// <summary>
    /// 消耗元宝
    /// </summary>
    public int ConsumeYb
    {
        get
        {
            int num = GameCenter.inventoryMng.GetNumberByType(WingPromoteList[0].eid);
            EquipmentRef equRef = ConfigMng.Instance.GetEquipmentRef(WingPromoteList[0].eid);
            if (num == 0)
                return equRef == null ? 0 : (int)(WingPromoteNum[0] * equRef.diamonPrice);
            else if (num > 0 && num < WingPromoteNum[0])
                return equRef == null ? 0 : (int)((WingPromoteNum[0] - num) * equRef.diamonPrice);
            else
                return 0;
        }
    }
    /// <summary>
    /// 激活翅膀的材料是否充足
    /// </summary>
    public bool WingActiveEnough
    {
        get
        {
            int num = GameCenter.inventoryMng.GetNumberByType(Condition_2[0]);
            if (num >= Condition_2[1])
            {
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// 材料是否充足，是否显示红点
    /// </summary>
    public bool WingConsumeEnough
    {
        get
        {
            int coinNum = (int)GameCenter.mainPlayerMng.MainPlayerInfo.TotalCoinCount;
            int consumeNum = GameCenter.inventoryMng.GetNumberByType(WingPromoteList[0].eid);
            if ( consumeNum >= WingPromoteNum[0] && coinNum >= WingPromoteNum[1])
                return true;
            return false;
        }
    }
    #endregion
    #endregion
}
