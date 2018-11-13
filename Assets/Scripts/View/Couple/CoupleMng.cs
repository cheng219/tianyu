//==================================
//作者：朱素云
//日期：2016/5/4
//用途：仙侣管理类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

public class CoupleMng
{ 
    /// <summary>
    /// 送花人姓名
    /// </summary>
    public string sendFowerName;
    /// <summary>
    /// 收花人姓名
    /// </summary>
    public string receiveFlowerName;
    /// <summary>
    /// 当前仙侣数据
    /// </summary>
    public CoupleData coupleData = null;
    /// <summary>
    /// 玩家婚否
    /// </summary>
    public bool isMarrige = false;
    //protected FDictionary newDic = new FDictionary();
    /// <summary>
    /// 玩家拥有的所有称号
    /// </summary> 
    //public FDictionary coupleOwnTitleDic
    //{
    //    get
    //    { 
    //        FDictionary dic = GameCenter.titleMng.TitleDictionary;
    //        foreach (TitleInfo info in dic.Values)
    //        {  
    //            //Debug.Log("   拥有的称号     " + info.Des   + "        id       "   + info.ID);
    //            if (!newDic.ContainsKey(info.ID))
    //                newDic[info.ID] = info; 
    //        }
    //        return newDic;
    //    }
    //}
    /// <summary>
    /// 所有的仙侣称号
    /// </summary>
    //public List<int> allCoupleTitle 
    //{
    //    get
    //    {
    //        return ConfigMng.Instance.GetTitleListByType(23);
    //    }
    //}
    protected TitleRef curtitleRef = null;
    /// <summary>
    /// 仙侣当前称号
    /// </summary>
    public TitleRef titleRef
    {
        get
        {
            return curtitleRef;
        }
        protected set
        { 
            curtitleRef = value;
            if (OnCoupleTitleUpdata != null)
                OnCoupleTitleUpdata(); 
        }
    }
    protected TitleRef nextTitle = null;
    /// <summary>
    /// 仙侣下级称号
    /// </summary>
    public TitleRef nextTitleRef
    {
        get
        {
            return nextTitle;
        }
        protected set
        {
            nextTitle = value;
            if (OnCoupleTitleUpdata != null)
                OnCoupleTitleUpdata();
        }
    }
    #region 事件
    /// <summary>
    /// 仙侣数据变化事件
    /// </summary>
    public System.Action OnCoupleDataUpdata;
    /// <summary>
    /// 仙侣称号变化
    /// </summary>
    public System.Action OnCoupleTitleUpdata;
    /// <summary>
    /// 婚礼数据变化
    /// </summary>
    public System.Action OnMerriageUpdata; 

    #endregion

    #region 构造
    public static CoupleMng CreateNew()
    {
        if (GameCenter.coupleMng == null)
        {
            CoupleMng coupleMng = new CoupleMng();
            coupleMng.Init();
            return coupleMng;
        }
        else
        {
            GameCenter.coupleMng.UnRegist();
            GameCenter.coupleMng.Init();
            return GameCenter.coupleMng;
        }
    }

    protected void Init()
    {
        MsgHander.Regist(0xD591, S2C_GetCoupleInfo);
        MsgHander.Regist(0xD534, S2C_GetTokenUpLev);
        MsgHander.Regist(0xD759, S2C_GetInitimacyUpdata);
        //MsgHander.Regist(0xD688, S2C_GetCouple);
        MsgHander.Regist(0xD531, S2C_GetCoupleBrokeUp);
        MsgHander.Regist(0xD797, S2C_ShowFlower);
    }

    protected void UnRegist()
    {
        MsgHander.UnRegist(0xD591, S2C_GetCoupleInfo);
        MsgHander.UnRegist(0xD534, S2C_GetTokenUpLev);
        MsgHander.UnRegist(0xD759, S2C_GetInitimacyUpdata);
        //MsgHander.UnRegist(0xD688, S2C_GetCouple);
        MsgHander.UnRegist(0xD531, S2C_GetCoupleBrokeUp);
        MsgHander.UnRegist(0xD797, S2C_ShowFlower);
        titleRef = null;
        nextTitleRef = null;
        coupleData = null;
        isMarrige = false;
    }
    /// <summary>
    /// 判断是否和某人结婚
    /// </summary> 
    public bool IsMarryWithSomeone(int _someoneId)
    {
        if (coupleData != null)
        {
            if (_someoneId == coupleData.objId)
                return true;
            return false;
        }
        else
            return false;
    }
    /// <summary>
    /// 是否与某某结婚的某某名字
    /// 双方之间的亲密度需达到：1314 点及以上；
    /// 双方之间不能结义；
    /// 双方必须都是没有仙侣；
    /// 双方等级≥30级； 
    /// </summary> 
    public string HerName()
    {
        //Debug.Log("队友数： " + GameCenter.teamMng.TeammatesDic.Count + "     " + GameCenter.teamMng.TeammateCount);
        if (!GameCenter.teamMng.isInTeam || GameCenter.teamMng.TeammateCount != 2)
        {
            //上浮提示两人队伍才可结为夫妻 
            GameCenter.messageMng.AddClientMsg(343);
        }
        else
        {
            if (GameCenter.teamMng.TeammateCount == 2)
            {
                foreach (TeamMenberInfo team in GameCenter.teamMng.TeammatesDic.Values)
                { 
                    if (GameCenter.mainPlayerMng.MainPlayerInfo != null)
                    { 
                        if (GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID != team.baseInfo.uid)
                            return team.baseInfo.name;  
                    } 
                }
            }
        }  
        return string.Empty;
    }
    /// <summary>
    /// 获取仙侣称号
    /// </summary> 
    public void GeTTitleRef()
    {
        List<int> allCoupleTitle = ConfigMng.Instance.GetTitleListByType(23);
        FDictionary coupleOwnTitleDic = GameCenter.titleMng.TitleDictionary;

        //bool isOwnCoupleTitle = false;
        for (int i = 0, max = allCoupleTitle.Count; i < max; i++)
        {
            if (coupleOwnTitleDic.ContainsKey(allCoupleTitle[i]))
            { 
                TitleInfo info = coupleOwnTitleDic[allCoupleTitle[i]] as TitleInfo;
                //Debug.Log(    " des     "      + info.Des + "       icon     " + info.IconName  + "   isoen : " + info.IsOwn);
                if (info.IsOwn)
                {
                    //isOwnCoupleTitle = true;
                    titleRef = ConfigMng.Instance.GetTitlesRef(allCoupleTitle[i]);
                    if (allCoupleTitle.Count > (i + 1)) nextTitleRef = ConfigMng.Instance.GetTitlesRef(allCoupleTitle[i + 1]); 
                    else
                        nextTitleRef = null;
                }
            }
        }
        if (titleRef == null)
            nextTitleRef = ConfigMng.Instance.GetTitlesRef(allCoupleTitle[0]); 
        
        //if (!isOwnCoupleTitle)
        //{
        //    titleRef = null;
        //    if (allCoupleTitle.Count > 0)
        //        nextTitleRef = ConfigMng.Instance.GetTitlesRef(allCoupleTitle[0]);
        //    else
        //        nextTitleRef = null;
        //} 
        //if (_title == CoupleTitle.CURTITLE)
        //    return titleRef;
        //else
        //    return nextTitleRef;
    }

    /// <summary>
    /// 根据信物id获取信物升级属性
    /// </summary> 
    public int GetTokenAttrValueByTag(ActorPropertyTag _tag)
    {
        if (coupleData != null && coupleData.tokenLev > 0)
        {
            for (int i = 0, max = coupleData.Attribute.Count; i < max; i++)
            {
                if (coupleData.Attribute[i].tag == _tag)
                { 
                    return coupleData.Attribute[i].value;
                }
            }
        }
        //Debug.LogError("信物表中找不到id为 ： " + _id + " 的数据");
        return 0;
    } 

    /// <summary>
    /// 根据信物id获取信物初始属性
    /// </summary> 
    public List<AttributePair> GetTokenAttrByItem(int _id)
    { 
        WeddingRef wedding = ConfigMng.Instance.GetWeddingRef(_id);
        if (wedding != null)
        {
            return wedding.attrs;
        }
        //Debug.LogError("信物表中找不到id为 ： " + _id + " 的数据");
        return null;
    } 
    #endregion

    #region S2C
    /// <summary>
    /// 全服炫耀999朵玫瑰
    /// </summary> 
    protected void S2C_ShowFlower(Pt pt)
    {
        pt_give_flower_allserver_inform_d797 msg = pt as pt_give_flower_allserver_inform_d797;
        if (msg != null)
        { 
            sendFowerName = msg.give_flower_name;
            receiveFlowerName = msg.receive_flower_name;
            GameCenter.uIMng.GenGUI(GUIType.SHOWFLOWER, true);
            //Debug.Log("sendFowerName :   " + sendFowerName + "   , receiveFlowerName : " + receiveFlowerName);
        }
    }
    /// <summary>
    /// 通知结婚成功
    /// </summary> 
    //protected void S2C_GetCouple(Pt pt)
    //{
    //    pt_marry_success_d688 msg = pt as pt_marry_success_d688;
    //    if (msg != null)
    //    {
    //        GameCenter.swornMng.isOpenSworn = OpenType.COUPLE;
    //    }
    //}
    /// <summary>
    /// 通知离婚成功
    /// </summary> 
    protected void S2C_GetCoupleBrokeUp(Pt pt)
    {
        pt_req_break_marry_d531 msg = pt as pt_req_break_marry_d531;
        if (msg != null)
        {
            //Debug.Log(" 通知离婚成功 ");
            coupleData = null;
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
        }
    }
    /// <summary>
    /// 获取仙侣信息
    /// </summary>
    protected void S2C_GetCoupleInfo(Pt pt)
    {
        isMarrige = false;
        pt_companion_d591 msg = pt as pt_companion_d591;
        if (msg != null)
        { 
            if (msg.create_time > 0)
            {
                isMarrige = true;
                if (coupleData == null)
                {
                    coupleData = new CoupleData(msg);
                }
                else
                {
                    coupleData.UpdateData(msg);
                }
            }
            else coupleData = null;
        }
        if (OnCoupleDataUpdata != null) OnCoupleDataUpdata();
    } 
    /// <summary>
    /// 提升信物
    /// </summary> 
    protected void S2C_GetTokenUpLev(Pt pt)
    {
        pt_keepsake_info_d534 msg = pt as pt_keepsake_info_d534;
        if (msg != null)
        {
            //Debug.Log("收到更新的等级为：" + msg.lev + "        收到更新的经验为：" + msg.exp);
            if (coupleData != null) coupleData.UpdateData(msg);
        }
        if (OnCoupleDataUpdata != null) OnCoupleDataUpdata();
    }   
    /// <summary>
    /// 更新亲密度
    /// </summary>
    /// <param name="pt"></param>
    protected void S2C_GetInitimacyUpdata(Pt pt)
    {
        pt_update_friend_intimacy_d759 msg = pt as pt_update_friend_intimacy_d759;
        if (msg != null)
        {
            //Debug.Log("收到亲密度 ： " + msg.intimacy + "  ,人物id : " + msg.oth_uid);
            if (coupleData != null) coupleData.UpdateData(msg);
        }
        if (OnCoupleDataUpdata != null) OnCoupleDataUpdata();
    }
    #endregion

    #region C2S
    /// <summary>
    /// 请求结成伴侣
    /// </summary> 
    public void C2S_ReqMarriage(int _tokenId)
    { 
        pt_req_marry_d536 msg = new pt_req_marry_d536();
        msg.keepsake_type = _tokenId;
        if (coupleData != null)
        {
            msg.marry_type = 2;//补办婚礼
        }
        else
        {
            msg.marry_type = 1;
        }
        //Debug.Log("结婚的信物id ： " + _tokenId + "  , 婚礼类型 ： " + msg.marry_type);
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求界面信息
    /// </summary>
    public void C2S_ReqMerrageInfo()
    {
        pt_req_companion_d530 msg = new pt_req_companion_d530();
        NetMsgMng.SendMsg(msg);
    } 
    /// <summary>
    /// 请求离婚
    /// </summary>
    public void C2S_ReqDivorce()
    { 
        pt_req_break_marry_d531 msg = new pt_req_break_marry_d531();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求进入副本
    /// </summary>
    public void C2S_ReqGoToCopy()
    {
        pt_req_enter_lovers_copy_d533 msg = new pt_req_enter_lovers_copy_d533();
        NetMsgMng.SendMsg(msg);
    } 
    /// <summary>
    /// 请求提升信物
    /// </summary>
    public void C2S_ReqUpToken()
    {
        pt_req_strengthen_keepsake_d535 msg = new pt_req_strengthen_keepsake_d535();
        NetMsgMng.SendMsg(msg);
    } 
    #endregion 
}

public enum CoupleTitle
{ 
    /// <summary>
    /// 当前称号
    /// </summary>
    CURTITLE,
    /// <summary>
    /// 下级称号
    /// </summary>
    NEXTTITLE,
}
