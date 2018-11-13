//===================
//作者：鲁家旗
//日期：2016/4/15
//用途：藏宝库管理类
//===================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
public class TreasureHouseMng
{
    #region 数据

    /// <summary>
    /// 是否可以免费抽奖
    /// </summary>
    private bool isCanFree = false;
    public bool IsCanFree
    {
        get
        {
            return isCanFree;
        }
    }
    /// <summary>
    /// 十连抽是否可以半价抽取
    /// </summary>
    private bool isCanHalfPrice = false;
    public bool IsCanHalfPrice
    {
        get
        {
            return isCanHalfPrice;
        }
    }
    /// <summary>
    /// 仓库数据不包含空格
    /// </summary>
    public Dictionary<int, EquipmentInfo> StorageDictionary = new Dictionary<int, EquipmentInfo>();
    /// <summary>
    /// 仓库物品数据集,包含空格
    /// </summary>
    protected Dictionary<int, EquipmentInfo> realStorageDictionary = new Dictionary<int, EquipmentInfo>();
    /// <summary>
    /// 仓库物品数据集,包含空格
    /// </summary>
    public Dictionary<int, EquipmentInfo> RealStorageDictionary
    {
        get { return realStorageDictionary; }
    }
    /// <summary>
    /// 抽取到奖励的列表
    /// </summary>
    public List<int> idList = new List<int>();
    /// <summary>
    /// 抽取奖励后抛出事件
    /// </summary>
    public System.Action OnTreasureUpdate;
    /// <summary>
    /// 整理仓库抛出事件
    /// </summary>
    public System.Action OnHouseUpdate;
    /// <summary>
    /// 取出仓库物品后抛出事件
    /// </summary>
    public System.Action OnStorageChangeUpdate;
    /// <summary>
    /// 抽奖记录返回后抛出事件
    /// </summary>
    public System.Action OnRecordUpdate;
    /// <summary>
    /// 仓库数据发生变化(位置,物品信息)
    /// </summary>
    public System.Action<int, EquipmentInfo> OnStorageItemUpdate;

    /// <summary>
    /// 抽奖记录玩家ID
    /// </summary>
    public List<int> playerId = new List<int>();
    /// <summary>
    /// 抽奖记录玩家名字
    /// </summary>
    public List<string> playerName = new List<string>();
    /// <summary>
    /// 抽到记录的道具
    /// </summary>
    public List<EquipmentInfo> goodsList = new List<EquipmentInfo>();
    /// <summary>
    /// 是否显示红点
    /// </summary>
    public bool isRed = false;
    /// <summary>
    /// 一键取出
    /// </summary>
    //public System.Action OnClickGetAll;
    //protected bool isGetAll = false;
    /// <summary>
    /// 抽奖奖励金币数量
    /// </summary>
    public int coinNum = 0;
    #endregion

    #region 构造
    /// <summary>
    /// 返回一个全新的藏宝阁管理类对象实例
    /// </summary>
    public static TreasureHouseMng CreateNew()
    {
        if (GameCenter.treasureHouseMng == null)
        {
            TreasureHouseMng treasureHouseMng = new TreasureHouseMng();
            treasureHouseMng.Init();
            return treasureHouseMng;
        }
        else
        {
            GameCenter.treasureHouseMng.UnRegist();
            GameCenter.treasureHouseMng.Init();
            return GameCenter.treasureHouseMng;
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    void Init()
    {
        MsgHander.Regist(0xD378, S2C_GetTreasureData);
        MsgHander.Regist(0xD390, S2C_GetHouseData);
        MsgHander.Regist(0xD391, S2C_GetChangeHouseData);
        MsgHander.Regist(0xD511, S2C_GetTreasureRecord);
    }
    /// <summary>
    /// 注销
    /// </summary>
    void UnRegist()
    {
        MsgHander.UnRegist(0xD378, S2C_GetTreasureData);
        MsgHander.UnRegist(0xD390, S2C_GetHouseData);
        MsgHander.UnRegist(0xD391, S2C_GetChangeHouseData);
        MsgHander.UnRegist(0xD511, S2C_GetTreasureRecord);
        realStorageDictionary.Clear();
        playerId.Clear();
        playerName.Clear();
        goodsList.Clear();
        coinNum = 0;
    }
    #endregion

    #region 协议
    #region S2C 服务端发往客户端的协议处理
    /// <summary>
    /// 抽奖后返回协议
    /// </summary>
    /// <param name="_msg"></param>
    protected void S2C_GetTreasureData(Pt _msg)
    {
        //Debug.Log("接收pt_treasure_d378协议 ！");
        pt_treasure_d378 msg = _msg as pt_treasure_d378;
        if (msg != null)
        {
            idList.Clear();
            //idList.Add(5);
            for (int i = 0; i < msg.reward.Count; i++)
            {
                treasure_list data = msg.reward[i];
                idList.Add((int)data.type);
            }
            if (msg.reward.Count == 1)
                coinNum = 5000;
            else if (msg.reward.Count == 10)
                coinNum = 50000;
            else
                coinNum = 250000;
            
            isRed = true;
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.HIDDENTREASURE, true);
        }
        if (OnTreasureUpdate != null)
        {
            OnTreasureUpdate();
        }
    }
    /// <summary>
    /// 服务端仓库数据返回 
    /// </summary>
    /// <param name="_info"></param>
    protected void S2C_GetHouseData(Pt _info)
    {
        //Debug.Log("接收 pt_treasure_house_info_d390 协议 ！");
        pt_treasure_house_info_d390 pt_treasure_house_info_d390 = _info as pt_treasure_house_info_d390;
        if (pt_treasure_house_info_d390 != null)
        {
            int count = pt_treasure_house_info_d390.item_list.Count;
            realStorageDictionary.Clear();
            StorageDictionary.Clear();
            for (int i = 0; i < 200; i++)
            {
                if (i < count)//仓库中非空格的数据
                {
                    EquipmentInfo info = new EquipmentInfo(pt_treasure_house_info_d390.item_list[i],EquipmentBelongTo.WAREHOUSE);
                    //Debug.Log("info.name  " + info.ItemName + " info.Postion  " + info.Postion + " i " + i);
                    realStorageDictionary[info.Postion] = info;
                    StorageDictionary[info.Postion] = info;
                }
            }
            //Debug.Log("realStorageDictionary.Count  " + realStorageDictionary.Count + "  StorageDictionary.Count  " + StorageDictionary.Count);
            //空格子
            int len = pt_treasure_house_info_d390.empty_bags.Count;
            for (int i = 0; i < len; i++)
            {
                realStorageDictionary[(int)pt_treasure_house_info_d390.empty_bags[i]] = new EquipmentInfo((int)(pt_treasure_house_info_d390.empty_bags[i]), EquipmentInfo.EmptyType.EMPTY, EquipmentBelongTo.WAREHOUSE);//空格物品
            }
            isRed = StorageDictionary.Count > 0;
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.HIDDENTREASURE, StorageDictionary.Count > 0 || IsCanFree);
        }
        if (OnHouseUpdate != null)
            OnHouseUpdate();
    }
    protected void S2C_GetChangeHouseData(Pt _info)
    {
        //Debug.Log("接收 pt_treasure_item_info_d391 协议 ！");
        pt_treasure_item_info_d391 pt_treasure_item_info_d391 = _info as pt_treasure_item_info_d391;
        if (pt_treasure_item_info_d391 != null)
        {
            int count = pt_treasure_item_info_d391.items.Count;
            if (count >= 0)
            {
                for (int i = 0; i < count; i++)
                {
                    EquipmentInfo info = new EquipmentInfo(pt_treasure_item_info_d391.items[i]);
                    //Debug.Log("info.name  " + info.ItemName + " info.Postion  " + info.Postion + " i " + i);
                    if (StorageDictionary.ContainsKey(info.Postion))
                    {
                        StorageDictionary.Remove(info.Postion);
                    }
                    if (info.StackCurCount == 0)
                        info = new EquipmentInfo(info.Postion, EquipmentInfo.EmptyType.EMPTY, EquipmentBelongTo.WAREHOUSE);
                    realStorageDictionary[info.Postion] = info;
                }
                //Debug.Log("StorageDictionary.Count  " + StorageDictionary.Count);
                GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.HIDDENTREASURE, StorageDictionary.Count > 0 || IsCanFree);
                isRed = StorageDictionary.Count > 0;
            }
            if (OnHouseUpdate != null)
                OnHouseUpdate();
            if (OnStorageChangeUpdate != null)
                OnStorageChangeUpdate();
            //if (OnClickGetAll != null && isGetAll)
            //{
            //    //Debug.Log("一键取出的时候进来");
            //    OnClickGetAll();
            //    isGetAll = false;
            //}
        }
    }
    /// <summary>
    /// 返回抽奖记录协议
    /// </summary>
    /// <param name="_msg"></param>
    protected void S2C_GetTreasureRecord(Pt _msg)
    { 
        pt_treasure_record_d511 msg = _msg as pt_treasure_record_d511;
        if (msg != null)
        {
            playerName.Clear();
            goodsList.Clear();
            playerId.Clear();
            isCanFree = msg.free_flag == 0;
            isCanHalfPrice = msg.half_price_flag == 0;
            for (int i = 0; i < msg.treasure_info.Count; i++)
            {
                treasure_record_list data = msg.treasure_info[i];
                EquipmentInfo info = new EquipmentInfo(data.type, EquipmentBelongTo.PREVIEW);
                goodsList.Add(info);
                playerName.Add(data.name);
                playerId.Add(data.uid);
            }
            //Debug.Log("接收pt_treasure_record_511 协议 ！" + IsCanFree);
            GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.HIDDENTREASURE, StorageDictionary.Count > 0 || IsCanFree);
        }
        if (OnRecordUpdate != null)
        {
            OnRecordUpdate();
        }
    }
    #endregion

    #region C2S 客户端发往服务端的协议处理
    /// <summary>
    /// 请求抽取物品 1 抽取一次 2 抽取十次 3 抽取五十次
    /// </summary>
    /// <param name="_type"></param>
    public void C2S_ReqGetTreasure(GetTreasureType _type)
    {
        pt_req_treasure_d377 msg = new pt_req_treasure_d377();
        msg.type = (int)_type;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求临时仓库列表信息
    /// </summary>
    public void C2S_ReqGetHouse()
    {
        pt_req_treasure_house_info_d389 msg = new pt_req_treasure_house_info_d389();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求整理仓库
    /// </summary>
    public void C2S_ReqArrageHouse()
    {
        pt_req_clear_treasure_house_d387 msg = new pt_req_clear_treasure_house_d387();
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求一键取出仓库
    /// </summary>
    public void C2S_ReqTakeOutHouse(List<int> _idList,bool _getAll)
    {
        //if (_getAll) isGetAll = true;
        pt_req_move_trearsure_item_d388 msg = new pt_req_move_trearsure_item_d388();
        msg.id_list = _idList;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求获取抽奖记录
    /// </summary>
    public void C2S_ReqTreasureRecord()
    {
        pt_req_treasure_record_d512 msg = new pt_req_treasure_record_d512();
        NetMsgMng.SendMsg(msg);
    } 
    #endregion
    #endregion
}
/// <summary>
/// 抽奖类型
/// </summary>
public enum GetTreasureType
{ 
    /// <summary>
    /// 抽取一次
    /// </summary>
    GETTREASUREONE = 1,
    /// <summary>
    /// 抽取十次
    /// </summary>
    GETTREASURETEN = 2,
    /// <summary>
    /// 抽取五十次
    /// </summary>
    GETTREASUREFIFTY = 3,
}
