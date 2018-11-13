//==================================
//作者：邓成
//日期：2016/4/13
//用途：仙盟日志界面类
//=================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using st.net.NetBase;
using System;

public class GuildLogWnd : SubWnd {

	public UILabel labLog;
	public UILabel labTime;
	protected override void OnOpen()
	{
		base.OnOpen();
		GameCenter.guildMng.C2S_ReqGuildLog(1);
	}
	protected override void OnClose()
	{
		base.OnClose();
	}
	protected override void HandEvent(bool _bind)
	{
		base.HandEvent(_bind);
		if (_bind)
		{
			GameCenter.guildMng.OnGetGuildLogEvent += ShowLogList;
		}
		else
		{ 
			GameCenter.guildMng.OnGetGuildLogEvent -= ShowLogList;
		}
	}

	void ShowLogList()
	{
		List<guild_log> logList = GameCenter.guildMng.guildLogList;
		System.Text.StringBuilder builder = new System.Text.StringBuilder();
		System.Text.StringBuilder builderTime = new System.Text.StringBuilder();
		for (int i = 0,max = logList.Count; i < max; i++) 
		{
			string log = ConfigMng.Instance.GetGlogStringByID((int)logList[i].event_id);
			string logText = string.Empty;
			if(!string.IsNullOrEmpty(log))
			{
				string[] words = logList[i].guild_log_args.Split(',');
				logText = UIUtil.Str2Str(log,words);
			}
			builder.Append(logText);
			builderTime.Append("[ffff00]").Append(GameHelper.GetRecentTimeStr((int)logList[i].time)).Append("[-]");
			if(i < max-1)
			{
				builder.Append("\n");
				builderTime.Append("\n");
			}
		}
		if(labLog != null)labLog.text = builder.ToString();
		if(labTime != null)labTime.text = builderTime.ToString();
	}
}