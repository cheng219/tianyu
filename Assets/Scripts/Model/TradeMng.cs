//==============================================
//作者：黄洪兴
//日期：2016/6/21
//用途：交易管理类
//=================================================



using UnityEngine;
using System.Collections;
using st.net.NetBase;
using System;
using System.Collections.Generic;

public class TradeMng
{
    private int tradeTargetUid;
    /// <summary>
    /// 交易对象的角色ID
    /// </summary>
    public int TradeTargetUid
    {
        get
        {
            return tradeTargetUid;
        }
    }
    /// <summary>
    /// 交易对象名字
    /// </summary>
    public string TradeTargetName;
    /// <summary>
    /// 交易对象锁定的交易物品
    /// </summary>
    public List<EquipmentInfo> TradeTargetItems = new List<EquipmentInfo>();
    /// <summary>
    /// 当前主玩家锁定的交易物品
    /// </summary>
    public List<EquipmentInfo> TradeMyItems = new List<EquipmentInfo>();

    /// <summary>
    /// 交易对象是否锁定交易物品
    /// </summary>
    public bool TradeTargetLockState=false;
    /// <summary>
    /// 当前主玩家是否锁定交易物品
    /// </summary>
    public bool TradeMyLockState = false;
    /// <summary>
    /// 当前选择交易的物品信息
    /// </summary>
    public EquipmentInfo CurTradeItemEQ;
    /// <summary>
    /// 交易时要取出的物品
    /// </summary>
    public EquipmentInfo CurTradeOutEQ;

    private int tradeReqID;
    private int tradeID;
    private List<trade_item_info> myLockItems = new List<trade_item_info>();





    /// <summary>
    /// 交易对方确认交易事件
    /// </summary>
    public Action OnTradeTargetReply;

    /// <summary>
    /// 交易双方锁定状态变化事件
    /// </summary>
    public Action OnTradeLockUpdate;

    /// <summary>
    /// 交易栏物品变化事件
    /// </summary>
    public Action<LockUpdateType> OnTradeItemUpdate;

    #region 构造
    /// <summary>
    /// 返回该管理类的唯一实例
    /// </summary>
    /// <returns></returns>
    public static TradeMng CreateNew(MainPlayerMng _main)
    {
        if (_main.tradeMng == null)
        {
            TradeMng TradeMng = new TradeMng();
            TradeMng.Init(_main);
            return TradeMng;
        }
        else
        {
            _main.tradeMng.UnRegist(_main);
            _main.tradeMng.Init(_main);
            return _main.tradeMng;
        }
    }
    /// <summary>
    /// 注册
    /// </summary>
    protected virtual void Init(MainPlayerMng _main)
    {
        MsgHander.Regist(0xD631, S2C_OnGetTradeReq);
        MsgHander.Regist(0xD633, S2C_OnGetTradeStart);
        MsgHander.Regist(0xD635, S2C_OnGetTradeLock);
        MsgHander.Regist(0xD639, S2C_OnGetReplyTrade);
        MsgHander.Regist(0xD638, S2C_OnGetTradeFinish);
        //		MsgHander.Regist(0xD401, S2C_OnGetUseSkillList);
        //GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate += ChangeAutoUseSkill;
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected virtual void UnRegist(MainPlayerMng _main)
    {
        MsgHander.UnRegist(0xD631, S2C_OnGetTradeReq);
        MsgHander.UnRegist(0xD633, S2C_OnGetTradeStart);
        MsgHander.UnRegist(0xD635, S2C_OnGetTradeLock);
        MsgHander.UnRegist(0xD639, S2C_OnGetReplyTrade);
        MsgHander.UnRegist(0xD638, S2C_OnGetTradeFinish);
        ReSet();
        //		MsgHander.UnRegist(0xD401, S2C_OnGetUseSkillList);
        //GameCenter.mainPlayerMng.MainPlayerInfo.OnBaseUpdate -= ChangeAutoUseSkill;
    }
    #endregion

    #region 通信S2C

    /// <summary>
    /// 收到交易请求
    /// </summary>
    /// <param name="_info"></param>
    private void S2C_OnGetTradeReq(Pt _info)
    {
        pt_req_trade_s2c_d631 pt = _info as pt_req_trade_s2c_d631;
        if (pt != null)
        {
            tradeReqID = (int)pt.req_id;
            tradeTargetUid = (int)pt.uid;
            OtherPlayerInfo op = GameCenter.sceneMng.GetOPCInfo(tradeTargetUid);
            if (op != null)
            {
                TradeTargetName = op.Name;
            //    Debug.Log("交易对象的名字" + op.Name);
            }
            else
            {
                TradeTargetName = "交易对象";
                Debug.LogError("交易的对象在场景中没有找到！by黄洪兴");
            }

            OnGetTradeReq();
        }
        //Debug.Log("收到交易请求协议");
    }


    /// <summary>
    /// 交易开始
    /// </summary>
    /// <param name="_info"></param>
    private void S2C_OnGetTradeStart(Pt _info)
    {
        pt_trade_start_d633 pt = _info as pt_trade_start_d633;
        if (pt != null)
        {
            tradeID =(int) pt.session_id;
            OtherPlayerInfo op = GameCenter.sceneMng.GetOPCInfo(tradeTargetUid);
            if (op != null)
            {
                TradeTargetName = op.Name;
            //    Debug.Log("交易对象的名字" + op.Name);
            }
            else
            {
                TradeTargetName = "交易对象";
                Debug.LogError("交易的对象在场景中没有找到！by黄洪兴");
            }

            GameCenter.uIMng.SwitchToUI(GUIType.TRADE);
        }
        //Debug.Log("收到交易开始协议" + (int)pt.session_id);
    }

    /// <summary>
    /// 收到对方锁定交易物品的协议
    /// </summary>
    /// <param name="_info"></param>
    private void S2C_OnGetTradeLock(Pt _info)
    {
        pt_reply_lock_trade_d635 pt = _info as pt_reply_lock_trade_d635;
        if (pt != null)
        {
            if ((int)pt.uid != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
            {
                tradeTargetUid = (int)pt.uid;
                for (int i = 0; i < pt.item_list.Count; i++)
                {
                    EquipmentInfo eq = new EquipmentInfo(pt.item_list[i], EquipmentBelongTo.PREVIEW);
                    TradeTargetItems.Add(eq);
                }
                if (OnTradeItemUpdate != null)
                {
                    OnTradeItemUpdate(LockUpdateType.TARGETLOCK);
                }
                TradeTargetLockState = true;
                //Debug.Log("收到对方锁定交易物品的协议");
            }
            else
            {
                TradeMyLockState = true;
            //    Debug.Log("服务端确认自己锁定物品");
            }
            if (OnTradeLockUpdate != null)
            {
                OnTradeLockUpdate();
            }
        }
    }

    /// <summary>
    /// 收到对方确认交易的协议
    /// </summary>
    /// <param name="_info"></param>
    private void S2C_OnGetReplyTrade(Pt _info)
    {
        pt_trade_confirm_d639 pt = _info as pt_trade_confirm_d639;
        if (pt != null)
        {
            if ((int)pt.uid != GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
            {
                tradeTargetUid = (int)pt.uid;
                //targetIsConfirm = true;
                if (OnTradeTargetReply!=null)
                OnTradeTargetReply();
            //    Debug.Log("收到对方确认交易的协议");
            }
        }
    }




    /// <summary>
    /// 收到交易成功完成的协议
    /// </summary>
    /// <param name="_info"></param>
    private void S2C_OnGetTradeFinish(Pt _info)
    {
        pt_trade_finish_d638 pt = _info as pt_trade_finish_d638;
        if (pt != null)
        {
            if ((int)pt.is_succ == 1)
            {
                GameCenter.messageMng.AddClientMsg(278);
            }   
            GameCenter.uIMng.SwitchToUI(GUIType.NONE);
            Debug.Log("收到交易结果的协议pt.is_succ:" + pt.is_succ);
        }
        

    }
    #endregion

    #region C2S

    /// <summary>
    /// 请求交易
    /// </summary>
    /// <param name="num"></param>
    public void C2S_AskTrade(int _uid)
    {
        pt_req_trade_c2s_d630 msg = new pt_req_trade_c2s_d630();
        msg.uid =(uint) _uid;
        tradeTargetUid = _uid;
        NetMsgMng.SendMsg(msg);
        //Debug.Log("发送请求交易的协议，请求对象ID为" + _uid);

    }


    /// <summary>
    /// 回答是否同意开始交易
    /// </summary>
    /// <param name="_uid"></param>
    public void C2S_ConfirmTradeReq(int _req_id,bool _reply)
    {
        pt_reply_trade_req_d632 msg = new pt_reply_trade_req_d632();
        msg.req_id = (uint)_req_id;
        msg.reply =(byte) (_reply ? 1 : 0);
        NetMsgMng.SendMsg(msg);
        //Debug.Log("回答是否同意开始交易,回答结果为" +_reply );

    }
    /// <summary>
    /// 请求锁定物品
    /// </summary>
    /// <param name="_uid"></param>
    public void C2S_AskLockTrade(int _session_id,List<trade_item_info> _itemList)
    {
        pt_req_lock_trade_d634 msg = new pt_req_lock_trade_d634();
        msg.session_id = (uint)_session_id;
        msg.item_list = _itemList;
        NetMsgMng.SendMsg(msg);
        if (OnTradeLockUpdate != null)
        {
            OnTradeLockUpdate();
        }
        //Debug.Log("发送锁定物品的请求，交易ID为" + _session_id);
    }

    /// <summary>
    /// 确认交易
    /// </summary>
    /// <param name="_uid"></param>
    public void C2S_ConfirmTrade(int _session_id)
    {
        pt_req_confirm_trade_d636 msg = new pt_req_confirm_trade_d636();
        msg.session_id = (uint)_session_id;
        NetMsgMng.SendMsg(msg);
        //Debug.Log("发送确认交易请求，交易ID为" + _session_id);
    }

    /// <summary>
    /// 取消交易
    /// </summary>
    /// <param name="_uid"></param>
    public void C2S_CancelTrade()
    {
        pt_req_cancel_trade_d637 msg = new pt_req_cancel_trade_d637();
        msg.session_id = (uint)tradeID;
        NetMsgMng.SendMsg(msg);
        //Debug.Log("发送取消交易请求，交易ID为" + tradeID);
    }


    #endregion


    #region 辅助逻辑


    public void AddTradeItem(int _num)
    {

        if (TradeMyItems.Count >= 10)
        {
            GameCenter.messageMng.AddClientMsg(292);
            return;
        }
        if (CurTradeItemEQ != null)
        {
            EquipmentInfo eq = new EquipmentInfo(CurTradeItemEQ, _num, EquipmentBelongTo.TRADEBOX);
            TradeMyItems.Add(eq);
            for (int i = 0, max = myLockItems.Count; i < max; i++)
            {
                if (myLockItems[i].id == CurTradeItemEQ.InstanceID)//已经存在的
                {
                    myLockItems.Remove(myLockItems[i]);
                    break;
                }
            }  
            trade_item_info info = new trade_item_info();
            info.id =(uint) CurTradeItemEQ.InstanceID;
            info.amount = (uint)_num;//(uint)(_num + oldNum);
            myLockItems.Add(info);
        }
        if (OnTradeItemUpdate!=null)
        {
            OnTradeItemUpdate(LockUpdateType.UPDATEMY);
        }

    }


    public void TakeOutTradeItem()
    {
        if (CurTradeOutEQ != null)
        {
            for (int i = 0, max = TradeMyItems.Count; i < max; i++)
            {
                if (TradeMyItems[i].InstanceID == CurTradeOutEQ.InstanceID)
                {
                    TradeMyItems.Remove(TradeMyItems[i]);
                    break;
                }
            }
            for (int i = 0, max = myLockItems.Count; i < max; i++)
            {
                if ((int)myLockItems[i].id == CurTradeOutEQ.InstanceID)
                {
                    myLockItems.Remove(myLockItems[i]);
                    break;
                }
            }
        } 
        if (OnTradeItemUpdate != null)
        {
            OnTradeItemUpdate(LockUpdateType.TAKEOUR);
        }

    }
    /// <summary>
    /// 判断装备是否在交易中
    /// </summary>
    public bool IsInTradeBox(int _instanceID)
    {
        for (int i = 0,length=TradeMyItems.Count; i < length; i++)
        {
            if (TradeMyItems[i].InstanceID == _instanceID)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 请求交易 参数为玩家ID
    /// </summary>
    public void TryToTrade(int _uid)
    {
        C2S_AskTrade(_uid);
    }

    /// <summary>
    /// 重置交易数据
    /// </summary>
    public void ReSet()
    {

        TradeTargetItems.Clear();
        TradeMyItems.Clear();
        myLockItems.Clear();
        tradeReqID = 0;
        tradeID = 0;
        tradeTargetUid = 0;
        TradeMyLockState = false;
        TradeTargetLockState = false;
    //    CurTradeItem = null;
        CurTradeItemEQ = null;
        CurTradeOutEQ = null;
    //    tradeItems.Clear();
    }

    /// <summary>
    /// 锁定交易
    /// </summary>
    public void LockTrade()
    {
        C2S_AskLockTrade(tradeID, myLockItems);
    }

    /// <summary>
    /// 确认交易
    /// </summary>
    public void ConfirmTrade()
    {
        GameCenter.tradeMng.C2S_ConfirmTrade(tradeID);
    }


    void OnGetTradeReq()
    {
        MessageST mst = new MessageST();
        mst.messID = 277;
        mst.words = new string[1] { TradeTargetName };
        mst.delYes = delegate
        {
            C2S_ConfirmTradeReq(tradeReqID, true);
        };

        mst.delNo = delegate
        {
            C2S_ConfirmTradeReq(tradeReqID, false);
        };
        GameCenter.messageMng.AddClientMsg(mst);
    }




    #endregion
}


public enum LockUpdateType
{
    /// <summary>
    /// 主动增加交易物品
    /// </summary>
    UPDATEMY,
    /// <summary>
    /// 锁定自己交易的物品
    /// </summary>
    MYLOCK,
    /// <summary>
    /// 对方锁定交易物品
    /// </summary>
    TARGETLOCK,
    /// <summary>
    /// 取出物品
    /// </summary>
    TAKEOUR,

}