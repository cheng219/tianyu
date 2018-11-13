/// <summary>
/// 何明军
/// 2016/4/19
/// 邮件系统
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;
using System.Text;

public class XXMailMng {
	public const string colorTip = "[-]";
	#region 事件, 变量与访问器
	/// <summary>
	/// 邮件列表
	/// </summary>
	public Dictionary<int,MailData> mailDic = new Dictionary<int, MailData>();
	/// <summary>
	/// 邮件列表
	/// </summary>
	public List<MailData> MailDataList{
		get{
			List<MailData> list = new List<MailData>(mailDic.Values);
			list.Sort(CompareSendTime);
			return list;
		}
	}
	
	int CompareSendTime(MailData x,MailData y){
		return x.CompareTo(y);
	}
	
	public enum MailState{
		/// <summary>
		/// 0=全部
		/// </summary>
		All,
		/// <summary>
		/// 1=已读
		/// </summary>
		IsRead,
		/// <summary>
		/// 2=未读
		/// </summary>
		Read,
	}
	/// <summary>
	/// 0=全部，1=已读，2=未读
	/// </summary>
	public Dictionary<int,MailData> MailDic(MailState type){
		Dictionary<int,MailData> mailDicEty = new Dictionary<int, MailData>();
		List<MailData> list = new List<MailData>(mailDic.Values);
		list.Sort(CompareSendTime);
		for(int i=0;i<list.Count;i++){
			if(type == MailState.All){
				mailDicEty[list[i].id] = list[i];
			}else if(type == MailState.Read && list[i].IsRead){
				mailDicEty[list[i].id] = list[i];
			}else if(type == MailState.IsRead && !list[i].IsRead){
				mailDicEty[list[i].id] = list[i];
			}
		}
		return mailDicEty;
	} 
	/// <summary>
	/// 总页数
	/// </summary>
	public int TotalPage = 0;
	/// <summary>
	/// 写邮件数据
	/// </summary>
	public MailWriteData mailWriteData = null;
	/// <summary>
	/// 邮件数据更新
	/// </summary>
	public Action OnMailWriteDataUpdate;
	/// <summary>
	/// 邮件列表更新
	/// </summary>
	public Action OnMailListUpdate;
	/// <summary>
	/// 邮件发送成功
	/// </summary>
	public Action OnSendSuccessMail;
	#endregion
	#region 构造
	public static XXMailMng CreateNew(){
		if (GameCenter.mailBoxMng == null)
		{
			XXMailMng mailBoxMng = new XXMailMng();
			mailBoxMng.Init();
			return mailBoxMng;
		}
		else
		{
			GameCenter.mailBoxMng.UnRegist();
			GameCenter.mailBoxMng.Init();
			return GameCenter.mailBoxMng;
		}
	} 
	
	protected void Init(){
		MsgHander.Regist(0xD339,S2C_AllMailList);
		MsgHander.Regist(0xD335,S2C_AllMailDelete);
		MsgHander.Regist(0xD337,S2C_AllMailContent);
		MsgHander.Regist(0xD340,C2S_SendSuccessMail);
	}
	
	protected void UnRegist(){
		mailWriteData = null;
		mailDic.Clear();
		TotalPage = 0;
		MsgHander.UnRegist(0xD339,S2C_AllMailList);
		MsgHander.UnRegist(0xD335,S2C_AllMailDelete);
		MsgHander.UnRegist(0xD337,S2C_AllMailContent);
		MsgHander.UnRegist(0xD340,C2S_SendSuccessMail);
	}
	
	#endregion
	
	void CheckAllMailState(){
		foreach(MailData data in mailDic.Values){
			if(data.IsRead){
				GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.MAIL,true);
				return ;
			}
		}
		GameCenter.mainPlayerMng.SetFunctionRed(FunctionType.MAIL,false);
	}
	#region  协议接受
	void S2C_AllMailList(Pt _info){
		pt_all_mail_list_d339 info = _info as pt_all_mail_list_d339;
		if(info == null)return ;
		for(int i=0;i<info.mails.Count;i++){
			base_mail_list data = info.mails[i];
			if(!mailDic.ContainsKey(data.id)){
				mailDic[data.id] = new MailData(data);
			}else{
				mailDic[data.id].Update(data);
			}
		}
		CheckAllMailState();
	}
	
	void S2C_AllMailDelete(Pt _info){
		pt_del_mail_d335 info = _info as pt_del_mail_d335;
		if(info == null)return ;
		for(int i=0;i<info.id_list.Count;i++){
			int id = (int)info.id_list[i];
			if(mailDic.ContainsKey(id)){
				mailDic.Remove(id);
			}
		}
		if(OnMailListUpdate != null)OnMailListUpdate();
		CheckAllMailState();
	}
	
	void S2C_AllMailContent(Pt _info){
		pt_mail_info_list_d337 info = _info as pt_mail_info_list_d337;
		if(info == null)return ;
		for(int i=0;i<info.infos.Count;i++){
			mail_info_list data = info.infos[i];
			if(mailDic.ContainsKey(data.id)){
				mailDic[data.id].Update(data);
			}
		}
		CheckAllMailState();
	}
	
	void C2S_SendSuccessMail(Pt _info){
		//pt_req_send_mail_d340 info = _info as pt_req_send_mail_d340;
		if(OnSendSuccessMail != null)OnSendSuccessMail();
	}
	#endregion
	
	#region  协议发送
	/// <summary>
	/// 删邮件
	/// </summary>
	public void C2S_DeleteMail(List<uint> idList){
		if(idList.Count <= 0)return ;
		pt_del_mail_d335 info = new pt_del_mail_d335();
		info.id_list = idList;
		NetMsgMng.SendMsg(info);
	}
	/// <summary>
	/// 邮件列表
	/// </summary>
	public void C2S_MailList(){
		pt_req_mail_list_d336 info = new pt_req_mail_list_d336();
		NetMsgMng.SendMsg(info);
	}
	/// <summary>
	/// 读邮件
	/// </summary>
	public void C2S_ReadMail(int _id){
		pt_req_read_mail_d338 info = new pt_req_read_mail_d338();
		info.id = (uint)_id;
		NetMsgMng.SendMsg(info);
	}
	/// <summary>
	/// 一键提取邮件
	/// </summary>
	public void C2S_ExtractMail(List<uint> idList){
		pt_req_get_mail_items_d341 info = new pt_req_get_mail_items_d341();
		info.id_list = idList;
		NetMsgMng.SendMsg(info);
	}
	/// <summary>
	/// 发邮件
	/// </summary>
	public void C2S_ReplyMail(MailWriteData data){
		pt_req_send_mail_d340 info = new pt_req_send_mail_d340();
		info.receive_name = data.name;
		info.title = data.titil;
		info.content = data.content;
		NetMsgMng.SendMsg(info);
	}
	#endregion
}
/// <summary>
/// 邮件数据
/// </summary>
public class MailData : IComparable<MailData>{
	/// <summary>
	/// 服务端ID
	/// </summary>
	public int id;
	/// <summary>
	/// 类型
	/// </summary>
	public int type;
	/// <summary>
	/// 状态
	/// </summary>
	public int read_state;
	/// <summary>
	/// 标题
	/// </summary>
	public string title;
	/// <summary>
	/// 内容
	/// </summary>
	public string content;
	/// <summary>
	/// 内容类型
	/// </summary>
	public int contentType = 0;
	List<EquipmentInfo> contentItems = new List<EquipmentInfo>();
	/// <summary>
	/// 标题
	/// </summary>
	public string Title{
		get{
			if(type == 2){
				SystemMailRef smail = ConfigMng.Instance.GetSystemMailRef(contentType);
                if (smail == null || smail.title == string.Empty)
                {
					return title;
				}
				return smail.title;
			}
			return title;
		}
	}
	/// <summary>
	/// 内容
	/// </summary>
	public string Content{
		get{
			if(type == 2 && contentType > 0){
				SystemMailRef smail = ConfigMng.Instance.GetSystemMailRef(contentType);
				if(smail == null){
					return content;
				}
				string[] str = new string[contentItems.Count * 2];
				int i = 0;
				for(int j=0;j<contentItems.Count;j++){
					if(contentItems[j] != null){
						str[i] = contentItems[j].ItemStrColor + contentItems[j].ItemName+XXMailMng.colorTip;
						i++;
						str[i] = contentItems[j].StackCurCount.ToString();
						i++;
					}
				}
				return UIUtil.Str2Str(smail.content,str);
			}
			return content;
		}
	}
	/// <summary>
	/// 附加物品
	/// </summary>
	public List<EquipmentInfo> items = new List<EquipmentInfo>();
	string send_name;
	int send_time;
	int expire_time;
	/// <summary>
	/// 内容更新
	/// </summary>
	public System.Action OnMailContentUpdate;
	/// <summary>
	/// 0=未读，1=已读
	/// </summary>
	public bool IsRead{
		get{
			return read_state == 0;
		}
	}
	/// <summary>
	/// 发送人
	/// </summary>
	public string SendName{
		get{
			if(type == 2){
				return ConfigMng.Instance.GetUItext(3);
			}
			return send_name;
		}
	}
	/// <summary>
	/// 发送时间
	/// </summary>
	public string SendTime{
		get{
			DateTime date = GameHelper.ToChinaTime(new DateTime(1970,1,1)).AddSeconds((double)send_time);
			return string.Format("{0}/{1}/{2} {3}:{4}:{5}",date.Year,StringAdd(date.Month),StringAdd(date.Day),StringAdd(date.Hour),StringAdd(date.Minute),StringAdd(date.Second));
		}
	}
	string ety = "0";
	string StringAdd(int val){
		return val < 10 ? ety + val.ToString() : val.ToString();
	}
	/// <summary>
	/// 到期时间
	/// </summary>
	public string ExpireTime{
		get{
			if(expire_time <= 0)return "0";
			DateTime date = GameHelper.ToChinaTime(new DateTime(1970,1,1)).AddSeconds((double)(send_time + expire_time));
			if(date.CompareTo(GameCenter.instance.CurServerTime) < 0){
				return string.Empty;
			}
			int extime = (int)(date - GameCenter.instance.CurServerTime).TotalSeconds;
			int hours = extime%3600 > 0 ? extime/3600 + 1 : extime/3600;
			if(hours / 24 > 0){
				string[] str = new string[1]{hours%24 > 0 ? (hours/24 + 1).ToString() : (hours/24).ToString()};
				return ConfigMng.Instance.GetUItext(4,str);
			}
			string[] str1 = new string[1]{hours.ToString()};
			return ConfigMng.Instance.GetUItext(5,str1);
		}
	}
	
	protected bool isNewContent = false;
	/// <summary>
	/// 是否更新过内容
	/// </summary>
	public bool IsNewContent{
		get{
			return isNewContent;
		}
	}
	/// <summary>
	/// 更新
	/// </summary>
	public void Update(mail_info_list data){
		isNewContent = true;
		this.read_state = data.read_state;
		this.title = data.title;
		this.content = data.content;
		int lenitems = data.items.Count;
		
		if(lenitems > 0){
			this.items.Add(new EquipmentInfo(1,0,EquipmentBelongTo.PREVIEW));
			this.items.Add(new EquipmentInfo(6,0,EquipmentBelongTo.PREVIEW));
			this.items.Add(new EquipmentInfo(5,0,EquipmentBelongTo.PREVIEW));
			this.items.Add(new EquipmentInfo(18,0,EquipmentBelongTo.PREVIEW));
			EquipmentInfo info = null;
			for(int i_items = 0 ; i_items < lenitems ; i_items ++)
			{
				if (data.items[i_items].type == 1){
					this.items[0] = new EquipmentInfo(data.items[i_items], EquipmentBelongTo.PREVIEW);
				}else if (data.items[i_items].type == 6){
					this.items[1] = new EquipmentInfo(data.items[i_items], EquipmentBelongTo.PREVIEW);
				} else if (data.items[i_items].type == 5){
					this.items[2] = new EquipmentInfo(data.items[i_items], EquipmentBelongTo.PREVIEW);
				}else if (data.items[i_items].type == 18){
					this.items[3] = new EquipmentInfo(data.items[i_items], EquipmentBelongTo.PREVIEW);
				}else{
					info = new EquipmentInfo(data.items[i_items],EquipmentBelongTo.PREVIEW);
					this.items.Add(info);
				}
			}
		}else{
			items.Clear();
		}
		this.send_name = data.send_name;
		this.send_time = data.send_time;
		this.expire_time = data.expire_time;
		
		this.contentType = data.content_type;
		lenitems = data.system_mail_args.Count;
		if(lenitems > 0){
			EquipmentInfo info = null;
			for(int i_items = 0 ; i_items < lenitems ; i_items ++)
			{
				info = new EquipmentInfo(data.system_mail_args[i_items].type,data.system_mail_args[i_items].num,EquipmentBelongTo.PREVIEW);
				this.contentItems.Add(info);
			}
		}else{
			contentItems.Clear();
		}
		if(OnMailContentUpdate != null)OnMailContentUpdate();
	}
	/// <summary>
	/// 更新
	/// </summary>
	public void Update(base_mail_list data){
		this.id = data.id;
		this.type = data.type;
		this.read_state = data.read_state;
		this.title = data.title;
		this.send_name = data.send_name;
		this.send_time = data.send_time;
		this.expire_time = data.expire_time;
		this.contentType = data.content_type;
	}
	/// <summary>
	/// 构造
	/// </summary>
	public MailData(base_mail_list data){
		this.id = data.id;
		this.type = data.type;
		this.read_state = data.read_state;
		this.title = data.title;
		this.send_name = data.send_name;
		this.send_time = data.send_time;
		this.expire_time = data.expire_time;
		this.contentType = data.content_type;
		contentItems.Clear();
		isNewContent = false;
	}
	/// <summary>
	/// 排序
	/// </summary>
	public int CompareTo(MailData x){
		if(x == null)return 1;
		return x.send_time.CompareTo(this.send_time);
	}
}


public class MailWriteData{
	/// <summary>
	/// 发送名
	/// </summary>
	public string name;
	/// <summary>
	/// 内容
	/// </summary>
	public string content;
	/// <summary>
	/// 标题
	/// </summary>
	public string titil;
	/// <summary>
	/// 构造
	/// </summary>
	public MailWriteData(string name,string titil,string content){
		this.name = name;
		this.content = content;
		this.titil = titil;
	}
	/// <summary>
	/// 构造
	/// </summary>
	public MailWriteData(string name){
		this.name = name;
	}
	/// <summary>
	/// 构造
	/// </summary>
	public MailWriteData(){
		
	}
}