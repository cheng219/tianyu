//======================================================
//作者:朱素云
//日期:2016/7/18
//用途:聊天管理类
//======================================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

/// <summary>
/// 聊天数据管理类
/// </summary>
public class ChatMng
{
    #region 数据定义
    public bool isStopMusic = false;
    protected float bgmValue = 0;
    protected float soundEffectValue = 0;
	/// <summary>
	/// 所有聊天数据
	/// </summary>
	public List<ChatInfo> allChatInfo=new List<ChatInfo>();
    /// <summary>
    /// 聊天记录
    /// </summary>
    public Dictionary<int, List<ChatInfo>> chatInfoDic = new Dictionary<int, List<ChatInfo>>();
    /// <summary>
    /// 私聊记录
    /// </summary>
    public Dictionary<string, List<ChatInfo>> PrivateChatDic = new Dictionary<string, List<ChatInfo>>();

    /// <summary>
    /// 私聊新信息数量
    /// </summary>
    public Dictionary<string, int> PrivateChatNewNum = new Dictionary<string, int>();

    public int AllPrivateChatNewNum=0;
    /// <summary>
    /// 每个聊天选项的未读的聊天记录
    /// </summary>
    public Dictionary<ChatInfo.Type, int> chatNumDic = new Dictionary<ChatInfo.Type, int>();



    /// <summary>
    /// 当前私聊对象名字
    /// </summary>
    public string CurTargetName=string.Empty;

    /// <summary>
    /// 当前玩家选择的聊天类型
    /// </summary>
    protected ChatInfo.Type curChatType = ChatInfo.Type.World;
    /// <summary>
    /// 当前玩家选择的聊天类型
    /// </summary>
    public ChatInfo.Type CurChatType
    {
        get
        {
            return curChatType;
        }
        set
        {
            if (curChatType != value)
            {
                curChatType = value;
                if (OnChangeCurChatType != null) OnChangeCurChatType((int)curChatType);
            }
        }
    }


    #region zsy
    /// <summary>
    /// 文字保存内容
    /// </summary>
    public static readonly string SAVED_CHAT = "SAVED_CHAT"; 
    /// <summary>
    /// 文字保存的内容
    /// </summary>
    protected string savedChat = string.Empty;
    public string SavedChat
    {
        get
        {
            return savedChat;
        }
        set
        {
            if (savedChat != value)
            {
                savedChat = value;
                SaveContent();
            }
        }
    }
    /// <summary>
    /// 保存的聊天类型
    /// </summary>
    public ChatInfo.Type savedChatType = ChatInfo.Type.All;

    public List<ChatInfo> voiceList = new List<ChatInfo>();

    public System.Action OnVoiceUpdate;

    protected ChatInfo.Type curVoiceType = ChatInfo.Type.World;
    /// <summary>
    /// 当前玩家选择的语音聊天类型
    /// </summary>
    public ChatInfo.Type CurVoiceType
    {
        get
        {
            return curVoiceType;
        }
        set
        {
            if (curVoiceType != value)
            {
                curVoiceType = value;
                if (OnVoiceUpdate != null)
                    OnVoiceUpdate();
            }
        }
    }

    protected string curVoicePrivateNmae = string.Empty;
    /// <summary>
    /// 当前玩家选择的语音私聊人名
    /// </summary>
    public string CurVoicePrivateNmae
    {
        get
        {
            return curVoicePrivateNmae;
        }
        set
        {
            if (curVoicePrivateNmae != value)
            {
                if (value == null)
                {
                    curVoicePrivateNmae = null;
                    return;
                }
                curVoiceType = ChatInfo.Type.Private;
                curVoicePrivateNmae = value;
                if (OnVoiceUpdate != null)
                    OnVoiceUpdate();
            }
        }
    }

    public List<VoiceData> GetVoiceTypeList()
    {
        List<VoiceData> list = new List<VoiceData>();

        if (curVoiceType != ChatInfo.Type.World)
        {
            list.Add(new VoiceData(ChatInfo.Type.World));
        }
        if (curVoiceType != ChatInfo.Type.Guild && GameCenter.mainPlayerMng.MainPlayerInfo.GuildName != string.Empty)
        {
            list.Add(new VoiceData(ChatInfo.Type.Guild));
        }
        if (curVoiceType != ChatInfo.Type.Team && GameCenter.teamMng.isInTeam)
        {
            list.Add(new VoiceData(ChatInfo.Type.Team));
        }
        List<string> infoList = GetAllPrivateInfoList();
        int privateCount = infoList.Count < 5 ? infoList.Count : 5;
        for (int i = 0, max = privateCount; i < max; i++)
        {
            VoiceData voiceData = new VoiceData(infoList[i]);
            if (!list.Contains(voiceData))
            {
                if (curVoicePrivateNmae != infoList[i] && infoList[i] != GameCenter.mainPlayerMng.MainPlayerInfo.Name)
                { 
                    list.Add(voiceData);
                }
            }
            continue;
        } 

        list.Sort(SotVoiceType);
        return list;
    }

    protected int SotVoiceType(VoiceData voiceData1, VoiceData voiceData2)
    {
        int vd1 = 0;
        int vd2 = 0;

        switch (voiceData1.type)
        {
            case ChatInfo.Type.World:
                vd1 = 4;
                break;
            case ChatInfo.Type.Guild:
                vd1 = 3;
                break;
            case ChatInfo.Type.Team:
                vd1 = 2;
                break;
            case ChatInfo.Type.Private:
                vd1 = 1;
                break;
        }

        switch (voiceData2.type)
        {
            case ChatInfo.Type.World:
                vd2 = 4;
                break;
            case ChatInfo.Type.Guild:
                vd2 = 3;
                break;
            case ChatInfo.Type.Team:
                vd2 = 2;
                break;
            case ChatInfo.Type.Private:
                vd2 = 1;
                break;
        }

        if (vd1 > vd2)
            return 1;
        if (vd1 < vd2)
            return -1;
        return 0;
    }



    #endregion


    /// <summary>
    /// 定位功能CD
    /// </summary>
    public float PointCD;


    //public int TouchFingerNum;

    #endregion


    #region 消息管理类初始化

    public static ChatMng CreateNew()
    {
        if (GameCenter.chatMng == null)
        {
            ChatMng chatMng = new ChatMng();
            chatMng.Init();
            return chatMng;
        }
        else
        {
            GameCenter.chatMng.UnRegist();
            GameCenter.chatMng.Init();
            return GameCenter.chatMng;
        }
    }
    /// <summary>
    /// 注册
    /// </summary>
    protected void Init()
    { 
		MsgHander.Regist(0xD325, S2C_NewRecieveChatContent);
        MsgHander.Regist(0xD689, S2C_RecieveChatTemplates);
        MsgHander.Regist(0xD802, S2C_AddMerryGoRound);
    }
    /// <summary>
    /// 注销
    /// </summary>
    protected void UnRegist()
    {
		MsgHander.UnRegist(0xD325, S2C_NewRecieveChatContent);
        MsgHander.UnRegist(0xD689, S2C_RecieveChatTemplates);
        MsgHander.UnRegist(0xD802, S2C_AddMerryGoRound);
        ResetData(); 
    }

    void ResetData()
    {
        allChatInfo.Clear();
        chatInfoDic.Clear();
        PrivateChatDic.Clear();
        PrivateChatNewNum.Clear();
        AllPrivateChatNewNum = 0;
        bgmValue = 0;
        soundEffectValue = 0;
        chatNumDic.Clear();
        CurTargetName = string.Empty;
        curChatType = ChatInfo.Type.World;
        ClearVoiceCach();
    }
    /// <summary>
    /// 获取保存的聊天内容
    /// </summary>
    public void GetSavedContent()
    {
        if (PlayerPrefs.HasKey(SAVED_CHAT))
        {
            string saved = PlayerPrefs.GetString(SAVED_CHAT);
            string[] content = saved.Split('/');
            if (content.Length > 0) savedChatType = (ChatInfo.Type)(int.Parse(content[0]));
            if (content.Length > 1) savedChat = content[1]; 
        }
        else
        {
            SaveContent();
        }
    }
    /// <summary>
    /// 保存聊天内容
    /// </summary>
    protected void SaveContent()
    {
        if (!string.IsNullOrEmpty(savedChat))
        {
            string saving = (int)savedChatType + "/" + savedChat;
            PlayerPrefs.GetString(saving); 
        }
    }
    /// <summary>
    /// 复原保存内容
    /// </summary>
    public void RecoverSavedContent()
    {
        savedChatType = ChatInfo.Type.All;
        savedChat = string.Empty;
    }
    /// <summary>
    /// 点击玩家名字显示功能
    /// </summary>
    public System.Action<ChatInfo, Vector3> OnShowInformation;


    public System.Action OnUpdatePrivateNewNum;
    public System.Action OnUpdatePrivateRed;
    //public System.Action<ChatInfo> ChatInfoEvent;
    /// <summary>
    /// 外部显示聊天
    /// </summary>
    public System.Action<ChatInfo> ChatInfoOutEvent;
    /// <summary>
    /// 接收聊天内容
    /// </summary>
	protected void S2C_NewRecieveChatContent(Pt _pt)
	{
	
		pt_chat_content_d325 chat=_pt as pt_chat_content_d325;
        if (chat != null)
        {
             //Debug.Log("收到聊天数据" + chat.type + ":" + chat.send_uid + ":" + chat.name + ":" + chat.receive_name + ":" + chat.vip_lev);
            if ((int)chat.type == 6)
            {
                MerryGoRoundDataInfo Info = new MerryGoRoundDataInfo("[u][FF7FEE]" + chat.name + "[/u][-]" + " : "+chat.content);
                if (OnAddMerryGoRoundData != null)
                    OnAddMerryGoRoundData(Info);
            }
            else
            {
                ChatInfo info = new ChatInfo(chat);
                {
                    UpdateChatContent(info);
                }
                if (info.chatTypeID == (int)ChatInfo.Type.Private)
                {
                    UpdateChatPlayer(info);
                }
                UpdateUnReadNum(info);
                AddOneVoiceToMianChat(info);
                AddOneVoiceToMnd(info);
                if ((int)CurChatType == info.chatTypeID || CurChatType == (int)ChatInfo.Type.All && info.chatTypeID != (int)ChatInfo.Type.Private)
                {
                    if (SelectChatTypeEvent != null)
                        SelectChatTypeEvent((int)CurChatType);
                }
                //聊天默认的频道为None
                // if (ChatInfoEvent != null && (info.chatTypeID == (int)curChatType || (int)curChatType == (int)ChatInfo.Type.World || (int)curChatType == (int)ChatInfo.Type.All)) ChatInfoEvent(info);
                if (ChatInfoOutEvent != null) ChatInfoOutEvent(info);
                if (OnVoiceUpdate != null) OnVoiceUpdate(); 

            }
        }
        
	}



    /// <summary>
    /// 收到系统消息
    /// </summary>
    /// <param name="_pt"></param>
    protected void S2C_RecieveChatTemplates(Pt _pt)
    { 
        pt_system_msg_d689 chat = _pt as pt_system_msg_d689;
        if (chat != null)
        {
            ChatInfo info = new ChatInfo(chat);
            UpdateChatContent(info);
            if (info.chatTypeID != (int)CurChatType) SetchatNumDic((ChatInfo.Type)info.chatTypeID, 1);
            //聊天默认的频道为None
            //if (ChatInfoEvent != null && (info.chatTypeID == (int)curChatType || (int)curChatType == (int)ChatInfo.Type.World || (int)curChatType == (int)ChatInfo.Type.All)) ChatInfoEvent(info);
            if (ChatInfoOutEvent != null) ChatInfoOutEvent(info);
            if ((int)CurChatType == info.chatTypeID || CurChatType == (int)ChatInfo.Type.All && info.chatTypeID != (int)ChatInfo.Type.Private)
            {
                if (SelectChatTypeEvent != null)
                    SelectChatTypeEvent((int)CurChatType);
            } 
        } 
    }

    #region 走马灯 by 何明军
    /// <summary>
    /// 走马灯数据增加事件
    /// </summary>
    public Action<MerryGoRoundDataInfo> OnAddMerryGoRoundData;
    protected void S2C_AddMerryGoRound(Pt _info)
    {
        pt_system_notic_d802 info = _info as pt_system_notic_d802;
        if (info == null) return;
        //Debug.Log("info.body  " + info.body);
        MerryGoRoundDataInfo dataInfo = new MerryGoRoundDataInfo(1, info.body);
        if (OnAddMerryGoRoundData != null) OnAddMerryGoRoundData(dataInfo);
    }
    #endregion

    /// <summary>
    /// 更新聊天内容
    /// </summary>
    public void UpdateChatContent(ChatInfo _info) 
    {
        if (_info != null)
        {

            if (!chatInfoDic.ContainsKey(_info.chatTypeID))
            {
                chatInfoDic[(int)_info.chatTypeID] = new List<ChatInfo>();
                chatInfoDic[_info.chatTypeID].Add(_info);
            }
            else
            {
                if (chatInfoDic[_info.chatTypeID].Count < 30)
                {
                    chatInfoDic[_info.chatTypeID].Add(_info);
                }
                else
                {
                    chatInfoDic[_info.chatTypeID].RemoveAt(0);
                    chatInfoDic[_info.chatTypeID].Add(_info);
                }
            }
            if ((ChatInfo.Type)_info.chatTypeID != ChatInfo.Type.Private)
            {

                if (allChatInfo.Count < 30)
                {
                    allChatInfo.Add(_info);
                }
                else
                {
                    allChatInfo.RemoveAt(0);
                    allChatInfo.Add(_info);
                }
            }
        }
    }


    /// <summary>
    /// 更新私聊信息
    /// </summary>
    protected void UpdateChatPlayer(ChatInfo _info)
    {
        string mainPlayerName = GameCenter.mainPlayerMng.MainPlayerInfo.Name;
        if (_info == null)
            return;
        if (_info.senderName == mainPlayerName)
        {
            if (!PrivateChatDic.ContainsKey(_info.receiveName))
            {
                PrivateChatDic[_info.receiveName] = new List<ChatInfo>();
                PrivateChatDic[_info.receiveName].Add(_info);
            }
            else
            {
                PrivateChatDic[_info.receiveName].Add(_info);
            }
            if (CurTargetName == string.Empty)
            {
                CurTargetName = _info.receiveName;
            }
        }
        else
        {
            if (!PrivateChatDic.ContainsKey(_info.senderName))
            {
                PrivateChatDic[_info.senderName] = new List<ChatInfo>();
                PrivateChatDic[_info.senderName].Add(_info);;
            }
            else
            {
                PrivateChatDic[_info.senderName].Add(_info);
            }
            if (CurTargetName == string.Empty)
            {
                CurTargetName = _info.senderName;
                if (PrivateChatNewNum.ContainsKey(_info.senderName))
                {
                    PrivateChatNewNum[_info.senderName]++;
                }
                else
                {
                    PrivateChatNewNum[_info.senderName] = 1;
                }
                if (OnUpdatePrivateNewNum != null)
                    OnUpdatePrivateNewNum();
            }
            else
            {
                if (CurTargetName != _info.senderName)
                {
                    if (PrivateChatNewNum.ContainsKey(_info.senderName))
                    {
                        PrivateChatNewNum[_info.senderName]++;
                    }
                    else
                    {
                        PrivateChatNewNum[_info.senderName] = 1;
                    }
                    if (OnUpdatePrivateNewNum != null)
                        OnUpdatePrivateNewNum();
                }
            }
        }
        AllPrivateChatNewNum = 0;
        using (var e = PrivateChatNewNum.GetEnumerator())
        {
            while (e.MoveNext())
            {
                AllPrivateChatNewNum = AllPrivateChatNewNum + e.Current.Value;
                //Debug.Log("私聊新消息数量" + e.Current.Value);
            }
        }
        if (OnUpdatePrivateRed != null)
            OnUpdatePrivateRed(); 

        //Debug.Log("UpdateChatPlayer" + AllPrivateChatNewNum);
       // Debug.Log("此时私聊信息的长度为" + PrivateChatDic.Count);
       // SelectChatType((int)ChatInfo.Type.Private);   
    }

   

    #endregion

    #region C2S

    public System.Action OnSendMsgSuccess;

    public bool C2S_SendContent(ChatInfo _info)
	{
        if ((ChatInfo.Type)_info.chatTypeID == ChatInfo.Type.All || (ChatInfo.Type)_info.chatTypeID == ChatInfo.Type.World)
        {
            if (!GameCenter.instance.IsSetWorldChatTime)
            {
                GameCenter.instance.IsSetWorldChatTime = true; 
            }
            else
            {
                GameCenter.messageMng.AddClientMsg(360);
                return false;
            }
        }
        if (!_info.ContentIsLegal) return false;
		pt_req_chat_to_world_d318 chat = new pt_req_chat_to_world_d318(); 
        if (_info.chatTypeID == (int)ChatInfo.Type.All)
        {
            chat.type = (uint)ChatInfo.Type.World;
        }
        else
        {
            chat.type = (uint)_info.chatTypeID;
        }
		chat.content = _info.ChatContent;
		chat.target_name =_info.receiveName;
        if (_info.point != Vector3.zero)
        {
            chat.x = (int)_info.point.x;
            chat.y = (int)_info.point.y;
            chat.z = (int)_info.point.z;
        }
        if (_info.equipmentID != 0)
        {
            chat.item_id = _info.equipmentID;
        }
        if (_info.equipmentType != 0)
        {
            chat.item_type = _info.equipmentType;
        }
        if (_info.sceneID != 0)
            chat.scene = _info.sceneID;
        //NGUIDebug.Log(" 敏感字符检测 ：  " + GameCenter.loginMng.CheckBadWord(chat.content));
        if (GameCenter.loginMng.CheckBadWord(chat.content))
        {
            NetMsgMng.SendMsg(chat);
            if (OnSendMsgSuccess != null && !_info.isVoice && _info.chatTypeID != 5) OnSendMsgSuccess();//发送成功后清空输入框
            return true;
            //NGUIDebug.Log("聊天信息已发送！！！！！" + chat.type + ":" + chat.content + ":" + chat.target_name + ":" + _info.point + ":" + _info.equipmentID + ":" + _info.equipmentType);
        }
        return false;
	}




    /// <summary>
    /// 发送聊天信息
    /// </summary>
    public bool SendChatContent(string _content,int _typeID,string _targetName)
    { 
        ChatInfo info = new ChatInfo(_typeID, _content, _targetName);
        return  C2S_SendContent(info);
     //   S2C_RecieveChatContent(info);
       // C2S_SendChatContent(info);
    }

    /// <summary>
    /// 发送语音 zsy
    /// </summary>
    public void SendChatContent(string _content, int _typeID)
    { 
        ChatInfo info = new ChatInfo(_typeID, _content);
        if(C2S_SendContent(info))
            GameCenter.messageMng.AddClientMsg(478);//语音发送成功提示
    }

    protected bool sendEquipSelf = false;//临时的解决方案，有待完善





    #endregion

    #region 辅助逻辑





    /// <summary>
    /// 发送世界公告
    /// </summary>
    public void SendNotice(string _notice)
    {
        if (!GameCenter.instance.IsSetWorldChatTime)
        {
            GameCenter.instance.IsSetWorldChatTime = true;
        }
        else
        {
            GameCenter.messageMng.AddClientMsg(360);
            return;
        }
        pt_req_chat_to_world_d318 chat = new pt_req_chat_to_world_d318();
        chat.type = (uint)ChatInfo.Type.World;
        chat.content = _notice;
        chat.target_name = string.Empty;
        //chat.uid = (uint)GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID;
        NetMsgMng.SendMsg(chat);
      //  Debug.Log("聊天信息已发送！！！！！");
    }


    /// <summary>
    /// 发送指定聊天类型的信息
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_content"></param>
    public void SendMsgByType(ChatInfo.Type _type, string _content,string _targetName="")
    {
       // Debug.Log("发送的消息为:" + _content);
        SendChatContent(_content,(int)_type , _targetName);
    }







    public void OpenPrivateWnd(string _name)
    {
        CurChatType = ChatInfo.Type.Private;
       // Debug.Log("设置打开的聊天类型为私聊");
        GameCenter.uIMng.SwitchToUI(GUIType.CHAT);
         PrivateChatTo(_name);
    }





    /// <summary>
    /// 打开一个私聊
    /// </summary>
    /// <param name="_name"></param>
    public void PrivateChatTo(string _name)
    {
        if (!PrivateChatDic.ContainsKey(_name) && PrivateChatDic.Count >= 5)
        {
            GameCenter.messageMng.AddClientMsg(495);
            return;
        }
        CurTargetName = _name;
        if (!PrivateChatDic.ContainsKey(_name))
        {
            PrivateChatDic[_name] = new List<ChatInfo>();
        }
        if (PrivateChatNewNum.ContainsKey(_name))
        {
            PrivateChatNewNum.Remove(_name);
        }
        SelectChatType((int)ChatInfo.Type.Private);
        AllPrivateChatNewNum = 0;
        using (var e = PrivateChatNewNum.GetEnumerator())
        {
            while (e.MoveNext())
            {
                AllPrivateChatNewNum = AllPrivateChatNewNum + e.Current.Value;
            }
        }
        if (OnUpdatePrivateRed != null)
            OnUpdatePrivateRed();
        //Debug.Log("PrivateChatTo" + AllPrivateChatNewNum);
    }


    /// <summary>
    /// 关闭一个私聊
    /// </summary>
    /// <param name="_name"></param>
    public void ClosePrivateChat()
    {
        if (PrivateChatNewNum.ContainsKey(CurTargetName))
        {
            PrivateChatNewNum.Remove(CurTargetName);
        }
        if (PrivateChatDic.ContainsKey(CurTargetName))
        {
            PrivateChatDic.Remove(CurTargetName);
        }
        if (PrivateChatDic.Count > 0)
        {
            int i = 0;
            using (var e = PrivateChatDic.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (i == 0)
                    {
                        CurTargetName = e.Current.Key;
                        i++;
                    }
                }
            }
        }
        else
        {
            CurTargetName = string.Empty;
        }
        if (SelectChatTypeEvent != null)
            SelectChatTypeEvent((int)ChatInfo.Type.Private);
        AllPrivateChatNewNum = 0;
        using (var e = PrivateChatNewNum.GetEnumerator())
        {
            while (e.MoveNext())
            {
                AllPrivateChatNewNum = AllPrivateChatNewNum + e.Current.Value;
            }
        }
        if (OnUpdatePrivateRed != null)
            OnUpdatePrivateRed();
       // Debug.Log("ClosePrivateChat" + AllPrivateChatNewNum);

    }





    /// <summary>
    /// 初始化聊天类型
    /// </summary>
    public void InitChatType() 
    {
        CurChatType = ChatInfo.Type.All;
    }

    public System.Action<int> OnChangeCurChatType;
    /// <summary>
    /// 选择聊天类型事件
    /// </summary>
    public System.Action<int> SelectChatTypeEvent;
    /// <summary>
    /// 选择聊天类型
    /// </summary>
    public void SelectChatType(int _type) 
    {
        CurChatType = (ChatInfo.Type)_type;
        SetchatNumDic((ChatInfo.Type)_type,2);
        if (CurChatType == ChatInfo.Type.Private)
        {
            if (PrivateChatNewNum.ContainsKey(CurTargetName))
            {
                PrivateChatNewNum.Remove(CurTargetName);
                AllPrivateChatNewNum = 0;
                using (var e = PrivateChatNewNum.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        AllPrivateChatNewNum = AllPrivateChatNewNum + e.Current.Value;
                    }
                }
                if (OnUpdatePrivateRed != null)
                    OnUpdatePrivateRed();
            }
        }
        //{
        //    AllPrivateChatNewNum = 0;
        //    if (OnUpdatePrivateRed != null)
        //        OnUpdatePrivateRed();
        //}
        if (SelectChatTypeEvent != null) SelectChatTypeEvent(_type);
    }



    protected bool isSelectByAnotherFun = false;
    /// <summary>
    /// 是否为外部调用选择聊天类型
    /// </summary>
    public bool IsSelectByAnotherFun 
    {
        get 
        {
            return isSelectByAnotherFun;
        }
    }

    /// <summary>
    /// 关闭窗口初始化
    /// </summary>
    public void InitCloseWnd() 
    {
        isSelectByAnotherFun = false;
        CurChatType = ChatInfo.Type.All;
    }



    /// <summary>
    /// 私聊事件
    /// </summary>
    public System.Action<int> PrivateChatEvent;
    /// <summary>
    /// 私聊事件
    /// </summary>
    public void PrivateChat(int _id)
    {
        
        if (PrivateChatEvent != null) PrivateChatEvent(_id);
    }

    /// <summary>
    /// 选择玩家事件
    /// </summary>
    public System.Action<ChatInfo> SelectPlayerEvent;
    /// <summary>
    /// 选择玩家
    /// </summary>
    public void SelectPlayer(string _name)
    {
        
    }

    /// <summary>
    /// 获取聊天数据通过类型
    /// </summary>
    public List<ChatInfo> GetInfoList(int _type) 
    {
        if (chatInfoDic.ContainsKey(_type)) 
        {
            //if (_type == (int)ChatInfo.Type.Private)
            //{
            //    return GetPrivateInfoList(CurTargetName);
            //}
            return chatInfoDic[_type];
        } 
		if (_type == (int)ChatInfo.Type.All)
			return allChatInfo;
        return new List<ChatInfo>();
    }


    public List<ChatInfo> GetPrivateInfoList(string _name)
    {
        if (_name == null || _name == string.Empty)
            return new List<ChatInfo>();
        if (PrivateChatDic.ContainsKey(_name))
        {
            return PrivateChatDic[_name];
        }
        return new List<ChatInfo>();

    }

    public List<string> GetAllPrivateInfoList()
    {
        List<string> list = new List<string>();
        using (var e = PrivateChatDic.GetEnumerator())
        {
            while (e.MoveNext())
            {
                list.Add(e.Current.Key);
            }
        }
        return list;
    }

    /// <summary>
    /// 获取当前玩家的未读聊天数目
    /// </summary>
    public int GetChatNumIsNotRead(ChatInfo.Type _type)
    {
        if (chatNumDic.ContainsKey(_type)) 
        {
            return chatNumDic[_type];
        }
        return 0;
    }

    /// <summary>
    /// 聊天类型
    /// </summary>
    public string ChatType(int _type)
    {
        switch ((ChatInfo.Type)_type)
        {
            case ChatInfo.Type.World: return "世界";
            case ChatInfo.Type.Team: return "队伍";
		    case ChatInfo.Type.All: return "世界";
            case ChatInfo.Type.Guild: return "仙盟";
            case ChatInfo.Type.Private: return "私聊";
            case ChatInfo.Type.Horn: return "喇叭";
            case ChatInfo.Type.System: return "系统";
            default: return "世界";
        }
    }

    /// <summary>
    /// 按照时间先后对内容进行排序
    /// </summary>
    protected int Sort(ChatInfo _e1,ChatInfo _e2) 
    {
        if (_e1.receiveTime - _e2.receiveTime < 0) return int.MinValue;
        return int.MaxValue;
    }

    public System.Action ChatNumChangeEvent;


    protected void UpdateUnReadNum(ChatInfo info)
    {
        if (info.senderName != GameCenter.mainPlayerMng.MainPlayerInfo.Name)//不是自己的数量加1
        {
            if (GameCenter.uIMng.CurOpenType != GUIType.CHAT)//没有打开聊天界面
            {  
                if (info.chatTypeID != (int)ChatInfo.Type.Private)//当前不是私聊
                {
                    if (!IsThisInfoAutoPlay(info) && info.isVoice)//收到的信息不是自动播放时数量加1
                    {
                        SetchatNumDic((ChatInfo.Type)info.chatTypeID, 1);
                    }
                }
                else 
                {
                    if (!info.isVoice || !IsThisInfoAutoPlay(info))//如果发的文字或者没有开启自动播放
                    {
                        SetchatNumDic((ChatInfo.Type)info.chatTypeID, 1);
                    } 
                }
            }
            else
            {
                if (info.chatTypeID != (int)CurChatType)
                {
                    SetchatNumDic((ChatInfo.Type)info.chatTypeID, 1);
                }
            }
        }
    }
    /// <summary>
    /// 更新每个聊天类型新增加的聊天数目(_operaterType=1加新消息，2，清空，玩家在选择这个选项时清空)
    /// </summary>
    protected void SetchatNumDic(ChatInfo.Type _type,int _operaterType) 
    { 
        if (!chatNumDic.ContainsKey(_type))
        {
            chatNumDic.Add(_type, 0);
        }
        if (_operaterType == 1)
        {
            chatNumDic[_type]++;
        }
        else if (_operaterType == 2)
        {
            chatNumDic[_type] = 0;
            if (_type == ChatInfo.Type.All)
            {
                if (chatNumDic.ContainsKey(ChatInfo.Type.Team))
                {
                    chatNumDic[ChatInfo.Type.Team] = 0;
                }
                if (chatNumDic.ContainsKey(ChatInfo.Type.Guild))
                {
                    chatNumDic[ChatInfo.Type.Guild] = 0;
                } 
            }
            if (_type == ChatInfo.Type.World)//系统的消息在世界里面，所以在世界里清空
            {
                if (chatNumDic.ContainsKey(ChatInfo.Type.System))
                    chatNumDic[ChatInfo.Type.System] = 0;
            }
        }
        if (ChatNumChangeEvent != null) ChatNumChangeEvent();
    }

    #region 语音播放部分

    //protected float recordStar = 0; 
    ///// <summary>
    ///// 语音录取
    ///// </summary>
    //public void VoiceRecord(bool _isPress, ChatInfo.Type _type)
    //{

    //    //int coldTimte = GameCenter.chatMng.curHornColdTime - (int)Time.time;
    //    //if (coldTimte > 0)
    //    //{
    //    //    GameCenter.messageMng.AddClientMsg(343);
    //    //    return;
    //    //}
    //    //SetHornRemainColdTime((int)_type, 0);
    //    // NGUIDebug.Log("isMoveFinger:" + isMoveFinger);
    //    //if (isMoveFinger)
    //    //{
    //    //    GameCenter.uIMng.ReleaseGUI(GUIType.RECORDVOICE);
    //    //    SetFingerMove(false);
    //    //    SetIsRcordingVoice(false);
    //    //    GameCenter.soundMng.SetCurAllVoiceValueAlongSystemSetting(1);
    //    //    InterruptVoiceSelfPlaying = false;
    //    //    return;
    //    //}
    //    if (_isPress)
    //    {
    //        //GameCenter.soundMng.SetCurAllVoiceValue(0);
    //       // TouchFingerNum = Input.touchCount;
    //        SetIsRcordingVoice(true);
    //        //  NGUIDebug.Log(touchFingerNum +":第几个手指在点击");
    //        //GameCenter.uIMng.GenGUI(GUIType.RECORDVOICE, true);
    //        //InterruptVoiceSelfPlaying = true;
    //        YvVoiceSdk.YvVoiceRecordVoice();
    //        recordStar = Time.time;
    //    }
    //    else
    //    {
    //        //GameCenter.uIMng.ReleaseGUI(GUIType.RECORDVOICE);
    //        //SetHornRemainColdTime((int)_type, 1);
    //       // InterruptVoiceSelfPlaying = false;
    //        //if (isRcordingVoice) GameCenter.soundMng.SetCurAllVoiceValueAlongSystemSetting(1);
    //        SetIsRcordingVoice(false);
    //        //string s = "[Voice]=" + "1" + "=" + "2" + "=" + (Time.time - recordStar);
    //       // GameCenter.chatMng.SendChatContent(s, (int)_type, CurTargetName);
    //        float overTime = Time.time - recordStar;
    //        YvVoiceSdk.YvVoiceStopRecordVoice((data1, data2) =>
    //        {
    //            string st = "[Voice]=[Voice]=" + data1 + "=[Voice]=" + data2 + "=[Voice]=" + (overTime);
    //            GameCenter.chatMng.SendChatContent(st, (int)_type,CurTargetName);
    //           // GameCenter.chatMng.C2S_SendVoice(data1, data2, (ChatInfo.Type)_type, _recieveName);
    //            //GameCenter.messageMng.AddClientMsg(342);
    //        });

    //    }
    //}
    /// <summary>
    /// 是否显示正在发送语音的提示
    /// </summary>
    public System.Action<bool> OnShowSendVoiceUpdate;
    protected float recordTime = 0;
    public void VoiceRecordMainWnd(bool _isPress, ChatInfo.Type _type, string name = null) 
    {
        //Debug.Log("VoiceRecordMainWnd :  " + _isPress);
        if (_isPress)
        { 
            SetIsRcordingVoice(true); 
            YvVoiceSdk.YvVoiceRecordVoice();
            recordTime = Time.time;
        }
        else
        { 
            SetIsRcordingVoice(false);
            float overTime = Time.time - recordTime;
            if (OnShowSendVoiceUpdate != null) OnShowSendVoiceUpdate(true);
            YvVoiceSdk.YvVoiceStopRecordVoice((data1, data2) =>
            {
                string st = "[Voice]=[Voice]=" + data1 + "=[Voice]=" + data2 + "=[Voice]=" + (overTime);
                if(!string.IsNullOrEmpty(name))
                    GameCenter.chatMng.SendChatContent(st, (int)_type, name);
                else
                    GameCenter.chatMng.SendChatContent(st, (int)_type);
                if (OnShowSendVoiceUpdate != null) OnShowSendVoiceUpdate(false);
            });

        }
    }


    protected bool isMoveFinger = false;
    /// <summary>
    /// 设置手指是否移动
    /// </summary>
    /// <param name="_state"></param>
    public void SetFingerMove(bool _state)
    {
        isMoveFinger = _state;
    }

    /// <summary>
    /// 是否正在录音
    /// </summary>
    public bool isRcordingVoice = false;

    public void SetIsRcordingVoice(bool _flag)
    {
        //   Debug.Log(_flag);
        isRcordingVoice = _flag;
    }

    /// <summary>
    /// 其他操作中断了语音录音
    /// </summary>
    //public void SuddenlyStopRecordVoice()
    //{

    //    GameCenter.uIMng.ReleaseGUI(GUIType.RECORDVOICE);
    //    Debug.Log(isRcordingVoice + ":SuddenlyStopRecordVoice");
    //    InterruptVoiceSelfPlaying = false;
    //    if (isRcordingVoice)
    //    {
    //        SetIsRcordingVoice(false);
    //        GameCenter.soundMng.SetCurAllVoiceValueAlongSystemSetting(1);
    //    }
    //}

    /// <summary>
    /// 增加一条语音信息
    /// </summary>
    protected void AddOneVoiceInfo(ChatInfo _info)
    {
        // Debug.Log(CurVoiceIsSelfPlay((ChatInfo.Type)_info.chatTypeID) + ":" + IsAutoPlayVoice);
        //if (CurVoiceIsSelfPlay((ChatInfo.Type)_info.chatTypeID) && IsAutoPlayVoice)
        //if (IsThisInfoAutoPlay(_info))
        {
            voiceList.Add(_info);
        }
        //  Debug.Log(voiceList.Count + ":" + isAutoPlayingVoice);
        if (!IsAutoPlayingVoice)
        {
            if (voiceList.Count > 0)
            { 
                IsAutoPlayingVoice = true;
                SlefPlayVoice();
            }
        } 
    }

    //public System.Action VoiceTestEvent;

    //public void VoiceTest()
    //{
    //    //  Debug.Log(voiceList.Count);
    // //   if (voiceList.Count > 0) voiceList.RemoveAt(0);
    //    SlefPlayVoice();
    //} 
    protected ChatInfo voiceInfo = null;
    public ChatInfo curVoiceInfo
    {
        get
        {
            return voiceInfo;
        }
        protected set
        {
            if (voiceInfo != value)
            {
                voiceInfo = value;
                if (OnAutoPlayVoiceUpdate != null)
                    OnAutoPlayVoiceUpdate(curVoiceInfo);
            }
        }
    }
    /// <summary>
    /// 自动播放语音
    /// </summary>
    protected void SlefPlayVoice()
    {
        //NGUIDebug.Log("自动播放语音    IsAutoPlayVoice : " + IsAutoPlayVoice + "    , InterruptVoiceSelfPlaying:" + InterruptVoiceSelfPlaying + "  , IsPausePlayVoice : " + IsPausePlayVoice);
        //if (IsAutoPlayVoice && !InterruptVoiceSelfPlaying && !IsPausePlayVoice)
        if (!InterruptVoiceSelfPlaying && !IsPausePlayVoice)
        {
            if (voiceList.Count > 0)
            { 
                string urlPath = voiceList[0].voicePath;
                string time = voiceList[0].accurateTime; 
                curVoiceInfo = voiceList[0];
                curVoiceInfo.AutoPlayVoice = true;
                YvVoiceSdk.YvVoicePlayVoiceCallBack(urlPath, time, () =>
                {
                    AutoPlayNextVoice(); 
                });
            }
            else
            {
                IsAutoPlayingVoice = false;
            }
        } 
    }
    /// <summary>
    /// 自动播放吓一跳语音,可能是上一条语音播放结束,也可能是上一条语音播放超时  by邓成
    /// </summary>
    public void AutoPlayNextVoice()
    {
        curVoiceInfo.AutoPlayVoice = false;
        curVoiceInfo.AutoPlayVoice = false;
        if (OnAutoPlayWndVoiceUpdate != null) OnAutoPlayWndVoiceUpdate(curVoiceInfo);
        if (voiceList.Count > 0 && !IsPausePlayVoice) voiceList.RemoveAt(0);
        if (voiceList.Count > 0)
        {
            SlefPlayVoice();
        }
        else
        {
            IsAutoPlayingVoice = false;
        }
    }

    ///// <summary>
    ///// 当前聊天类型语音是否自动播放
    ///// </summary>
    //protected bool CurVoiceIsSelfPlay(ChatInfo.Type _type)
    //{
    //    bool flag = false;

    //    flag = GetChatTypeVoiceSelfPlaying(_type);

    //    return flag;
    //}
    /// <summary>
    /// 播放完成
    /// </summary>
    public System.Action<ChatInfo> OnAutoPlayWndVoiceUpdate;
    /// <summary>
    /// 播放开始
    /// </summary>
    public System.Action<ChatInfo> OnAutoPlayVoiceUpdate;
    protected bool isAutoPlayingVoice = false;
    /// <summary>
    /// 是否正在自动播放语音
    /// </summary>
    public bool IsAutoPlayingVoice
    {
        get
        {
            return isAutoPlayingVoice;
        }
        protected set
        {
            if (isAutoPlayingVoice != value)
            {
                isAutoPlayingVoice = value;
                if (!isAutoPlayingVoice)
                { 
                    OnAutoPlayVoiceUpdate(null);
                }
            }
        }
    }
    /// <summary>
    /// 是否自动播放世界频道语音
    /// </summary>
    public bool IsAutoPlayWorld
    {
        get
        {
            return GameCenter.systemSettingMng.AutoPlayWorldVoice; 
        }
    }
    /// <summary>
    /// 是否自动播放仙盟频道语音
    /// </summary>
    public bool IsAutoPlayGuild
    {
        get
        {
            return GameCenter.systemSettingMng.AutoPlayAllyVoice;
        }
    }
    /// <summary>
    /// 是否自动播放队伍频道语音
    /// </summary>
    public bool IsAutoPlayTeam
    {
        get
        {
            return GameCenter.systemSettingMng.AutoPlayTeamVoice;
        }
    }
    /// <summary>
    /// 是否自动播放私聊频道语音
    /// </summary>
    public bool IsAutoPlayPrivateChat
    {
        get
        {
            return GameCenter.systemSettingMng.AutoPlayChatVoice;
        }
    }

    //protected bool isAutoPlayVoice = false;
    /// <summary>
    /// 是否自动播放语音
    /// </summary>
    //public bool IsAutoPlayVoice
    //{ 
        //get
        //{
        //    //  Debug.Log(PlayerPrefs.HasKey("isAutoPlayVoice") + ":" + PlayerPrefs.GetInt("isAutoPlayVoice"));
        //    if (PlayerPrefs.HasKey("isAutoPlayVoice") && YvVoiceSdk.IsWifiNetWork())
        //    {
        //        return PlayerPrefs.GetInt("isAutoPlayVoice") == 1;
        //    }
        //    else
        //    {
        //        PlayerPrefs.SetInt("isAutoPlayVoice", YvVoiceSdk.IsWifiNetWork() ? 1 : 0);
        //        return PlayerPrefs.GetInt("isAutoPlayVoice") == 1;
        //    }

        //}
        //set
        //{
        //    isAutoPlayVoice = value;
        //    //  Debug.Log(isAutoPlayVoice);
        //    PlayerPrefs.SetInt("isAutoPlayVoice", isAutoPlayVoice ? 1 : 0);
        //    if (!isAutoPlayVoice)
        //    {
        //        IsAutoPlayingVoice = false;
        //        //  voiceList.Clear();
        //    }
        //}
    //}
    protected bool ispause = false;
    /// <summary>
    /// 是否暂停语音
    /// </summary>
    public bool IsPausePlayVoice
    {
        get
        {
            return ispause;
        }
        set
        {
            if (ispause != value)
            {
                ispause = value;
                if (!ispause)
                {
                    if (voiceList.Count > 0)
                    { 
                        IsAutoPlayingVoice = true; 
                        SlefPlayVoice();
                    } 
                }
            }
        }
    } 
    public void SetPauseFalse()
    {
        ispause = false;
    }

    protected bool interruptVoiceSelfPlaying = false;
    /// <summary>
    /// 是否中断语音自动播放
    /// </summary>
    public bool InterruptVoiceSelfPlaying
    {
        get
        {
            return interruptVoiceSelfPlaying;
        }
        set
        {
            interruptVoiceSelfPlaying = value;
            if (interruptVoiceSelfPlaying)
            {
                voiceList.Clear();
                isAutoPlayingVoice = false;
            }
        }
    } 
    ///// <summary>
    ///// 设置语音是否自动播放
    ///// </summary>
    ///// <param name="_type"></param>
    ///// <param name="_isSelect"></param>
    //public void SetChatTypeVoiceSelfPlaying(ChatInfo.Type _type, bool _isSelect)
    //{
    //    string type = _type.ToString();
    //    PlayerPrefs.SetInt(type, _isSelect ? 1 : 0);
    //    if (!_isSelect)
    //    {
    //        ClearVoiceListAlongSelfSet(_type);
    //    }
    //}

    /// <summary>
    /// 获得语音自动播放设置
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    //public bool GetChatTypeVoiceSelfPlaying(ChatInfo.Type _type)
    //{
    //    string type = _type.ToString();
    //    //   Debug.Log(PlayerPrefs.HasKey(type) + ":" + YvVoiceSdk.IsWifiNetWork() + ":" + type);
    //    if (PlayerPrefs.HasKey(type) && YvVoiceSdk.IsWifiNetWork())
    //    {
    //        return PlayerPrefs.GetInt(type) == 1;
    //    }
    //    else
    //    {
    //        PlayerPrefs.SetInt(type, YvVoiceSdk.IsWifiNetWork() ? 1 : 0);
    //        return PlayerPrefs.GetInt(type) == 1;
    //    }

    //}

    /// <summary>
    /// 设置自动播放时的语音数据清理
    /// </summary>
    protected void ClearVoiceListAlongSelfSet(ChatInfo.Type _type)
    {
        voiceList.RemoveAll((ChatInfo temp) => temp.chatTypeID == (int)_type);
    }

    /// <summary>
    /// 清除语音缓存数据
    /// </summary>
    protected void ClearVoiceCach()
    {
        InterruptVoiceSelfPlaying = false;
        isAutoPlayingVoice = false;
        isMoveFinger = false;
        curVoiceInfo = null;
        ispause = false;
        curAutoPlayType = CurAutoPlayVoiceType.MAINCHAT;
        voiceList.Clear();
    }

    #endregion





    #region 聊天界面语音自动播放zsy
    /// <summary>
    /// 主界面新增一条语音
    /// </summary> 
    protected void AddOneVoiceToMianChat(ChatInfo info)
    {
        if (curAutoPlayType == CurAutoPlayVoiceType.MAINCHAT)
        {
            //bool isAddToList = false;
            //NGUIDebug.Log("主界面新增一条语音 curVoicePrivateNmae : " + curVoicePrivateNmae + "   , senderName : " + info.senderName);
            if (info.isVoice && info.senderName != GameCenter.mainPlayerMng.MainPlayerInfo.Name)
            { 
                if (IsThisInfoAutoPlay(info))
                {
                    AddOneVoiceInfo(info);
                }
            }
        }
    }
    /// <summary>
    /// 界面上新增一条语音
    /// </summary> 
    protected void AddOneVoiceToMnd(ChatInfo info)
    {
        if (curAutoPlayType == CurAutoPlayVoiceType.CHATWND)
        {
            //NGUIDebug.Log("界面上新增一条语音 curChatType : " + curChatType + "   , senderName : " + info.senderName);
            if ((int)CurChatType == info.chatTypeID && info.isVoice)//当前界面来了一条语音
            {
                if (CurChatType != ChatInfo.Type.Private)
                {
                    AddOneVoiceInfo(info);
                }
                else
                {
                    if (CurTargetName == info.senderName)
                    {
                        AddOneVoiceInfo(info);
                    }
                }
            }
        }
    } 

    protected CurAutoPlayVoiceType curAutoPlayType = CurAutoPlayVoiceType.MAINCHAT;
    public CurAutoPlayVoiceType CurAutoPlayType
    {
        get
        {
            return curAutoPlayType;
        }
    }
    /// <summary>
    /// 结束界面自动播放设置当前自动播放类型为主界面
    /// </summary> 
    public void SetCurAutoPlayTypeNoPlay()
    {
        voiceList.Clear();
        isAutoPlayingVoice = false;
        if (interruptVoiceSelfPlaying)
        {
            interruptVoiceSelfPlaying = false;
        }
        if (curAutoPlayType != CurAutoPlayVoiceType.MAINCHAT)
        {
            curAutoPlayType = CurAutoPlayVoiceType.MAINCHAT;
        }
    }
    /// <summary>
    /// 设置界面语音自动播放
    /// </summary> 
    public void SetCurAutoPlayType(ChatInfo _info)
    {
        voiceList.Clear(); 
        curAutoPlayType = CurAutoPlayVoiceType.CHATWND;  
        ispause = false;
        interruptVoiceSelfPlaying = false;
        isAutoPlayingVoice = false;
        List<ChatInfo> list = new List<ChatInfo>();
        if (CurChatType != ChatInfo.Type.Private)
        {
            list = GetInfoList((int)CurChatType);
        }
        else
        {
            if (!string.IsNullOrEmpty(CurTargetName))
            {
                list = GetPrivateInfoList(CurTargetName);
            }
        }
        int curVal = 0;
        for (int i = 0, max = list.Count; i < max; i++)
        {
            if (list[i].accurateTime == _info.accurateTime)//播放该条语音下面的语音
            {
                curVal = i;
            }
        }
        for (int i = 0, max = list.Count; i < max; i++)
        {
            if (i >= curVal && list[i].isVoice && list[i].voiceRed)
            {
                voiceList.Add(list[i]);
            } 
        } 
        if (voiceList.Count > 0)
        {
            if (!IsAutoPlayingVoice)
            { 
                IsAutoPlayingVoice = true;
                SlefPlayVoice();
            }
        }
    }
    /// <summary>
    /// 录音或播放语音前保存声音大小
    /// </summary>
    protected void SaveSoundValue()
    {
        if (bgmValue != 0.00123) bgmValue = PlayerPrefs.HasKey(SystemSettingMng.SETTING_NAME_BGM_VOLUME) ? PlayerPrefs.GetFloat(SystemSettingMng.SETTING_NAME_BGM_VOLUME) : 1.0f;
        if (soundEffectValue != 0.00123) soundEffectValue = PlayerPrefs.HasKey(SystemSettingMng.SETTING_NAME_SOUND_EFFECT_VOLUME) ? PlayerPrefs.GetFloat(SystemSettingMng.SETTING_NAME_SOUND_EFFECT_VOLUME) : 1.0f; 
    }
    /// <summary>
    /// 将声音大小设置为0，用于录音和播放语音
    /// </summary>
    public void StopSound()
    {
        isStopMusic = true;
        SaveSoundValue();
        GameCenter.systemSettingMng.BGMVolume = 0.00123f;
        GameCenter.systemSettingMng.SoundEffectVolume = 0.00123f;
    }
    /// <summary>
    /// 恢复声音，用于录音完毕和播放完毕
    /// </summary>
    public void ResetBGM()
    {
        isStopMusic = false;
        if (GameCenter.systemSettingMng.OpenMusic)
        { 
            GameCenter.systemSettingMng.BGMVolume = bgmValue;
        }
        if (GameCenter.systemSettingMng.OpenSoundEffect)
        { 
            GameCenter.systemSettingMng.SoundEffectVolume = soundEffectValue;
        } 
    }

    protected bool IsThisInfoAutoPlay(ChatInfo _info)
    {
        switch (_info.chatTypeID)
        {
            case (int)ChatInfo.Type.World:
                return IsAutoPlayWorld; 
            case (int)ChatInfo.Type.Guild:
                return IsAutoPlayGuild; 
            case (int)ChatInfo.Type.Team:
                return IsAutoPlayTeam; 
            case (int)ChatInfo.Type.Private:
                return IsAutoPlayPrivateChat; 
            default:
                return false;
        }
    }
    #endregion

    #endregion

}

/// <summary>
/// 当前自动播放语音的类型，主界面和聊天界面
/// </summary>
public enum CurAutoPlayVoiceType
{
    /// <summary>
    /// 主界面
    /// </summary>
    MAINCHAT,
    /// <summary>
    /// 聊天界面
    /// </summary>
    CHATWND,
}