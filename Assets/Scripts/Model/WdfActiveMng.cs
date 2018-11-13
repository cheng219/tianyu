//==============================================
//作者：黄洪兴
//日期：2016/7/9
//用途：运营活动管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class WdfActiveMng
{

    //protected List<WdfActiveData> wdfActiveTypeDic = new List<WdfActiveData>();
    public List<WdfActiveData> WdfActiveTypeDic
    {
        get
        {
            List<WdfActiveData> wdfActiveTypeDic = new List<WdfActiveData>();
            if (ActiveTypeInfoDic.ContainsKey(CurWdfActiveMainType))
            {
                using (var e = ActiveTypeInfoDic[CurWdfActiveMainType].GetEnumerator())
                {
                    while (e.MoveNext())
                    { 
                        wdfActiveTypeDic.Add(e.Current.Value);
                    }
                } 
            }
            wdfActiveTypeDic.Sort(SortCompare);
            return wdfActiveTypeDic;
        }
    }
    /// <summary>
    /// 根据活动类型获得活动数据
    /// </summary>
    public Dictionary<WdfType, Dictionary<int, WdfActiveData>> ActiveTypeInfoDic = new Dictionary<WdfType, Dictionary<int, WdfActiveData>>();

    /// <summary>
    /// 红点
    /// </summary>
    public Dictionary<int, bool> RedDic = new Dictionary<int, bool>();
   


    /// <summary>
    /// 当前需要查看的大活动类型
    /// </summary>
    public WdfType CurWdfActiveMainType = WdfType.NONE;


    
    /// <summary>
    /// 当前选择的小活动类型
    /// </summary>
    public int CurWdfActiveType = 0;

    /// <summary>
    /// 当前小活动的奖励数据
    /// </summary>
    public WdfActiveTypeData CurWdfActiveItemInfo = null;
    /// <summary>
    /// 获得活动详细数据
    /// </summary>
    public Action OnGetAllActiveInfo;
    /// <summary>
    /// 获得所有Type活动数据
    /// </summary>
    public Action OnGetAllActiveTypes;

    public Action RefreshShow;

    public Action RefreshRed;





    /// <summary>
    /// 节日活动开启状态
    /// </summary>
    public bool isHolidayOpen=false;
    /// <summary>
    /// 精彩活动开启状态
    /// </summary>
    public bool isWdfOpen = false;
    /// <summary>
    /// 开服活动开启状态
    /// </summary>
    public bool isOpenOpen = false;
    /// <summary>
    /// 开服贺礼开启状态
    /// </summary>
    public bool isGiftOpen = false;
    /// <summary>
    /// 和服活动开启状态
    /// </summary>
    public bool isCombineOpen = false;

    public bool needReset = true;

    public bool isAccord = false;
    #region 构造
    /// <summary>
    /// 返回该管理类的唯一实例
    /// </summary>
    /// <returns></returns>
    public static WdfActiveMng CreateNew()
    {
        if (GameCenter.wdfActiveMng == null)
        {
            GameCenter.wdfActiveMng = new WdfActiveMng();
            GameCenter.wdfActiveMng.Init();
            return GameCenter.wdfActiveMng;
        }
        else
        {
            GameCenter.wdfActiveMng.UnRegist();
            GameCenter.wdfActiveMng.Init();
            return GameCenter.wdfActiveMng;
        }
    }
    /// <summary>
    /// 注册
    /// </summary>
    protected  void Init()
    {
     
        //GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += ChangeAutoUseSkill;
        MsgHander.Regist(0xD901, S2C_GetAllActivitys);
        MsgHander.Regist(0xD903, S2C_GetActivitysByID);
        MsgHander.Regist(0xD905, S2C_GetActivitysRewardPt); 
        C2S_AskAllActivitysInfo();
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected  void UnRegist()
    {

        //GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= ChangeAutoUseSkill;
        MsgHander.UnRegist(0xD901, S2C_GetAllActivitys);
        MsgHander.UnRegist(0xD903, S2C_GetActivitysByID);
        MsgHander.UnRegist(0xD905, S2C_GetActivitysRewardPt); 
        ActiveTypeInfoDic.Clear(); 
        RedDic.Clear();
        isHolidayOpen = false;
        isWdfOpen = false;
        isOpenOpen = false;
        isGiftOpen = false;
        isCombineOpen = false;
        needReset = true;
        isAccord = false;
        CurWdfActiveType = 0;
        CurWdfActiveMainType = WdfType.NONE;
        CurWdfActiveItemInfo = null; 
    }
    #endregion


   
    #region 通信S2C



    /// <summary>
    /// 获得所有活动类型
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetAllActivitys(Pt _pt)
    { 
        pt_reply_all_operation_activity_d901 pt = _pt as pt_reply_all_operation_activity_d901;
        if (pt != null)
        {
            //Debug.Log("S2C_GetAllActivitys ： " + pt.title_info_list.Count);
            //Debug.Log("GameCenter.openServerRewardMng.CanOpen" + GameCenter.openServerRewardMng.CanOpen);
            //Debug.Log("isGiftOpen"+ isGiftOpen);
            ActiveTypeInfoDic.Clear(); 
            RedDic.Clear();
            for (int i = 0; i < pt.title_info_list.Count; i++)
            {
                WdfActiveData data = new WdfActiveData(pt.title_info_list[i]);

                if (data != null)
                { 
                    if(CurWdfActiveMainType == data.activateType)RedDic[data.type] = data.redState;
                    if (data.activateType == WdfType.OPEN) isOpenOpen = true;
                    if (data.activateType == WdfType.WONDERFUL) isWdfOpen = true;
                    if (data.activateType == WdfType.COMBINE) isCombineOpen = true;
                    if (data.activateType == WdfType.HOLIDAY) isHolidayOpen = true;
                    if (!ActiveTypeInfoDic.ContainsKey(data.activateType))
                    {
                        ActiveTypeInfoDic[data.activateType] = new Dictionary<int, WdfActiveData>();
                    }
                    if (!ActiveTypeInfoDic[data.activateType].ContainsKey(data.type))
                    {
                        ActiveTypeInfoDic[data.activateType][data.type] = data;
                    }
                }
                //Debug.Log("大活动类型 ： " + data.activateType + " , 第" + i + "组活动数据ID : " + data.id + " , 名字 : " + data.name + " , 类型 : " + data.type);
            }
            //wdfActiveTypeDic.Sort(SortCompare);
            if (OnGetAllActiveTypes != null)
                OnGetAllActiveTypes();
            if (pt.title_info_list.Count > 0)
            { 
                if (RefreshShow != null)
                    RefreshShow();
            }
            else
            {
               if (isAccord)
                {
                    isOpenOpen = false;
                    if (RefreshShow != null)
                        RefreshShow();
                    if (GameCenter.uIMng.CurOpenType == GUIType.WDFACTIVE)
                    {
                        GameCenter.uIMng.SwitchToUI(GUIType.NONE);
                        GameCenter.messageMng.AddClientMsg(427);
                    } 
                }
            }
            ResetRed();
        } 
    }

    /// <summary>
    /// 获得所有活动类型
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetActivitysByID(Pt _pt)
    {
        //Debug.Log("S2C_GetActivitysByID");
        pt_reply_operation_activity_info_d903 pt = _pt as pt_reply_operation_activity_info_d903;
        if (pt != null)
        {
            //CurWdfActiveType = (int)pt.id;
            CurWdfActiveItemInfo =new WdfActiveTypeData(pt);
            //Debug.Log("ID为"+pt.id+"种类为"+pt.type+"长度为"+pt.details.Count+"充值天数"+pt.counter_value);

            if (OnGetAllActiveInfo != null)
                OnGetAllActiveInfo();
        } 
    }


    /// <summary>
    /// 获得领取奖励结果
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_GetActivitysRewardPt(Pt _pt)
    {
       // Debug.Log("S2C_GetActivitysRewardPt");
       pt_reply_operation_activity_reward_d905 pt = _pt as pt_reply_operation_activity_reward_d905;
        if (pt != null)
        {
            //Debug.Log("收到领取奖励结果协议ID"+pt.act_id+"index"+pt.index+"结果"+pt.result);
            C2S_AskActivitysInfoByID((int)pt.act_id);
            C2S_AskAllActivitysInfo();
        } 
    }









    #endregion

    #region C2S
    /// <summary>
    /// 请求所有活动数据
    /// </summary>
    /// <param name="num"></param>
    public void C2S_AskAllActivitysInfo()
    {
        pt_req_all_operation_activity_d900 msg = new pt_req_all_operation_activity_d900();
        NetMsgMng.SendMsg(msg);
        //Debug.Log("所有活动数据");
    }

    /// <summary>
    /// 请求对应ID的活动数据
    /// </summary>
    /// <param name="num"></param>
    public void C2S_AskActivitysInfoByID(int _id)
    {
        pt_req_operation_activity_info_d902 msg = new pt_req_operation_activity_info_d902();
        msg.id = (uint)_id;
        NetMsgMng.SendMsg(msg);
        //Debug.Log("请求ID为"+_id+"的活动数据");
    }

    /// <summary>
    /// 领取对应ID的奖励
    /// </summary>
    /// <param name="num"></param>
    public void C2S_AskActivitysRewards(int _id,int _index)
    {
        pt_req_operation_activity_reward_d904 msg = new pt_req_operation_activity_reward_d904();
        msg.act_id= (uint)_id;
        msg.index = (byte)_index;
        NetMsgMng.SendMsg(msg);
        //Debug.Log("请求ID为" + _id +"编号为"+_index+ "的活动数据");
    }


    



    #endregion

    #region 辅助逻辑
    private  int SortCompare(WdfActiveData AF1, WdfActiveData AF2)
   {
   int res = 0;
   if (AF1.type > AF2.type)
   {
   res = 1;
   }
   else if (AF1.type < AF2.type)
   {
   res = -1;
   }
   return res;
   }


    public  void ResetRed()
    { 
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.OPENACTIVE, IsRedRemind(WdfType.OPEN));
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.WONDERFUL, IsRedRemind(WdfType.WONDERFUL));
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.FESTIVAL, IsRedRemind(WdfType.HOLIDAY));
        GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.COMBINESERVER, IsRedRemind(WdfType.COMBINE));
    }
     
    protected bool IsRedRemind(WdfType _type)
    {
        if (ActiveTypeInfoDic.ContainsKey(_type))
        {
            using (var e = ActiveTypeInfoDic[_type].GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (e.Current.Value.redState)
                    { 
                        return true; 
                    }
                }
            }
        }
        return false;
    } 
    #endregion
}


/// <summary>
/// 运营活动数据
/// </summary>
public class WdfActiveData
{

    public int id;
    public int type;
    public string name;
    public  bool redState = false;
    public WdfType activateType = WdfType.NONE;




    public WdfActiveData(operation_activity_title_info _info)
    {
        this.id =(int)_info.id;
        this.type =(int) _info.type;
        this.name = _info.name;
        this.redState = _info.reward_flag != 0;
        if (id >= 10000 && id < 20000)
        {
            activateType = WdfType.OPEN;
        }
        if (id >= 20000 && id < 30000)
        {
            activateType = WdfType.WONDERFUL;
        }
        if (id >= 30000 && id < 40000)
        {
            activateType = WdfType.COMBINE;
        }
        if (id >= 40000 && id < 50000)
        {
            activateType = WdfType.HOLIDAY;
        } 
    }
    public WdfActiveData()
    {

    }

    //public int CompareTo(object obj)
    //{
    //    int result;
    //    try
    //    {
    //        WdfActiveData info = obj as WdfActiveData;
    //        if (this.type > info.type)
    //        {
    //            result = 0;
    //        }
    //        else
    //            result = 1;
    //        return result;
    //    }
    //    catch (Exception ex) { throw new Exception(ex.Message); }
    //}
}


/// <summary>
/// 单个运营活动数据
/// </summary>
public class WdfActiveTypeData
{
   public  int id;
   public  int type;
   public  int rest_time;
   public  string desc;
   public  int counter_value;

   public  List<WdfActiveDetailsData> details = new List<WdfActiveDetailsData>();




    public WdfActiveTypeData(pt_reply_operation_activity_info_d903 _info)
    {

        this.id = (int)_info.id;
        this.type = _info.type;
        this.rest_time = (int)_info.rest_time;
        this.desc = _info.desc;
        this.counter_value = (int)_info.counter_value;
        for (int i = 0; i < _info.details.Count; i++)
        { 
            WdfActiveDetailsData data = new WdfActiveDetailsData(_info.details[i]); 
            if (data != null)
                this.details.Add(data);
        }
    }

    /// <summary>
    /// 构造登陆红利数据
    /// </summary> 
    public WdfActiveTypeData(pt_reply_login_dividend_info_d981 _info)
    {
        this.details.Clear();
        this.id = 66;
        this.type = 66; 
        FDictionary dic = ConfigMng.Instance.GetDividendRefTable();

        foreach (DividendRef info in dic.Values)//已经领取的天数
        {
            WdfActiveDetailsData data = new WdfActiveDetailsData(info, (int)_info.active_days, _info.reward_ids);
            if (data != null)
                this.details.Add(data);
        } 
    }
    public void Update(pt_reply_login_dividend_info_d981 _info)
    {
        this.details.Clear();
        this.id = 66;
        this.type = 66; 
        FDictionary dic = ConfigMng.Instance.GetDividendRefTable();

        foreach (DividendRef info in dic.Values)//已经领取的天数
        {
            WdfActiveDetailsData data = new WdfActiveDetailsData(info, (int)_info.active_days, _info.reward_ids);
            if (data != null)
                this.details.Add(data);
        }
    }

    public WdfActiveTypeData()
    {

    }

}





/// <summary>
/// 单个运营活动的详细数据
/// </summary>
public class WdfActiveDetailsData
{

    public string lastName = null;//上届得主
    public string curName = "???";//当前玩家

    public int index;
    public string desc;
    public int value1;
    public int value2;
    public int reward_times;
    public int total_reward_times;
    public List<EquipmentInfo> reward_info = new List<EquipmentInfo>();

    public WdfActiveDetailsData(operation_activity_detail_info _info)
    {
        this.index = _info.index;
        this.desc = _info.desc;
        this.value1 = (int)_info.value1;
        this.value2 = (int)_info.value2;
        this.reward_times = (int)_info.reward_times;
        this.total_reward_times = (int)_info.total_reward_times;
        this.lastName = _info.svalue1;
        this.curName = _info.svalue2;
        for (int i = 0; i < _info.reward_info.Count; i++)
        {

            EquipmentInfo item = new EquipmentInfo((int)_info.reward_info[i].item_id, (int)_info.reward_info[i].item_num,EquipmentBelongTo.PREVIEW);
            if (item != null)
                this.reward_info.Add(item);
        } 
    }

    public WdfActiveDetailsData(DividendRef _info, int _totleDays, List<uint> _list)
    {
        this.index = _info.ID;
        this.desc = _info.txt;
        this.reward_times = _info.need_days;
        this.total_reward_times = _totleDays;
        for (int i = 0, max = _list.Count; i < max; i++)
        {
            if (_info.ID == _list[i])//已经领取
            {
                this.reward_times = 0;
            }
        }
        for (int i = 0, max = _info.items.Count; i < max; i++)
        { 
            EquipmentInfo item = new EquipmentInfo(_info.items[i].eid, _info.items[i].count, EquipmentBelongTo.PREVIEW);
            if (item != null)
                this.reward_info.Add(item);
        } 
    }

    public WdfActiveDetailsData()
    {

    }

}

 
/// <summary>
/// 运营活动类型
/// </summary>
public enum WdfType
{
    NONE,
    /// <summary>
    /// 开服活动10000~19999
    /// </summary>
    OPEN,
    /// <summary>
    /// 精彩活动20000~29999
    /// </summary>
    WONDERFUL,
    /// <summary>
    /// 和服活动30000~39999
    /// </summary>
    COMBINE,
    /// <summary>
    /// 节日活动40000~49999
    /// </summary>
    HOLIDAY,
}
