//============================
//作者：何明军
//日期：2016/3/23
//用途：无尽挑战系统数据
//============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndLessTrialsDataInfo{
	/// <summary>
	/// 章节ID
	/// </summary>
	public int id;
	/// <summary>
	/// 关卡列表
	/// </summary>
	public Dictionary<int,EndLessTrialsDataInfo.EndLessTrialsItemData> itemsList = new Dictionary<int,EndLessTrialsItemData>();
	/// <summary>
	/// 星级列表
	/// </summary>
	public Dictionary<int,EndLessTrialsDataInfo.EndLessTrialsStarData> starsList = new Dictionary<int,EndLessTrialsStarData>();
	/// <summary>
	/// 关卡数据结构
	/// </summary>
	public class EndLessTrialsItemData{
		/// <summary>
		/// 关卡ID
		/// </summary>
		public int id;
		/// <summary>
		/// 关卡是否进入
		/// </summary>
		public int enter;
		/// <summary>
		/// 关卡星级
		/// </summary>
		public int star;
		/// <summary>
		/// 关卡通关时间
		/// </summary>
		public int time;
		/// <summary>
		/// 关卡通关时间
		/// </summary>
		public string ItemTime{
			get{
				int tmp = time;
				int s = tmp%60;
				tmp /= 60;
				int m = tmp%60;
				int h = tmp/60;
				if(h>0)
				{
					return string.Format("{0:D2}:{1:D2}:{2:D2}",h,m,s);
				}
				else
				{
					return string.Format("{0:D2}:{1:D2}",m,s);
				}
				//return "0:0";
			}
		}
		/// <summary>
		/// 构造
		/// </summary>
		public EndLessTrialsItemData(){}
		/// <summary>
		/// 构造
		/// </summary>
		public EndLessTrialsItemData(st.net.NetBase.pass_list item){Update(item);}
		/// <summary>
		/// 更新
		/// </summary>
		public void Update(st.net.NetBase.pass_list item){
			this.id  = (int)item.pass_id;
			this.enter = (int)item.pass_state;
			this.star = (int)item.star_num;
			this.time = (int)item.pass_time;
		}
	}
	/// <summary>
	/// 星星结构
	/// </summary>
	public class EndLessTrialsStarData{
		/// <summary>
		/// 星星ID
		/// </summary>
		public int id;
		/// <summary>
		/// 星星领取状态
		/// </summary>
		public bool receive;
		/// <summary>
		/// 构造
		/// </summary>
		public EndLessTrialsStarData(){}
		/// <summary>
		/// 构造
		/// </summary>
		public EndLessTrialsStarData(st.net.NetBase.pass_star_list item){
			Update(item);
		}
		/// <summary>
		/// 更新
		/// </summary>
		public void Update(st.net.NetBase.pass_star_list item){
			this.id =(int)item.star_id;
			this.receive = item.star_state == 1;
		}
	}
	/// <summary>
	/// 构造
	/// </summary>
	public EndLessTrialsDataInfo(){

	}
	/// <summary>
	/// 构造
	/// </summary>
	public EndLessTrialsDataInfo(st.net.NetBase.endless_list data){
		Update(data);
	}
	/// <summary>
	/// 更新
	/// </summary>
	public void Update(st.net.NetBase.endless_list data){
		this.id = (int)data.chpter_id;
		int eid = 0;
		for (int i = 0; i < data.pass_list.Count; i++)
		{
			st.net.NetBase.pass_list item = data.pass_list[i];
			eid = (int)item.pass_id;
			if (!itemsList.ContainsKey(eid))
			{
				itemsList[eid] = new EndLessTrialsItemData(item);
			}
			else
			{
				itemsList[eid].Update(item);
			}
		}
		for (int i = 0; i < data.pass_star_list.Count; i++)
		{
			st.net.NetBase.pass_star_list item = data.pass_star_list[i];
			eid = (int)item.star_id;
			if (!starsList.ContainsKey(eid))
			{
				starsList[eid] = new EndLessTrialsStarData(item);
			}
			else
			{
				starsList[eid].Update(item);
			}
		}
	}
}
