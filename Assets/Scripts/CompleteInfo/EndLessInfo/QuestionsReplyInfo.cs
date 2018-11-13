//======================================================
//作者:何明军
//日期:2016/7/6
//用途:问题描述数据
//======================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class QuestionsReplyInfo : IComparable<QuestionsReplyInfo>{
	protected int id;
	/// <summary>
	/// 服务端唯一id
	/// </summary>
	public int ID{
		get{return id;}
	}
	protected string titil;
	/// <summary>
	/// 标题
	/// </summary>
	public string Titil{
		get{
			return titil;
		}
	}
	protected string des;
	/// <summary>
	/// 问题描述
	/// </summary>
	public string Des{
		get {return des;}
	}
	protected string replyDes;
	/// <summary>
	/// 回复内容
	/// </summary>
	public string ReplyDes{
		get{return replyDes;}
	}
	protected int type;
	/// <summary>
	/// 问题类型
	/// </summary>
	public int Type{
		get{return type;}
	}
	protected int evaluation;
	/// <summary>
	/// 评价
	/// </summary>
	public int Evaluation{
		get{return evaluation;}
	}
	
	protected string time;
	/// <summary>
	/// 提问时间
	/// </summary>
	public string Time{
		get{
			return time;
//			return string.Format("yyyyMMdd hhmmss",time.ToString());
		}
	}
	/// <summary>
	/// 是否回复
	/// </summary>
	public bool IsReply{
		get{
			if(!string.IsNullOrEmpty(replyDes) || evaluation > 0){
				return true;
			}
			return false;
		}
	}
	
	public enum PathType{
		NONE = 0,
		/// <summary>
		/// 上报路径
		/// </summary>
		REPROT = 1,
		/// <summary>
		/// 回复路径
		/// </summary>
		REPLY = 2,
		/// <summary>
		/// 评价路径
		/// </summary>
		EVALUATE=3,
		/// <summary>
		/// 所有问题的前10条路径
		/// </summary>
		QUESTIONSALL=4,
	}
	/// <summary>
	/// 访问路径类型
	/// </summary>
	public PathType GetPathType{
		get{
			if(id == 0){
				return PathType.REPROT;
			}
			if(!IsReply){
				return PathType.REPLY;
			}else if(Evaluation <= 0){
				return PathType.EVALUATE;
			}
			return PathType.NONE;
		}
	}
	
	public string GetPath(PathType _pathType){
		string source = LynSdkManager.Instance.GetSourceId();
		string server = GameCenter.loginMng.LoginServerID;
		string user = GameCenter.loginMng.Login_Name;
		int role = GameCenter.mainPlayerMng.MainPlayerInfo.ServerInstanceID;
		DateTime oldTime = new DateTime(1970,1,1);
		ulong times = (ulong)(DateTime.Now - oldTime).TotalMilliseconds;
		string sign = UIUtil.MD5Encrypt("act"+8+source+server+user+times+role+"LYN");
		string path = string.Empty;
		switch(_pathType){
		case PathType.REPROT:
			path = string.Format(
				"http://xyuc.zhangwangkj.cn:7001/GM/service?act=Report&GameID={0}&SourceID={1}&ServerNo={2}&UserID={3}&RoleId={4}&Times={5}&Type={6}&Titil={7}&Content={8}&Sign={9}",
				8,source,server,user,role,times,Type,UIUtil.EncodeBase64(Titil),UIUtil.EncodeBase64(Des),sign);
			return path;
		case PathType.REPLY:
			path = string.Format("http://xyuc.zhangwangkj.cn:7001/GM/service?act=GetReply&ReportID={0}&GameID={1}&SourceID={2}&ServerNo={3}&UserID={4}&RoleId={5}&Sign={6}",
				ID,8,source,server,user,role,times,sign);
			return path;
		case PathType.EVALUATE:
			path = string.Format(
				"http://xyuc.zhangwangkj.cn:7001/GM/service?act=Assess&GameID={0}&SourceID={1}&ServerNo={2}&UserID={3}&RoleId={4}&Times={5}&ReportID={6}&Assess={7}&Sign={8}",
				8,source,server,user,role,times,ID,Evaluation,sign);
			return path;
		case PathType.QUESTIONSALL:
			path = string.Format(
				"http://xyuc.zhangwangkj.cn:7001/GM/service?act=GetInfos&GameID={0}&SourceID={1}&ServerNo={2}&UserID={3}&RoleId={4}&Times={5}&Sign={6}",
				8,source,server,user,role,times,sign);
			return path;
		default:
			return path;
		}
		//return path;
	}
	/// <summary>
	/// 上报路径
	/// </summary>
	public string ReprotPath{
		get{
			return GetPath(PathType.REPROT);
		}
	}
	
	/// <summary>
	/// 评价路径
	/// </summary>
	public string EvaluatePath{
		get{
			return GetPath(PathType.EVALUATE);
		}
	}
	/// <summary>
	/// 请求回复路径
	/// </summary>
	public string ReplyPath{
		get{
			return GetPath(PathType.REPLY);
		}
	}
	
	public QuestionsReplyInfo(){}
	/// <summary>
	/// 上报问题构造
	/// </summary>
	public QuestionsReplyInfo(string _titil,string _content,int _type){
		titil = _titil;
		des = _content;
		type = _type;
	}
	/// <summary>
	/// 请求评价构造
	/// </summary>
	public QuestionsReplyInfo(int _id,int _evaluation){
		id = _id;
		evaluation = _evaluation;
	}
	
	public QuestionsReplyInfo(int _id,int _evaluation,int _type,string _time,string _replyDes,string _info){
		id = _id;
		evaluation = _evaluation;
		string[] str = UIUtil.DecodeBase64(_info).Split('|');
		titil = str[0];
		des = str[1];
		type = _type;
		time = _time;
		replyDes = UIUtil.DecodeBase64(_replyDes);
	}
	/// <summary>
	/// 上报成功更新
	/// </summary>
	public void Update(string _id,string _time){
		id = Convert.ToInt32(_id);
		time = _time;
	}
	/// <summary>
	/// 更新回复
	/// </summary>
	public void Update(string _replyDes){
		replyDes = UIUtil.DecodeBase64(_replyDes);
	}
	/// <summary>
	/// 更新评价
	/// </summary>
	public void Update(int _evaluation){
		evaluation = _evaluation;
	}
	
	/// <summary>
	/// 排序
	/// </summary>
	public int CompareTo(QuestionsReplyInfo x){
		if(x == null)return 1;
		return this.IsReply.CompareTo(x.IsReply);
	}
}
