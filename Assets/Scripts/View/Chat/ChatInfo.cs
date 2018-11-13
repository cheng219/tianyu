//==================================
//作者：黄洪兴
//日期：2016/7/20
//用途：聊天数据层对象
//=================================


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 聊天数据类
/// </summary>
public class ChatInfo {

    /// <summary>
    /// 聊天类型ID
    /// </summary>
    public int chatTypeID = 0;
    /// <summary>
    /// 发送人的用户ID
    /// </summary>
    public int senderID = 0;
    /// <summary>
    /// 发送人的名字
    /// </summary>
    public string senderName = string.Empty;
    /// <summary>
    /// 发送人的VIP等级
    /// </summary>
    public int sendVipLv = 0;
    /// <summary>
    /// 发送人的职业
    /// </summary>
    public int sendProf = 0;

    public string sendTime;
    /// <summary>
    /// 精确到秒的时间
    /// </summary>
    public string accurateTime;


    /// <summary>
    /// 接收人的名字
    /// </summary>
    public string receiveName = string.Empty;


    /// <summary>
    /// 接收人的ID
    /// </summary>
    public int receiveID ;

    /// <summary>
    ///接收人VIP等级
    /// </summary>
    public int recieveVipLv = 0;
    /// <summary>
    /// 收到聊天信息的时间
    /// </summary>
    public long receiveTime;
    /// <summary>
    /// 聊天内容
    /// </summary>
    protected string chatContent=string.Empty;
    /// <summary>
    /// 是否包含装备显示
    /// </summary>
    public bool isContinEquipment = false;


    /// <summary>
    /// 炫耀装备的唯一ID
    /// </summary>
    public int equipmentID=0;


    /// <summary>
    /// 炫耀装备的配置索引
    /// </summary>
    public int equipmentType = 0;

    /// <summary>
    /// 场景ID
    /// </summary>
    public int sceneID=0;

    /// <summary>
    /// 发送的坐标
    /// </summary>
    public Vector3 point = Vector3.zero;

    /// <summary>
    /// 是否是语音
    /// </summary>
    public bool isVoice = false;

    public bool voiceRed = false;

    public string voicePath=string.Empty;
    public int voiceCont;

    /// <summary>
    /// 是否为系统消息
    /// </summary>
    public bool isSystemInfo = false;

    /// <summary>
    /// 需要查看的玩家ID列表
    /// </summary>
    public List<int> playerID = new List<int>();




    public List<int> funcationList = new List<int>();

    /// <summary>
    /// 没有属性变化的物品信息列表
    /// </summary>
    public List<EquipmentInfo> equipmentRefList = new List<EquipmentInfo>();
    /// <summary>
    /// 有属性变化的物品信息列表
    /// </summary>
    public  List<EquipmentInfo> equipmentList = new List<EquipmentInfo>();

    protected EquipmentInfo equipmentInfo;
    /// <summary>
    /// 装备数据
    /// </summary>
    public EquipmentInfo EquipInfo 
    {
        get 
        {
            return equipmentInfo;
        }
    }

    /// <summary>
    /// 聊天类型枚举跟ID对应
    /// </summary>
    public enum Type//没有按照预制上排，是根据服务端
    {
        /// <summary>
        /// 无（聊天界面关闭后则为None）
        /// </summary>
        All = 0,
        /// <summary>
        /// 世界1
        /// </summary>
        World = 1,
        /// <summary>
        /// 组队2
        /// </summary>
        Team=2,
        /// <summary>
        /// 公会3
        /// </summary>
        Guild=3,//
        /// <summary>
        /// 私聊4
        /// </summary>
        Private = 4,
        /// <summary>
        /// 系统消息5
        /// </summary>
        System = 5,
        /// <summary>
        /// 喇叭7
        /// </summary>
        Horn,
    }



    /// <summary>
    /// 收到系统信息
    /// </summary>
    public ChatInfo(pt_system_msg_d689 _pt)
    {

        ChatTemplatesRef Ref = ConfigMng.Instance.GetChatTemplatesRef(_pt.type);
        if (Ref != null)
        {
            isSystemInfo = true;
            chatTypeID = 5;
            if (Ref.channel.Count > 0)
            {
                switch (Ref.channel[0])
                {
                    case 1: chatTypeID = 5; break;
                    case 3: chatTypeID = 3; break;
                    default: chatTypeID = 5; break;
                }
            }
            else
            {
                Debug.LogError("ChatTemplatesRef 表中的channel字段数据异常  by黄洪兴");
            }
            string st = Ref.text;
            int a = 0;
            List<string> list = new List<string>();
            for (int i = 0; i < Ref.parameter.Count; i++)
            {

                switch (Ref.parameter[i])
                {
                    case 1: if (_pt.nomal_list.Count > i) { list.Add(_pt.nomal_list[i].data); funcationList.Add(1); } break;
                    case 2: if (_pt.nomal_list.Count > i) 
                    {
                            int num;
                            if (int.TryParse(_pt.nomal_list[i].data, out num))
                            {
                                EquipmentInfo item = new EquipmentInfo(Convert.ToInt32(_pt.nomal_list[i].data), EquipmentBelongTo.PREVIEW);
                                equipmentRefList.Add(item);
                                list.Add(item.ItemStrColor + item.ItemName + "[-]");
                                funcationList.Add(2);
                            }
                    } 
                    break;
                    case 3: if (_pt.item.Count > a) { EquipmentInfo item = new EquipmentInfo(_pt.item[a], true); equipmentList.Add(item); list.Add(item.ItemStrColor+item.ItemName + "[-]"); a++; funcationList.Add(3); } break;
                    case 4: if (_pt.nomal_list.Count > i)
                        {
                            int num;
                            if (int.TryParse(_pt.nomal_list[i].data,out num))
                            {
                                SceneRef SceneRef = ConfigMng.Instance.GetSceneRef(Convert.ToInt32(_pt.nomal_list[i].data));
                                if (SceneRef != null)
                                    list.Add(SceneRef.name);
                                funcationList.Add(4);
                            }
                        }
                        break;
                    case 5: if (_pt.nomal_list.Count > i)
                        {
                            int num;
                            if (int.TryParse(_pt.nomal_list[i].data, out num))
                            {
                                MonsterRef monsterRef = ConfigMng.Instance.GetMonsterRef(Convert.ToInt32(_pt.nomal_list[i].data));
                                if (monsterRef != null)
                                    list.Add(monsterRef.name);
                            }
                        }
                        break;
                    case 6: if (_pt.nomal_list.Count > i) { list.Add(_pt.nomal_list[i].data); } break;
                    case 7: if (_pt.nomal_list.Count > i) { list.Add(_pt.nomal_list[i].data); } break;
                    case 8: if (_pt.nomal_list.Count > i) { list.Add(_pt.nomal_list[i].data); } break;
                    case 9: if (_pt.nomal_list.Count > i) { list.Add(_pt.nomal_list[i].data); } break;
                    case 10: if (_pt.nomal_list.Count > i)
                        {
                            int num;
                            if (int.TryParse(_pt.nomal_list[i].data, out num))
                            {
                                MonsterRef monsterRef = ConfigMng.Instance.GetMonsterRef(Convert.ToInt32(_pt.nomal_list[i].data));
                                if (monsterRef != null)
                                    list.Add(monsterRef.name);
                            }
                        }
                        break;
                    case 13: if (_pt.nomal_list.Count > i) { list.Add(_pt.nomal_list[i].data); } break;
                    case 14: if (_pt.nomal_list.Count > i)
                        {
                            int num;
                            if (int.TryParse(_pt.nomal_list[i].data, out num))
                            {
                                MountRef mountRef = ConfigMng.Instance.GetMountRef(Convert.ToInt32(_pt.nomal_list[i].data));
                                if (mountRef != null)
                                {
                                    list.Add(mountRef.mountName);
                                }
                            }
                        }
                        break;
                    case 15: if (_pt.nomal_list.Count > i)
                        {
                            int num;
                            if (int.TryParse(_pt.nomal_list[i].data, out num))
                            {
                                RidePropertyRef rideRef = ConfigMng.Instance.GetMountPropertyRef(Convert.ToInt32(_pt.nomal_list[i].data));
                                if (rideRef != null)
                                {
                                    list.Add(rideRef.name);
                                }
                            }
                        }
                        break;
                    case 16: if (_pt.nomal_list.Count > i)
                        {
                            string s = string.Empty;
                            int num;
                            if (int.TryParse(_pt.nomal_list[i].data, out num))
                            {
                                switch (Convert.ToInt32(_pt.nomal_list[i].data))
                                {
                                    case 1: s = ConfigMng.Instance.GetUItext(70); break;
                                    case 2: s = ConfigMng.Instance.GetUItext(71); break;
                                    case 3: s = ConfigMng.Instance.GetUItext(72); break;
                                    case 4: s = ConfigMng.Instance.GetUItext(73); break;
                                    case 5: s = ConfigMng.Instance.GetUItext(74); break;
                                    default:
                                        break;
                                }
                                if (s != string.Empty)
                                    list.Add(s);
                            }
                        }
                        break;
                    case 18: if (_pt.nomal_list.Count > i)
                        {
                            if (_pt.nomal_list[i].data != string.Empty)
                            {
                                    list.Add(_pt.nomal_list[i].data);
                            }
                            else
                            {
                                list.Add(string.Empty);
                            }
                        }
                        break;
                    case 19: if (_pt.nomal_list.Count > i) { list.Add(_pt.nomal_list[i].data); } break;
                    case 20: if (_pt.nomal_list.Count > i)
                        {
                            int num;
                            if (int.TryParse(_pt.nomal_list[i].data, out num))
                            {
                                list.Add(ConfigMng.Instance.GetLevelDes(Convert.ToInt32(_pt.nomal_list[i].data)));
                            }
                        }
                        break;
                    case 23: if (_pt.nomal_list.Count > i)
                        {
                            int num;
                            if (int.TryParse(_pt.nomal_list[i].data, out num))
                            {
                                playerID.Add(Convert.ToInt32(_pt.nomal_list[i].data));
                            }
                        }
                        break;
                    case 24: if (_pt.nomal_list.Count > i)
                        {
                            list.Add(_pt.nomal_list[i].data);
                        }
                        break;
                    case 25: if (_pt.nomal_list.Count > i)
                        {
                            int num;
                            if (int.TryParse(_pt.nomal_list[i].data, out num))
                            {
                                CheckPointRef CKRef = ConfigMng.Instance.GetCheckPointRef(Convert.ToInt32(_pt.nomal_list[i].data));
                                if (CKRef != null)
                                    list.Add(CKRef.name);
                            }
                        }
                        break;
                    case 26: if (_pt.nomal_list.Count > i)
                        {
                            list.Add(_pt.nomal_list[i].data);
                        }
                        break;
                    case 27: if (_pt.nomal_list.Count > i)
                        {
                            list.Add(_pt.nomal_list[i].data);
                        }
                        break;
                    default:
                        break;
                }               
            }

            chatContent = UIUtil.Str2Str(st, list);

            if (Ref.channel.Count > 1)
            {
                MerryGoRoundDataInfo Info=new MerryGoRoundDataInfo(chatContent);
                if (GameCenter.chatMng.OnAddMerryGoRoundData != null)
                {
                    GameCenter.chatMng.OnAddMerryGoRoundData(Info);
                }
            }


        }



    }

    /// <summary>
    /// 客户端自己构造(活动开始前五分钟提示)
    /// </summary>
    public ChatInfo(int _id,string _name,int _type)
    {
        ChatTemplatesRef Ref = ConfigMng.Instance.GetChatTemplatesRef(_id);
        if (Ref != null)
        {
            isSystemInfo = true;
            chatTypeID = 5;
            if (Ref.channel.Count > 0)
            {
                switch (Ref.channel[0])
                {
                    case 1: chatTypeID = 5; break;
                    case 3: chatTypeID = 3; break;
                    default: chatTypeID = 5; break;
                }
            }
            else
            {
                Debug.LogError("ChatTemplatesRef 表中的channel字段数据异常  by黄洪兴");
            }
            string st = Ref.text;
            List<string> list = new List<string>();
            for (int i = 0; i < Ref.parameter.Count; i++)
            {
                switch (Ref.parameter[i])
                {
                    case 27:
                        list.Add(_name);
                        break;
                    default:
                        break;
                }
            }
            chatContent = UIUtil.Str2Str(st, list);
        }
    }




    /// <summary>
    /// 收到聊天信息
    /// </summary>
	public ChatInfo(pt_chat_content_d325 _pt) 
	{

		chatTypeID = (int)_pt.type;
		senderID = (int)_pt.send_uid;
		SenderName = _pt.name;
        sendProf = _pt.prof;
        sendVipLv = _pt.vip_lev;
		ChatContent = _pt.content;
        receiveName = _pt.receive_name;
        if (chatTypeID == 1 || chatTypeID == 2 || chatTypeID == 3 || chatTypeID == 4)//世界、仙盟、队伍、私聊都可以语音
        {
            string[] s = ChatContent.Split(new string[] { "=[Voice]=" }, StringSplitOptions.RemoveEmptyEntries);
            //Debug.Log("当前长度为" + s.Length + ":" + s[0] + ":" + s[1] + ":" + s[2] + ":" + s[3]);
                if (s.Length>0&&s[0].Equals("[Voice]"))
                {
                    isVoice = true;
                    voiceRed = true;
                    voicePath = s[2];
                    voiceCont =(int)Convert.ToDouble(s[3]);
                    if (voiceCont < 1)
                        voiceCont = 1;
                }
        }

        if (voiceRed)//自己的语音永远不亮红点
        {
            if (senderName == GameCenter.mainPlayerMng.MainPlayerInfo.Name)
            {
                voiceRed = false;
            }
        }

        if(_pt.x!=0||_pt.y!=0||_pt.z!=0)
        {
            point = new Vector3(_pt.x, _pt.y,_pt.z);

            string[] words = { ConfigMng.Instance.GetSceneName(_pt.scene), point.x+" "+point.z};
            string s = ConfigMng.Instance.GetUItext(79, words);
            ChatContent = s;

        }
        if (_pt.scene != 0)
        {
            sceneID = _pt.scene;
        }

        if (_pt.item.Count > 0)
        {
            equipmentInfo = new EquipmentInfo(_pt.item[0]);
            ChatContent = "[u][url=5|0]" + equipmentInfo.ItemStrColor + equipmentInfo.ItemName + "[/url][-][/u]";
        }
        if (_pt.item_type > 0)
        {
            equipmentInfo = new EquipmentInfo(_pt.item_type, EquipmentBelongTo.PREVIEW);
            ChatContent = "[u][url=5|0]" + equipmentInfo.ItemStrColor + equipmentInfo.ItemName + "[/url][-][/u]";
        }
        if (senderID == GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID)
        {
            ChatContent = ChatType + "[url=6|0][u][D1FFFE]" + senderName + "[/url][/u][-]" + " : " + ChatContent;
        }
        else
        {
            ChatContent = ChatType + "[url=6|0][u][FF7FEE]" + senderName + "[/url][/u][-]" + " : " + ChatContent;
        }
        if (_pt.time > 0)
        {
            sendTime = ConvertIntDateTime(_pt.time);
            accurateTime = ConvertIntAccurateTime(_pt.time);
        }

		receiveTime = (long)9999999999 - TimeStamp.TimeNowToInt();//时间戳越大，说明聊天越近

	}


    public ChatInfo(Vector3 _point,int _sceneID,int _equipmentID=0)
    {
        chatTypeID = (int)GameCenter.chatMng.CurChatType;
        switch (GameCenter.chatMng.CurChatType)
        {
            case Type.All: chatTypeID = 1;
                break;
            case Type.World:
                break;
            case Type.Team:
                break;
            case Type.Guild:
                break;
            case Type.Private: 
                senderName = GameCenter.mainPlayerMng.MainPlayerInfo.Name;
                receiveName = GameCenter.chatMng.CurTargetName;
                break;
            case Type.System: chatTypeID = 1;
                break;
            case Type.Horn:
                break;
            default:
                break;
        }
        point = _point;
        sceneID = _sceneID;
        equipmentID = _equipmentID;
    }

    public ChatInfo( int _equipmentType)
    {
        chatTypeID = (int)GameCenter.chatMng.CurChatType;
        switch (GameCenter.chatMng.CurChatType)
        {
            case Type.All: chatTypeID = 1;
                break;
            case Type.World:
                break;
            case Type.Team:
                break;
            case Type.Guild:
                break;
            case Type.Private:
                senderName = GameCenter.mainPlayerMng.MainPlayerInfo.Name;
                receiveName = GameCenter.chatMng.CurTargetName;
                break;
            case Type.System: chatTypeID = 1;
                break;
            case Type.Horn:
                break;
            default:
                break;
        }
        equipmentType = _equipmentType;
    }




    /// <summary>
    /// 发送聊天信息
    /// </summary>
    public ChatInfo(int _chatType,string _content,string _targetName)
    {
       
        chatTypeID = _chatType;       
        senderName = GameCenter.mainPlayerMng.MainPlayerInfo.Name;
        receiveName = _targetName;
        chatContent = _content;
    }

    /// <summary>
    /// 发送语音 zsy
    /// </summary>
    public ChatInfo(int _chatType, string _content)
    { 
        chatTypeID = _chatType;
        senderName = GameCenter.mainPlayerMng.MainPlayerInfo.Name; 
        chatContent = _content;
    }
    /// <summary>
    /// 聊天类型字符串
    /// </summary>
    public string ChatType
    {
        get 
        {
            string type = GameCenter.chatMng.ChatType(chatTypeID);
            //if(chatTypeID==5)
            //    return ChatTypeColor((Type)chatTypeID) + "【" + type + "】[-]";
            return ChatTypeColor((Type)chatTypeID) + "【" + type + "】[-]";
        }
    }

    /// <summary>
    /// 聊天类型颜色
    /// </summary>
    protected string ChatTypeColor(Type _chatType) 
    {
        switch (_chatType) 
        {
            case Type.World: return "[ffffff]";//"[efdf33]"
		    case Type.All: return "[ff9000]";//"[e69515]"
            case Type.Guild: return "[30ff00]";//"[91f449]"
            case Type.Team: return "[0070ff]";//"[6bf2f0]"
            case Type.Private: return "[fff710]";//"[cf67e5]"
            case Type.Horn: return "[ff0000]";//"[ddac42]"
            case Type.System: return "[ff0000]";
            default: return "[ff0000]";//"[efdf33]"
        }
    }

    /// <summary>
    /// 聊天的内容
    /// </summary>
    public string ChatContent 
    {
        get 
        {
            return  chatContent;
        }
        protected set 
        {           
            ProcessingChatContent(value);
        }
    }

    /// <summary>
    /// 发送人的名字
    /// </summary>
    protected string SenderName
    {
        set
        {
            if (chatTypeID == (int)Type.System)
            {
               senderName = string.Empty;
            }
           else 
           {
                senderName = value;
           }
        }
    }

    /// <summary>
    /// 聊天内容处理(针对装备的发送)
    /// </summary>
    public void ProcessingChatContent(string _text) 
    {

        for (int i = 0; i < _text.Length; i++)
        {
            string s = _text.Substring(i, 1);
            if (s == "[")
            {
                int index = _text.IndexOf("]", i);
                string ss = _text.Substring(i + 1, index - i - 1);

                

                string[] ps = ss.Split(new char[] { ',' });

                if (ps.Length > 1)
                {
                    int eid = int.Parse(ps[1]);

                    EquipmentInfo info = new EquipmentInfo(eid, EquipmentBelongTo.PREVIEW);
                    equipmentInfo = info;
                    string text = GetItemShowText(info);
                  //  Debug.Log("isContinEquipment");
                    isContinEquipment = true;
                    chatContent = text;
                    return;
                }
            }

        }
        chatContent = _text;
    }

    /// <summary>
    /// 返回物品转成的字符串
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    protected  string GetItemShowText(EquipmentInfo _info)
    {
        string text = "[{0}]【" + _info.ItemName + "】[-]";

        switch (_info.Quality)
        {
            case 1:
                text = string.Format(text, "fbf1bb");
                break;
            case 2:
                text = string.Format(text, "a6d554");
                break;
            case 3:
                text = string.Format(text, "00eaff");
                break;
            case 4:
                text = string.Format(text, "8515ba");
                break;
            case 5:
                text = string.Format(text, "f66e0f");
                break;
            case 6:
                text = string.Format(text, "d30d0c");
                break;
            case 7:
                text = string.Format(text, "f75486");
                break;
            default:
                text = string.Format(text, "ffffff");
                break;
        }

        return text;

    }

    string  ConvertIntDateTime(int _time)
    {
        System.DateTime time = System.DateTime.MinValue;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0));
        //Debug.Log(startTime);
        time = startTime.AddSeconds(_time);
        string s;
        //s ="["+ time.Hour + ":" + time.Minute + ":" + time.Second+"]"; 
        string min;
        if (time.Minute == 0)
        {
            min = "00";
        }
        else if (time.Minute / 10 < 1)
        {
            min = "0" + time.Minute;
        }
        else
        {
            min = time.Minute.ToString();
        }
        s = "[" + time.Hour + ":" + min + "]";
        return s;
    }
    /// <summary>
    /// 精确到秒的时间
    /// </summary> 
    string ConvertIntAccurateTime(int _time)
    {
        System.DateTime time = System.DateTime.MinValue;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0));
        //Debug.Log(startTime);
        time = startTime.AddSeconds(_time);
        string s;
        s =time.Hour + ":" + time.Minute + ":" + time.Second;  
        return s;
    }


    /// <summary>
    /// 聊天的合法性
    /// </summary>
    public bool ContentIsLegal
    {

        get 
        {
            switch ((ChatInfo.Type)chatTypeID)
            {
                case ChatInfo.Type.World:
                    { 
                        return true; 
                    }
                case ChatInfo.Type.Team: 
                    {
                        return true;                                                     
                    }
                case ChatInfo.Type.All: 
                    {  
                            return true;
                    }
                case ChatInfo.Type.Guild: 
                    {
                        bool isInGuild = GameCenter.mainPlayerMng.MainPlayerInfo.GuildName != string.Empty;
                        if (!isInGuild) GameCenter.messageMng.AddClientMsg(361);
                        return isInGuild;  
                    }
                case ChatInfo.Type.Private: { return true; }
                case ChatInfo.Type.System: 
                    {
                        if (GameCenter.inventoryMng.GetNumberByType(2600022) <= 0)
                        {
                            GameCenter.messageMng.AddClientMsg(141); 
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                default: return false;
            }
        }
        
    }

    protected bool JugementTeamContent() 
    {
        
        return true;
    }

    protected bool JugementCampContent()
    {
        return true;
    }

    protected bool JugementGuildContent()
    {
        return true;
    }
    /// <summary>
    /// 語音消息是否播放超時
    /// </summary>
    public bool IsVoiceTimeOut
    {
        get
        {
            return voicePlaying && (Time.time - voiceStartTime) > (voiceCont + 5f);
        }
    }

    protected float voiceStartTime = 0f;
    protected bool voicePlaying = false;
    /// <summary>
    /// 自动播放语音(true表示开始,false表示结束或者未播)
    /// </summary>
    public bool AutoPlayVoice
    {
        set
        {
            if (isVoice)
            {
                if (value)
                {
                    voiceStartTime = Time.time;
                    voicePlaying = true;
                }
                else
                {
                    voicePlaying = false;
                }
            }
        }
    }
}


public enum ChatFuncationType
{
    /// <summary>
    /// 查看玩家
    /// </summary>
    ViewPlayer=1,
    /// <summary>
    /// 传闻展示静态装备
    /// </summary>
    ShowEquipRef=2,
    /// <summary>
    /// 传闻展示真实装备
    /// </summary>
    ShowEquip=3,
    /// <summary>
    /// 寻路坐标
    /// </summary>
    GoPoint=4,
    /// <summary>
    /// 普通炫耀一件装备
    /// </summary>
    Equipmen=5,





}