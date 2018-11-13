//============================
//作者：何明军
//日期：2016/3/23
//用途：副本系统数据
//============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CopySceneTeamPlayerInfo{
	/// <summary>
	/// 玩家ID
	/// </summary>
	public int pId;
	/// <summary>
	/// 玩家次数
	/// </summary>
	public int pNum;
	/// <summary>
	/// 玩家准备
	/// </summary>
	public bool isPerpare;
	/// <summary>
	/// 是否是化身
	/// </summary>
	public bool isAvatar = false;
	/// <summary>
	/// 构造
	/// </summary>
	public CopySceneTeamPlayerInfo(st.net.NetBase.member_challengenum_list info){
		pId = (int)info.uid;
		pNum = (int)info.challenge_num;
		isPerpare = info.prepare == 1;
	}
	/// <summary>
	/// 更新
	/// </summary>
	public void UpdateData(int _star){
		pNum = _star;
	}
	/// <summary>
	/// 更新
	/// </summary>
	public void UpdateData(uint uid,uint _perpare){
		isPerpare = _perpare == 1;
	}
	/// <summary>
	/// 构造
	/// </summary>
	public CopySceneTeamPlayerInfo(int _uid,uint _perpare,bool _isAvatar = false){
		pId = (int)_uid;
		isPerpare = _perpare == 1;
		isAvatar = _isAvatar;
	}
}
