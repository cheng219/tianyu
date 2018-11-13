//==================================
//作者：朱素云
//日期：2016/4/12
//用途：仙友管理类
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

public class FriendsMng
{
    #region 数据
    /// <summary>
    /// 所有人物数据
    /// </summary>
    public FDictionary friendsDic = new FDictionary();
    protected List<int> list = new List<int>();
    /// <summary>
    /// 所有的好友id
    /// </summary>
    public List<int> friendsUid
    {
        get
        {
            list.Clear();
            foreach (int key in friendsDic.Keys)
            { 
              list.Add(key); 
            }
            return list;
        }
    }
    /// <summary>
    /// 根据好友id获取好友数据
    /// </summary> 
    public FriendsInfo GetFriendsInfoById(int id)
    {
        FriendsInfo info = null;
        info = friendsDic[id] as FriendsInfo;
        return info;
    }
    /// <summary>
    /// 存放好友OthersItemUi脚本字典
    /// </summary>
    public FDictionary items = new FDictionary();
    protected List<FriendsInfo> friends = new List<FriendsInfo>();
    /// <summary>
    /// 好友、黑名单、仇人链表
    /// </summary>
    public List<FriendsInfo> friendList
    {
        get
        { 
            friends.Clear();
            foreach (FriendsInfo value in friendsDic.Values)
            {
                friends.Add(value);
            }
            friends.Sort(SortFriendsInfo); 
            return friends;
        }
    } 
    /// <summary>
    /// 好友管理中勾选的好友id
    /// </summary>
    public List<int> chooseList = new List<int>();
    /// <summary>
    /// 当前点击的好友
    /// </summary>
    public FriendsInfo curFriend;
    /// <summary>
    /// 点击爱心送花给他（他的id）
    /// </summary>
    public int sendFlowerToOne = 0;
    /// <summary>
    /// 送花类型
    /// </summary>
    public int SendFlowerType = 0;
    /// <summary>
    /// 查找好友的信息
    /// </summary>
    public FriendsInfo findFriend = null;
    /// <summary>
    /// 添加的好友是否是好友推送里面的
    /// </summary>
    public bool isAddInAdvice = false;

    /// <summary>
    /// 好友链表 供外部使用
    /// </summary>
    //public FDictionary myFriendDic = new FDictionary();
    /// <summary>
    /// 1 好友，4 仇人， 3黑名单
    /// </summary>
    public Dictionary<int, FDictionary> allFriendDic = new Dictionary<int, FDictionary>();
    #endregion

    #region 事件
    /// <summary>
    /// 好友链表数据变化事件
    /// </summary>
    public System.Action OnFriendsDicUpdata;
    /// <summary>
    /// 仇人链表数据变化
    /// </summary>
    public System.Action OnEnemyDicUpdata;
    /// <summary>
    /// 给某人送花事件
    /// </summary>
    public System.Action OnSendFlowerToSomeone;
    #endregion

    #region 构造
    public static FriendsMng CreateNew()
    {
        if (GameCenter.friendsMng == null)
        {
            FriendsMng friendsMng = new FriendsMng();
            friendsMng.Init();
            return friendsMng;
        }
        else
        {
            GameCenter.friendsMng.UnRegist();
            GameCenter.friendsMng.Init();
            return GameCenter.friendsMng;
        }
    }

    protected void Init()
    {
        MsgHander.Regist(0xD705, S2C_GetFriendsList);
        MsgHander.Regist(0xD710, S2C_GetFriendAfterFind);
        MsgHander.Regist(0xD759, S2C_GetInitimacyUpdata);
    }

    protected void UnRegist()
    {
        MsgHander.UnRegist(0xD705, S2C_GetFriendsList);
        MsgHander.UnRegist(0xD710, S2C_GetFriendAfterFind);
        MsgHander.UnRegist(0xD759, S2C_GetInitimacyUpdata);
        friendsDic.Clear();
        friendList.Clear();
        allFriendDic.Clear();
        list.Clear();
        friends.Clear();
        items.Clear();
        sendFlowerToOne = 0;
        SendFlowerType = 0;
    }
    /// <summary>
    /// 获取玩家一定范围之内的玩家
    /// </summary>
    public void GetNearbyPlayer()
    {  
        FDictionary opcInfoDictionary = GameCenter.sceneMng.OPCInfoDictionary; 
        foreach (OtherPlayerInfo item in opcInfoDictionary.Values)
        { 
            if (!item.IsHide)
            {
                if (!friendsDic.ContainsKey(item.ServerInstanceID))
                {
                    friendsDic[item.ServerInstanceID] = new FriendsInfo(item); 
                }
                else
                {
                    FriendsInfo info = friendsDic[item.ServerInstanceID] as FriendsInfo;
                    info.Updata(item);
                }
            }
        } 
    }
    #endregion

    #region S2C 
    /// <summary>
    /// 接收好友链表
    /// </summary> 
    protected void S2C_GetFriendsList(Pt pt)
    { 
        pt_friend_relation_list_d705 msg = pt as pt_friend_relation_list_d705;
        if (msg != null)
        {
            //for (int j = 0; j < msg.relation_list.Count; j++)
            //{
            //    Debug.Log("d705    add_or_remove：  " + msg.add_or_remove + "   , type : " + msg.relation_list[j].type + "   ,  uid : " + msg.relation_list[j].uid + "   , name : " + msg.relation_list[j].name + "   ,lev : " + msg.relation_list[j].lev + "  " + ConfigMng.Instance.GetLevelDes(msg.relation_list[j].lev));
            //}
            if (isAddInAdvice && msg.add_or_remove == 1)//将好友推送中的人添加到好友列表，删除好友推送中的人
            {
                for (int i = 0, max = msg.relation_list.Count; i < max; i++)
                {
                    if (friendsDic.ContainsKey(msg.relation_list[i].uid))
                    {
                        friendsDic.Remove(msg.relation_list[i].uid);
                    }
                }
            }
            else if (msg.add_or_remove == 1)//添加
            {
                if (msg.relation_list.Count > 0)
                {
                    int type = msg.relation_list[0].type;

                    if (!allFriendDic.ContainsKey(type))
                    {
                        allFriendDic[type] = new FDictionary();
                    }
                    for (int i = 0, max = msg.relation_list.Count; i < max; i++)
                    {
                        FriendsInfo info = new FriendsInfo(msg.relation_list[i]);
                        if (!allFriendDic[type].ContainsKey(msg.relation_list[i].uid))
                        { 
                            allFriendDic[type][msg.relation_list[i].uid] = info;
                        }
                    }
                }
                for (int i = 0, max = msg.relation_list.Count; i < max; i++)
                { 
                    if (!friendsDic.ContainsKey(msg.relation_list[i].uid))
                    {
                        friendsDic[msg.relation_list[i].uid] = new FriendsInfo(msg.relation_list[i]);
                    } 
                }
            }
            else if (msg.add_or_remove == 2)//移除
            {
                if (msg.relation_list.Count > 0)
                {
                    int type = msg.relation_list[0].type;

                    if (allFriendDic.ContainsKey(type))
                    {
                        for (int i = 0, max = msg.relation_list.Count; i < max; i++)
                        { 
                            if (allFriendDic[type].ContainsKey(msg.relation_list[i].uid))
                            {
                                allFriendDic[type].Remove(msg.relation_list[i].uid);
                            }
                        }
                    }
                }

                for (int i = 0, max = msg.relation_list.Count; i < max; i++)
                {
                    if (friendsDic.ContainsKey(msg.relation_list[i].uid))
                    {
                        friendsDic.Remove(msg.relation_list[i].uid);
                    } 
                }
            }
            else//链表
            { 
                curFriend = null;
                friendsDic.Clear();
                for (int i = 0, max = msg.relation_list.Count; i < max; i++)
                {
                    relation_list data = msg.relation_list[i];
                    if (!friendsDic.ContainsKey(data.uid))
                    {
                        friendsDic[data.uid] = new FriendsInfo(data);
                    }
                    else
                    {
                        FriendsInfo info = friendsDic[data.uid] as FriendsInfo;
                        info.Updata(data);
                    } 
                } 
                if (msg.relation_list.Count > 0)
                {
                    FDictionary dic = new FDictionary();
                    foreach (FriendsInfo info in friendsDic.Values)
                    {
                        dic[info.configId] = info;
                    }
                    if (!allFriendDic.ContainsKey(msg.relation_list[0].type))
                    {
                        allFriendDic[msg.relation_list[0].type] = dic;
                    }
                }
            }
        }
        if (msg.relation_list.Count > 0 && msg.relation_list[0].type == 4)
        {
            if (OnEnemyDicUpdata != null) OnEnemyDicUpdata();
        }
        if(OnFriendsDicUpdata != null)OnFriendsDicUpdata(); 
    } 
    /// <summary>
    /// 查找的好友
    /// </summary> 
    protected void S2C_GetFriendAfterFind(Pt pt)
    { 
        pt_update_find_friend_d710 msg = pt as pt_update_find_friend_d710;
        if (msg != null)
        {
            //Debug.Log(" 查找到的好友id : " + msg.uid + "   , name : " + msg.name + "  , lev : " + msg.lev);
            if (findFriend == null) findFriend = new FriendsInfo(msg);
            else findFriend.Updata(msg); 
        }
        if (OnFriendsDicUpdata != null) OnFriendsDicUpdata();
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
            FriendsInfo info = null;
            if (friendsDic.ContainsKey(msg.oth_uid))
            {
                info = friendsDic[msg.oth_uid] as FriendsInfo;
            }
            if (info != null) info.Updata(msg.intimacy);
        }
        if (OnFriendsDicUpdata != null) OnFriendsDicUpdata();
        if (GameCenter.coupleMng.OnCoupleDataUpdata != null) GameCenter.coupleMng.OnCoupleDataUpdata();
    }
    #endregion

    #region C2S 
    /// <summary>
    /// 1 加为好友 3 添加黑名单 2 删除好友
    /// </summary> 
    public void C2S_ReqOperateFriend(FriendOperation _state, List<int> _friends)
    {
        //Debug.Log("请求：1：加好友2：加黑名单3：删除好友    " + _state);
        pt_req_add_del_blacklist_d706 msg = new pt_req_add_del_blacklist_d706();
        msg.state = (int)_state;
        msg.uid_list = _friends;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 加朋友
    /// </summary> 
    public void C2S_AddFriend(int uid)
    {
        //Debug.Log("人物id:" + uid);
        List<int> list = new List<int>();
        list.Add(uid);
        C2S_ReqOperateFriend(FriendOperation.ADDFRIEND, list);
    }
    /// <summary>
    /// 将朋友添加到黑名单
    /// </summary> 
    public void C2S_AddFriendToBlack(int uid)
    {
        List<int> list = new List<int>();
        list.Add(uid);
        C2S_ReqOperateFriend(FriendOperation.ADDBALCKLIST, list);
    }
    /// <summary>
    /// 删除朋友
    /// </summary> 
    public void C2S_DeleteFriend(int uid)  
    {
        List<int> list = new List<int>();
        list.Add(uid);
        C2S_ReqOperateFriend(FriendOperation.DELETEFRIEND, list);
    }
    /// <summary>
    /// 1 好友 2 附近的人 3 黑名单 4 仇人 5 推送列表
    /// </summary> 
    public void C2S_ReqFriendsList(int _state)
    { 
        pt_req_friend_relation_d704 msg = new pt_req_friend_relation_d704();
        msg.state = _state;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 请求好友链表
    /// </summary>
    public void C2S_ReqFriendsList()
    { 
        pt_req_friend_relation_d704 msg = new pt_req_friend_relation_d704();
        msg.state = 1;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 1 移出黑名单 2 去仇人身边 3 换一批（id发0）
    /// </summary> 
    public void C2S_ReqDoSomething(FriendOperation _state, int _id)
    {
        //Debug.Log("好友请求    " + _state + "   ,  id :  " + _id);
        pt_req_firend_other_d709 msg = new pt_req_firend_other_d709();
        msg.state = (int)_state;
        msg.oth_uid = _id;
        NetMsgMng.SendMsg(msg);
    }
    /// <summary>
    /// 查找好友
    /// </summary> 
    public void C2S_ReqFindFriend(string _name)
    {
        //Debug.Log("  我要查找 ： " + _name);
        pt_req_find_firend_d708 msg = new pt_req_find_firend_d708();
        msg.name = _name;
        NetMsgMng.SendMsg(msg);
    } 
    /// <summary>
    /// 送花
    /// </summary> 
    public void C2S_SendFlower(int flowerId, int id, int _type)
    {
        pt_req_give_flower_d707 msg = new pt_req_give_flower_d707();
        msg.source = _type;
        msg.flower_type = flowerId;
        msg.oth_uid = id;
        NetMsgMng.SendMsg(msg);
    }   
    
    #endregion

    #region 辅助逻辑
    protected int SortFriendsInfo(FriendsInfo FriendsInfo1, FriendsInfo FriendsInfo2)
    {
        int state1 = 0;
        int state2 = 0;
        if (FriendsInfo1.IsOnline) state1 = 1;
        if (FriendsInfo2.IsOnline) state2 = 1;
        if (state1 < state2)
            return 1;
        if (state1 > state2)
            return -1;
        if (FriendsInfo1.Intimacy < FriendsInfo2.Intimacy)
            return 1;
        if (FriendsInfo1.Intimacy > FriendsInfo2.Intimacy)
            return -1;
        return 0;
    } 
    #endregion
}
#region 枚举 
public enum FriendOperation
{ 
    /// <summary>
    /// 添加好友d706、移出黑名单d709
    /// </summary>
    ADDFRIEND = 1,
    /// <summary>
    /// 删除好友d706、去仇人身边d709
    /// </summary>
    DELETEFRIEND = 2,
    /// <summary>
    /// 添加到黑名单d706、换一批d709
    /// </summary>
    ADDBALCKLIST = 3,
}
public enum FriendType
{ 
    /// <summary>
    /// 朋友
    /// </summary>
    FRIEND = 0,
    /// <summary>
    /// 附近的人
    /// </summary>
    NEARBY = 1,
    /// <summary>
    /// 黑名单
    /// </summary>
    BLACKLIST = 2,
    /// <summary>
    /// 仇人
    /// </summary>
    ENEMY = 3,
    /// <summary>
    /// 好友推送
    /// </summary>
    COMMEND = 4,
    /// <summary>
    /// 好友管理
    /// </summary>
    MANAGEFRIEND = 5,
}
#endregion
