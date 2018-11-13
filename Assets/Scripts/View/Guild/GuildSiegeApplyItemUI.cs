//===============================
//作者：邓成
//日期：2016/5/18
//用途：攻城战申请列表子界面类
//===============================

using UnityEngine;
using System.Collections;
using st.net.NetBase;

public class GuildSiegeApplyItemUI : MonoBehaviour {
	public UILabel labRank;
	public UILabel guildName;
	public UILabel labLevel;
	public UILabel president;
	public UILabel memCount;
	public UILabel fightValue;

	public void SetData(req_apply_list item)
	{
		if(labRank != null)labRank.text = item.rank.ToString();
		if(guildName != null)guildName.text = item.guild_name.ToString();
		if(labLevel != null)labLevel.text = item.guild_lev.ToString();
		if(president != null)president.text = item.leader_name.ToString();
		if(memCount != null)memCount.text = item.member_num.ToString();
		if(fightValue != null)fightValue.text = item.guild_fight.ToString();
	}
}
