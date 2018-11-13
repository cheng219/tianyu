//======================================================
//作者:朱素云
//日期:2016/5/4
//用途:仙侣数据类
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class CoupleData
{
    #region 数据
    /// <summary>
    /// 对象id
    /// </summary>
    public int objId;
    /// <summary>
    /// 对象等级
    /// </summary>
    public int objLev;
    /// <summary>
    /// 玩家是否在线
    /// </summary>
    public int onlineState;
    /// <summary>
    /// 结婚时间
    /// </summary>
    public int marrageTime;
    /// <summary>
    /// 对象名
    /// </summary>
    public string objName;
    /// <summary>
    /// 职业
    /// </summary>
    public int career;
    /// <summary>
    /// 亲密度
    /// </summary>
    public int intimacy;
    /// <summary>
    /// 称号id
    /// </summary>
    public int titleId;
    /// <summary>
    /// 信物id
    /// </summary>
    public int tokenId;
    /// <summary>
    /// 信物等级
    /// </summary>
    public int tokenLev;
    /// <summary>
    /// 信物经验
    /// </summary>
    public int tokenExp;
    /// <summary>
    /// 可进入副本的次数 
    /// </summary>
    public int time;
    /// <summary>
    /// 是否补办过婚礼 0 没有补办 1 补办过
    /// </summary>
    public bool isHoldMerige; 
    #endregion 
    
    #region 构造
    /// <summary>
    /// 以pt_companion_d591构造好友数据
    /// </summary> 
    public CoupleData(pt_companion_d591 _data)
    {
        this.objId = _data.target_id;
        this.marrageTime = _data.create_time;
        this.objName = _data.target_name;
        this.career = _data.target_prof;
        this.intimacy = _data.intimacy;
        this.titleId = _data.designation;
        this.tokenId = _data.keepsake_type;//服务器唯一id
        this.tokenLev = _data.keepsake_lev;
        this.tokenExp = _data.keepsake_exp;
        this.time = _data.res_copy_num > 0 ? _data.res_copy_num:0;
        this.isHoldMerige = _data.marry == 1 ? true : false;
        this.objLev = _data.target_lev;
        this.onlineState = _data.target_online_state;
    }
    /// <summary>
    /// 更新好友数据
    /// </summary> 
    public void UpdateData(pt_companion_d591 _data)
    {
        this.objId = _data.target_id;
        this.marrageTime = _data.create_time;
        this.objName = _data.target_name;
        this.career = _data.target_prof;
        this.intimacy = _data.intimacy;
        this.titleId = _data.designation;
        this.tokenId = _data.keepsake_type;
        this.tokenLev = _data.keepsake_lev;
        this.tokenExp = _data.keepsake_exp;
        this.time = _data.res_copy_num > 0 ? _data.res_copy_num : 0;
        this.isHoldMerige = _data.marry == 1 ? true : false;
        this.objLev = _data.target_lev;
        this.onlineState = _data.target_online_state;
    }
    /// <summary>
    /// 更新信物等级和经验(10级封顶)
    /// </summary> 
    public void UpdateData(pt_keepsake_info_d534 _data)
    {
        this.tokenLev = _data.lev;
        this.tokenExp = _data.exp; 
    }
    /// <summary>
    /// 更新亲密度
    /// </summary> 
    public void UpdateData(pt_update_friend_intimacy_d759 _data)
    {
        this.intimacy = _data.intimacy; 
    } 
    #endregion

    #region 访问器
    protected TokenLevRef tokenData = null;
    protected TokenLevRef TokenData
    {
        get
        {
            if (tokenData == null)
                tokenData = ConfigMng.Instance.GetTokenLevRef(tokenLev > 0 ? tokenLev : 1);
            return tokenData;
        }
    }
    protected TokenLevRef nextTokenData = null;
    protected TokenLevRef NextTokenData
    {
        get
        { 
            nextTokenData = ConfigMng.Instance.GetTokenLevRef(tokenLev + 1);
            return nextTokenData;
        }
    }     
    /// <summary>
    /// 对象图像
    /// </summary>
    public string ObjIcon
    {
        get
        {
            PlayerConfig ply = ConfigMng.Instance.GetPlayerConfig(career); 
            return ply!= null?ply.res_head_Icon:string.Empty;
        }
    }
    /// <summary>
    /// 升级信物需要的经验
    /// </summary>
    public int Exp
    {
        get
        {
            return TokenData == null ? 100 : TokenData.exp;
        }
    }
    /// <summary>
    /// 升级信物消耗的物品
    /// </summary>
    public List<ItemValue> Items
    {
        get
        {
            return TokenData != null ? TokenData.consume : new List<ItemValue>();
        }
    }
    /// <summary>
    /// 升级信物增加的属性
    /// </summary>
    public List<AttributePair> Attribute
    {
        get
        {
            return TokenData != null ? TokenData.attrs : new List<AttributePair>();
        }
    }
    /// <summary>
    /// 升到下级需要的物品
    /// </summary>
    public List<ItemValue> NextItems
    {
        get
        {
            return NextTokenData != null ? NextTokenData.consume : new List<ItemValue>();
        }
    }
    #endregion
}
