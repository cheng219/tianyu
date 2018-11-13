//===============================
//作者：邓成
//日期：2016/6/27
//用途：仙域守护积分排行显示类
//===============================

using UnityEngine;
using System.Collections;

public class GuildProtectRankUI : MonoBehaviour {
	public UILabel labRank;
	public UILabel labName;
	public UILabel labScore;

	public void SetData(st.net.NetBase.guild_guard_rank rankData,int rank)
	{
		if(labRank != null)labRank.text = rank.ToString();
		if(labName != null)labName.text = rankData.name;
		if(labScore != null)labScore.text = rankData.damage.ToString();
	}
	public void SetData(st.net.NetBase.rank_info_base rankData,int rank)
	{
		if(labRank != null)labRank.text = rank.ToString();
		if(labName != null)labName.text = rankData.name;
		if(labScore != null)labScore.text = rankData.value1.ToString();
	}

	public GuildProtectRankUI CreateNew(Transform _parent)
	{
		GameObject obj = Instantiate(this.gameObject) as GameObject;
		obj.transform.parent = _parent;
		obj.transform.localScale = Vector3.one;
		obj.transform.localPosition = Vector3.zero;
		obj.SetActive(true);
		return obj.GetComponent<GuildProtectRankUI>();
	}
}
