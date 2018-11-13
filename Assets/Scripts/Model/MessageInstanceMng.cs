//==================================
//作者：何明军
//日期：2015/12/2
//用途：错误提示管理类
//=================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;

/// <summary>
/// 系统提示
/// Messge instance mng.
/// </summary>
public class MessageInstanceMng  {

	public static MessageInstanceMng CreateNew()
    {
        if (GameCenter.messageMng == null)
        {
            MessageInstanceMng messageMng = new MessageInstanceMng();
            messageMng.Init();
            return messageMng;
        }
        else
        {
            GameCenter.messageMng.UnRegist();
            GameCenter.messageMng.Init();
            return GameCenter.messageMng;
        }
    }

    /// <summary>
    /// 注册，构造
    /// </summary>
    protected void Init()
    {
        MsgHander.Regist(0xD001, S2C_ServerMSG);
    }

    /// <summary>
    /// 注销，清理
    /// </summary>
    protected void UnRegist()
    {
        MsgHander.UnRegist(0xD001, S2C_ServerMSG);
        serverMsgList.Clear();
        clientMsgList.Clear();
        clientMsgDic.Clear();
    }
	
	#region 本地
//	private MainPlayerInfo mainPlayerInfo = null;
	/// <summary>
	/// 主玩家创建后，注册属性变化事件 
	/// </summary>
//	void MonitorMainPlayerChange()
//	{
////		mainPlayerInfo = null;
////        mainPlayerInfo = GameCenter.mainPlayerMng.MainPlayerInfo;
//		ListenClientEvent();
//	}
	
	
	/// <summary>
	/// 监听主玩家属性事件 
	/// </summary>
//	public void ListenClientEvent()
//	{
////		mainPlayerInfo.OnPropertyDiffUpdate += DoPropertyTip;
//		mainPlayerInfo.OnBaseDiffUpdate -=DoFightValueTip;
//		mainPlayerInfo.OnBaseDiffUpdate +=DoFightValueTip;
//	}

//	void DoPropertyTip (ActorPropertyTag arg1, int arg2)
//	{
//        if (!SystemSettingMng.ShowStateTexts) return;
//		
//		if(arg1 == ActorPropertyTag.ATKDOWN || arg1 == ActorPropertyTag.ATKUP || arg1 == ActorPropertyTag.DEFDOWN || arg1 == ActorPropertyTag.DEFUP
//			|| arg1 == ActorPropertyTag.HPLIMIT || arg1 == ActorPropertyTag.MPLIMIT || arg1 == ActorPropertyTag.HIT || arg1 == ActorPropertyTag.DOD 
//			|| arg1 == ActorPropertyTag.CRI || arg1 == ActorPropertyTag.SLA || arg1 == ActorPropertyTag.LUCKY || arg1 == ActorPropertyTag.TOUGH
//			|| arg1 == ActorPropertyTag.ADDHURT || arg1 == ActorPropertyTag.REDUCEHURT){
//			return ;
//		}
//		MessageST mess = new MessageST();
//		if(arg2>=0)
//			mess.messID = 30;
//		else
//			mess.messID = 31;
//		mess.words = new string[2];
////		Debug.Log("DoPropertyTip     =   "+arg1.ToString());
//        string configStr = ConfigMng.Instance.GetAttributeTypeName(arg1);
//		mess.words[0] = configStr;
//		mess.words[1] =Mathf.Abs(arg2).ToString();
//		AddClientMsg(mess);
//	}

//	void DoFightValueTip(ActorBaseTag _arg1, int _value,bool _fromAbility)
//	{
//        if (_value == 0) return;
//        if (_arg1 == ActorBaseTag.FightValue)
//        {
//            MessageST mess = new MessageST();
//            if (_value >= 0)
//                mess.messID = 28;
//            else
//            {
//                mess.messID = 29;
//            }
//            mess.words = new string[1];
//            mess.words[0] = Mathf.Abs(_value).ToString();
//            AddClientMsg(mess);
//        }
//        else if (_arg1 == ActorBaseTag.Exp)
//        {
//
//            MessageST mess = new MessageST();
//            mess.messID = 19;
//            mess.words = new string[] { _value.ToString() };
//            AddClientMsg(mess);
//        }
//        else
//        {
//            EquipmentRef eq = ConfigMng.Instance.GetEquipByResIDRef((int)_arg1);
//			if (eq != null && _value > 0)
//            {
//                EquipmentInfo eqInfo = new EquipmentInfo(eq.id, _value, EquipmentBelongTo.PREVIEW);
//				AddClientMsg(eqInfo, 67);
//            }
//        }
//	}
	
	

	
	
	
	#endregion
	List<MessageST> clientMsgList = new List<MessageST>();
	/// <summary>
	/// 获取列表
	/// Gets the client message list.
	/// </summary>
	/// <value>
	/// The client message list.
	/// </value>
	public List<MessageST> ClientMsgList{
		get{
			return clientMsgList;
		}
	}


    protected int sortID = 0;

    protected int NewSortID
    {
        get
        {
            if (sortID >= int.MaxValue)
            {
                sortID = 0;
            }
            return ++sortID;
        }
    }
    Dictionary<int, MessageST> clientMsgDic = new Dictionary<int, MessageST>();//每个提示实例化一个sortID存储在这个队列中，可根据对应的sortID来移除对应的提示
	/// <summary>
	/// 添加客服端提示
	/// Adds the client message.
	/// </summary>
	/// <param name='_id'>
	/// _id.
	/// </param>
	public void AddClientMsg(int _id){
		MessageST mess = new MessageST();
		mess.messID = _id;
		AddClientMsg(mess);
	}
    public int AddClientMsg(int _id, MessageST.MessDel _doYes, MessageST.MessDel _doNo)
    {
        MessageST mess = new MessageST();
        mess.messID = _id;
        mess.sottID = NewSortID;
        mess.delYes = _doYes;
        mess.delNo = _doNo;
        AddClientMsgSort(mess);
        return mess.sottID;
    }
    public int AddClientMsg(int _id, MessageST.MessDel _doYes)
    {
        MessageST mess = new MessageST();
        mess.messID = _id;
        mess.sottID = NewSortID;
        mess.delYes = _doYes;
        AddClientMsgSort(mess);
        return mess.sottID;
    }
    public int AddClientMsg(int _id,string[] _words, MessageST.MessDel _doYes, MessageST.MessDel _doNo)
    {
        MessageST mess = new MessageST();
        mess.messID = _id;
        mess.sottID = NewSortID;
        mess.delYes = _doYes;
        mess.delNo = _doNo;
        mess.words = _words;
        AddClientMsgSort(mess);
        return mess.sottID;
    }

    public int AddClientMsg(int _id, string[] _words, MessageST.MessDel _doYes)
    {
        MessageST mess = new MessageST();
        mess.messID = _id;
        mess.sottID = NewSortID;
        mess.delYes = _doYes;
        mess.words = _words;
        AddClientMsgSort(mess);
        return mess.sottID;
    }

    public void AddClientMsg(int _id, string[] _words)
    {
        MessageST mess = new MessageST();
        mess.messID = _id;
        mess.words = _words;
        AddClientMsg(mess);
    }

	
	public void AddClientMsg(MessageST _mess){
		AddClientMsgNoduplicate(_mess);
	}
    /// <summary>
    /// 二次确认框增加序列号的标记
    /// </summary>
    /// <param name="_mess"></param>
    public void AddClientMsgSort(MessageST _mess)
    {
        clientMsgDic.Add(sortID, _mess);
        clientMsgList.Add(_mess);
        if (UpdateClientMsg != null)
        {
            UpdateClientMsg(_mess);
        }
    }
	/// <summary>
	/// 添加客服端消息 处理重复
	/// </summary>
	/// <param name='_mess'>
	/// _mess.
	/// </param>
	public void AddClientMsgNoduplicate(MessageST _mess){
		clientMsgList.Add(_mess);
		if(UpdateClientMsg != null){
			UpdateClientMsg(_mess);
		}
	}
	
	public void AddClientMsg(string _msg,bool showFrame = true)
	{
		if(updateClientStrMsg != null)
			updateClientStrMsg(_msg,showFrame);
	}
	/// <summary>
	/// 提示活动物品
	/// </summary>
	public void AddClientMsg(List<EquipmentInfo> listEqu,int mstID){
		MessageST mst = null;
		foreach(EquipmentInfo equ in listEqu){
			mst = new MessageST();
			mst.messID = mstID;
			mst.equInfo = equ;
			AddClientMsg(mst);
		}
	}
    /// <summary>
    /// 提示活动物品
    /// </summary>
    public void AddClientMsg(EquipmentInfo eq, int mstID)
    {
        MessageST mst = new MessageST();
        mst.messID = mstID;
        mst.equInfo = eq;
        AddClientMsg(mst);
    }
	
	
	/// <summary>
	/// 去除客服端提示
	/// Removes the client message.
	/// </summary>
	/// <param name='_id'>
	/// _id.
	/// </param>
	public void RemoveClientMsg(int _id){
		MessageST mess = new MessageST();
		mess.messID = _id;
		RemoveClientMsg(mess);
	}
	
	public void RemoveClientMsg(MessageST _mess){
		if(clientMsgList.Contains(_mess)){
			clientMsgList.Remove(_mess);
		}
	}
    /// <summary>
    /// 根据每个提示的sortID来移除提示队列中对应的提示
    /// </summary>
    /// <param name="_sort"></param>
    public void RemoveClientMsgBySortID(int _sort)
    {
        if (clientMsgDic.ContainsKey(_sort))
        {
            RemoveClientMsg(clientMsgDic[_sort]);
        }
        else
        {
            Debug.Log("队列中不存在sort为" + _sort + "的提示");
        }
    }

	List<ErrorMsgStr> serverMsgList = new List<ErrorMsgStr>();
	/// <summary>
	/// 获取列表
	/// Gets the server message list.
	/// </summary>
	/// <value>
	/// The server message list.
	/// </value>
	public List<ErrorMsgStr> ServerMsgList{
		get{
			return serverMsgList;
		}
	}
	/// <summary>
	/// 去除服务端提示
	/// Removes the server message.
	/// </summary>
	/// <param name='_id'>
	/// _id.
	/// </param>
	
	public void RemoveServerMsg(int _id){
		ErrorMsgStr mess = new ErrorMsgStr();
		mess.id = _id;
		RemoveServerMsg(mess);
	}
	
	public void RemoveServerMsg(ErrorMsgStr _mess){
		if(serverMsgList.Contains(_mess)){
			serverMsgList.Remove(_mess);
		}
	}
	
	public void ClearAllMsg(){
		serverMsgList.Clear();
		clientMsgList.Clear();
		if(ClearMsg != null)ClearMsg();
	}
	
	public System.Action ClearMsg;
	/// <summary>
	/// 添加客服端提示，消息
	/// Updates the client message.
	/// </summary>
	/// <returns>
	/// The client message.
	/// </returns>
	/// <param name='type'>
	/// Type.
	/// </param>
	public System.Action<MessageST> UpdateClientMsg;
	/// <summary>
	/// 添加客户端文字提示
	/// </summary>
	public System.Action<string,bool> updateClientStrMsg;
	/// <summary>
	/// 添加服务端提示，消息
	/// Updates the error message string.
	/// </summary>
	/// <returns>
	/// The error message string.
	/// </returns>
	/// <param name='type'>
	/// Type.
	/// </param>
	public System.Action<ErrorMsgStr> UpdateErrorMsgStr;
	/// <summary>
	/// 物品获得提示
	/// </summary>
	public System.Action<List<MessageST>> UpdateClientMsgList;
    ////protected void S2C_ItemDelete(Cmd _cmd)
    ////{
    ////    int id = _cmd.Read_int();
    ////    EquipmentInfo info = new EquipmentInfo(id, EquipmentBelongTo.PREVIEW);
    ////    MessageST mst = new MessageST();
    ////    mst.messID = 245;
    ////    mst.words = new string[1] { info.ItemName };
    ////    AddClientMsg(mst);
    ////}

    protected void S2C_ServerMSG(Pt pt)
    {
        ErrorMsgStr str = new ErrorMsgStr();
        pt_error_info_d001 pt_error_info_d001 = pt as pt_error_info_d001;
        str.id = pt_error_info_d001.error;
        int len = pt_error_info_d001.msg.Count;
        str.msgs = new string[len];
        if (str.id == 6 && !GameCenter.instance.IsReConnecteding)
        {
            GameCenter.instance.GoPassWord();
        }
        for (int i = 0; i < len; i++)
        {
            str.msgs[i] = pt_error_info_d001.msg[i].data;
        }

        if (serverMsgList.Contains(str))
        {
            serverMsgList.Remove(str);
        }else
        {
            serverMsgList.Add(str);

            if (UpdateErrorMsgStr != null)
            {
                UpdateErrorMsgStr(str);
            }
        }
        if (str.id == 1 || str.id == 2 || str.id == 3 || str.id == 113)
        {
            //1\2\3登陆账号密码错误
            //服务器已关闭 关掉 loading by 何明军.
            GameCenter.uIMng.ReleaseGUI(GUIType.WAIT);
        }
    }
    ///// <summary>
    ///// 询问是否消耗星钻复活
    ///// 记录被复活的的玩家ID
    ///// </summary>
    //public int pid = -1;
    //void FHBGOUMAI(object[] arr){
    //    MYPOint myPoint = MYPOint.local2worldV3(GameCenter.curMainPlayer.transform.localPosition);
    //    GameCenter.mainPlayerMng.C2S_AskReBirth(pid,0,myPoint.x,myPoint.y);
    //}
	
	
	public static bool ArrarEquals(object[] objA,object[] objB){
		if(objA == objB){

			return true;
		}
		if(objA != null && objB != null){
			if(objA.Length != objB.Length){
				return false;
			}else{
				for(int i=0;i<objA.Length;i++){
					if(objA[i].ToString() != objA[i].ToString()){
						return false;
					}
				}
				return true;
			}
		}
		return false;
	}

    /// <summary>
    /// 发送推送消息
    /// </summary>
    /// <param name="_id">推送表静态ID 皇室宝箱是 2 每日登录是1, 离线推送 3</param>
    /// <param name="_type">皇室宝箱 1 每日登录 2 , 离线推送 3</param>
    public void SendPushInfo(int _id, int _type, string _time = "")
    {
        PushedRef pushRef = ConfigMng.Instance.GetPushedRef(_id);
        if (pushRef != null)
        {
            string title = pushRef.title;
            string content = pushRef.res;
            if (pushRef.startTime.Count >= 2)
            {
                _time = string.Format("{0:D2}:{1:D2}:00", pushRef.startTime[0], pushRef.startTime[1]);   
                //Debug.Log("  time : " + _time);
            }
            LynSdkManager.Instance.SetNotificationInfos(_type, _time, title, content);
        }
        else
        {
            Debug.LogError("推送表有错，请找策划谢凯");
        }
    }
    /// <summary>
    /// 取消发送推送
    /// </summary>
    /// <param name="_type">取消推送皇室宝箱 11 取消推送每日登录 12 取消离线推送 13</param>
    public void CancelPushInfo(int _type)
    {
        LynSdkManager.Instance.SetNotificationInfos(_type, "00:00:00", "123", "123");
    }
}


//服务器提示
public class ErrorMsgStr
{
    public int id;
	public string[] msgs;
	
	ServerMSGRef data = null;
	public ServerMSGRef Data{
		get{
            if (data == null || data.id != id)
            {
                data = ConfigMng.Instance.GetServerMSGRef(id);
            }
			return data;
		}
	}
	
    public string MessStr{
		get{
			return Data == null ? "没有找到ID = "+id+"数据" : UIUtil.Str2Str(Data.refData.messStr,msgs);
		}
	}
	public int Sort{
		get{
			return Data == null ? 4 : Data.refData.sort;
		}
	}

	public string MessageTitle
	{
		get
		{
			return Data == null ? "提示":Data.refData.title;
		}
	}
	
	public Vector3 FlowStartV3
	{
		get
		{
			return Data == null ? Vector3.zero : Data.refData.flowStartV3;
		}
	}
	
	public Vector3 FlowEndV3
	{
		get
		{
			return Data == null ? Vector3.zero : Data.refData.flowEndV3;
		}
	}
	
	public Vector3 FlowLocaV3
	{
		get
		{
			return Data == null ? Vector3.zero : Data.refData.flowLocaV3;
		}
	}
	
	public int Size
	{
		get
		{
			if(Data == null){
				return 25;
			}
			return Data.refData.size == 0 ? 25:Data.refData.size;
		}
	}
	
	public bool ShowBg
	{
		get
		{
			return Data == null ? false : Data.refData.showBg;
		}
	}
	
	public string TextFont
	{
		get
		{
			return Data == null ? "0":Data.refData.font;
		}
	}
	
	public float StopTime{
		get{
			return Data == null ? 0.4f : Data.refData.stopTime;
		}
		
	}
	
	public float MoveTime{
		get{
			return Data == null ? 0.4f : Data.refData.moveTime;
		}
		
	}
	
	public float HoldTime{
		get{
			return Data == null ? 0 : Data.refData.holdTime;
		}
		
	}
	/// <summary>
	/// 1=移动起始停留
	/// 2=移动中
	/// 3=移动结束停留
	/// (前三种能兼容，后几种只能选其一)
	/// 4=渐隐
	/// 5=渐隐渐现
	/// 6=弹跳效果
	/// 7=停留结束飞入点
	/// </summary>
	public List<int> ShowType{
		get{
			return Data == null ? new List<int>() : Data.refData.showType;
		}
		
	}
	
	public float Acceleration{
		get{
			return Data == null ? 0 : Data.refData.acceleration;
		}
		
	}

	public MsgRefData RefData{
		get{
			MsgRefData dataInfo = new MsgRefData();
			dataInfo.messStr = MessStr ;
			dataInfo.title = MessageTitle;
			dataInfo.font = TextFont;
			dataInfo.sort = Sort;
			dataInfo.flowStartV3 = FlowStartV3;
			dataInfo.flowEndV3 = FlowEndV3;
			dataInfo.flowLocaV3 = FlowLocaV3;
			dataInfo.size = Size;
			dataInfo.showBg = ShowBg;
			dataInfo.stopTime = StopTime;
			dataInfo.moveTime = MoveTime;
			dataInfo.holdTime = HoldTime;
			dataInfo.showType = ShowType;
			dataInfo.acceleration = Acceleration;
			if(Sort == 12 && msgs.Length == 2)//兼容后台发起的物品上浮提示(msgs中第一个参数是物品TYPE,第二个是物品数量) by邓成
			{
				int eType = 0;
				int eNum = 0;
				if(int.TryParse(msgs[0],out eType) && int.TryParse(msgs[1],out eNum))
				{
					dataInfo.item = new ItemValue(eType,eNum);
				}
			}
			return Data == null ? null : dataInfo; 
		}
	}
	
	public bool Equals (ErrorMsgStr item)
	{
		return (id == item.id) && (MessageInstanceMng.ArrarEquals(msgs,item.msgs));
	}
}
//客服端提示
public class MessageST
{
    public int messID;
    public int sottID;
    public bool duplicate= true;
    ServerMSGRef data = null;
    public ServerMSGRef Data
    {
		get{
            if (data == null || data.id != messID)
            {
                data = ConfigMng.Instance.GetServerMSGRef(messID);
            }
			return data;
		}
	}
	/// <summary>
	/// 物品提示
	/// </summary>
	public EquipmentInfo equInfo;
	/// <summary>
	/// 是否显示取消按钮
	/// </summary>
	public bool isShowColse = true;
	/// <summary>
	/// 额外的显示类型
	/// pars = new object[长度]{parsType,....}
	/// parsType = {1=UICheckbox }
	/// </summary>
	public object[] pars;
	/// <summary>
	/// 额外的显示类型中返回值
	/// </summary>
	public MessDel delPars;	
	
	//确定之后调用
	public MessDel delYes;	
	//取消之后调用
	public MessDel delNo; 
	//#?# 传字符串
	public string[] words;
	
	public delegate void MessDel(params object[] _objArr);

	public MsgRefData RefData{
		get{
			MsgRefData dataInfo = new MsgRefData();
			dataInfo.messStr = MessageString ;
			dataInfo.title = MessageTitle;
			dataInfo.font = TextFont;
			dataInfo.sort = MessgeType;
			dataInfo.flowStartV3 = FlowStartV3;
			dataInfo.flowEndV3 = FlowEndV3;
			dataInfo.flowLocaV3 = FlowLocaV3;
			dataInfo.size = Size;
			dataInfo.showBg = ShowBg;
			dataInfo.stopTime = StopTime;
			dataInfo.moveTime = MoveTime;
			dataInfo.holdTime = HoldTime;
			dataInfo.showType = ShowType;
			dataInfo.acceleration = Acceleration;
			if(equInfo != null)
				dataInfo.item = new ItemValue(equInfo.EID,equInfo.StackCurCount);
			return Data == null ? null : dataInfo; 
		}
	}

    public string MessageString
    {
        get
        {
            return Data == null ? "没有找到ID = "+messID+"数据":UIUtil.Str2Str(Data.refData.messStr, words);
        }
    }

    public string MessageTitle
    {
        get
        {
            return Data == null ? "提示":Data.refData.title;
        }
    }
	/// <summary>
	/// 2：文本提示,11：升级提示,12：物品获得提示,4：文本框提示（只有确定）,8：确认框（处理确定，取消）
	/// </summary>
    public int MessgeType
    {
        get
        {
            return Data == null ? 4:Data.refData.sort;
        }
    }
	
	public Vector3 FlowStartV3
    {
        get
        {
            return Data == null ? Vector3.zero : Data.refData.flowStartV3;
        }
    }
	
	public Vector3 FlowEndV3
    {
        get
        {
            return Data == null ? Vector3.zero : Data.refData.flowEndV3;
        }
    }
	
	public Vector3 FlowLocaV3
    {
        get
        {
            return Data == null ? Vector3.zero : Data.refData.flowLocaV3;
        }
    }
	
	public int Size
    {
        get
        {
			if(Data == null){
				return 25;
			}
            return Data.refData.size == 0 ? 25:Data.refData.size;
        }
    }
	
	public bool ShowBg
    {
        get
        {
            return Data == null ? false : Data.refData.showBg;
        }
    }
	
	public string TextFont
    {
        get
        {
			return Data == null ? "0":Data.refData.font;
        }
    }
	
	public float StopTime{
		get{
			return Data == null ? 0.4f : Data.refData.stopTime;
		}
			
	}
	
	public float MoveTime{
		get{
			return Data == null ? 0.4f : Data.refData.moveTime;
		}
			
	}
	
	public float HoldTime{
		get{
			return Data == null ? 0 : Data.refData.holdTime;
		}
			
	}
	/// <summary>
	/// 1=移动起始停留
	/// 2=移动中
	/// 3=移动结束停留
	/// (前三种能兼容，后几种只能选其一)
	/// 4=渐隐
	/// 5=渐隐渐现
	/// 6=弹跳效果
	/// 7=停留结束飞入点
	/// </summary>
	public List<int> ShowType{
		get{
			return Data == null ? new List<int>() : Data.refData.showType;
		}
			
	}
	
	public float Acceleration{
		get{
			return Data == null ? 0 : Data.refData.acceleration;
		}
			
	}
	/// <summary>
	/// 快捷按钮打开的窗口 
	/// </summary>
	public GUIType OpenWndType
	{
		get
		{
			return Data == null?GUIType.NONE:Data.refData.ButtonSort;
		}
	}
	/// <summary>
	/// 描述按钮的文字 
	/// </summary>
	public string ButtonTips
	{
		get
		{
			return Data == null?string.Empty:Data.refData.tips;
		}
	}


	public bool Equals (MessageST obj)
	{
		return (messID == obj.messID) && (delNo == obj.delNo) && MessageInstanceMng.ArrarEquals(pars,obj.pars) && MessageInstanceMng.ArrarEquals(words,obj.words);
	}
}
