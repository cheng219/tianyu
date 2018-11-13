//==============================================
//作者：黄洪兴
//日期：2016/6/4
//用途：小助手管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class LittleHelperMng
{


    public Dictionary<int, List<LittleHelperRef>> LittleHelperDic = new Dictionary<int, List<LittleHelperRef>>();
    public Dictionary<int, LittleHelperTypeRef> LittleHelperTypeDic = new Dictionary<int, LittleHelperTypeRef>();
    public LittleHelpType NeedType;
    


    #region 构造
    /// <summary>
    /// 返回该管理类的唯一实例
    /// </summary>
    /// <returns></returns>
    public static LittleHelperMng CreateNew(MainPlayerMng _main)
    {
        if (_main.littleHelperMng == null)
        {
            LittleHelperMng LittleHelperMng = new LittleHelperMng();
            LittleHelperMng.Init(_main);
            return LittleHelperMng;
        }
        else
        {
            _main.littleHelperMng.UnRegist(_main);
            _main.littleHelperMng.Init(_main);
            return _main.littleHelperMng;
        }
    }
    /// <summary>
    /// 注册
    /// </summary>
    protected virtual void Init(MainPlayerMng _main)
    {
        LittleHelperDic = ConfigMng.Instance.GetLittleHelperDic();
        LittleHelperTypeDic = ConfigMng.Instance.GetLittleHelperTypeDic();
        NeedType = LittleHelpType.STRONG;
        //Debug.Log("长度分别为" + LittleHelperDic.Count + ":" + LittleHelperTypeDic.Count);
        //		MsgHander.Regist(0xD100, S2C_OnGetSkillList);
        //		MsgHander.Regist(0xD401, S2C_OnGetUseSkillList);
        //GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += ChangeAutoUseSkill;
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected virtual void UnRegist(MainPlayerMng _main)
    {
        LittleHelperDic.Clear();
        LittleHelperTypeDic.Clear();
        //		MsgHander.UnRegist(0xD100, S2C_OnGetSkillList);
        //		MsgHander.UnRegist(0xD401, S2C_OnGetUseSkillList);
        //GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= ChangeAutoUseSkill;
    }
    #endregion

    #region 通信S2C


    #endregion

    #region C2S
 
    #endregion

    #region 辅助逻辑

    public void OpenWndByType(LittleHelpType _type)
    {
        NeedType = _type;
        GameCenter.uIMng.SwitchToSubUI(SubGUIType.LITTLEHELPER);
    }

  

    #endregion
}

public enum LittleHelpType
{
    /// <summary>
    /// 变强
    /// </summary>
    STRONG,
    /// <summary>
    /// 要经验
    /// </summary>
    EXP,
    /// <summary>
    /// 要钱
    /// </summary>
    COIN,
    /// <summary>
    /// 要宠物
    /// </summary>
    PET,
    /// <summary>
    /// 要活动
    /// </summary>
    ACTIVITY,


}