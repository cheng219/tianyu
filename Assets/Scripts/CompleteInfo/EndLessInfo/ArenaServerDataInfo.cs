//============================
//作者：何明军
//日期：2016/3/23
//用途：竞技场系统数据
//============================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArenaServerDataInfo{
	/// <summary>
	/// 排名
	/// </summary>
	public int rank;
    /// <summary>
    /// 上个赛季的排名(用来计算奖励)
    /// </summary>
    public int reward_rank;
	/// <summary>
	/// 挑战冷却时间
	/// </summary>
	public int surplus_time;
	/// <summary>
	/// 挑战次数
	/// </summary>
	public int challenge_num;
	/// <summary>
	/// 奖励冷却时间
	/// </summary>
	public int reward_countdown;
	/// <summary>
	/// 奖励冷却状态
	/// </summary>
	public int state;
    /// <summary>
    /// 竞技场购买次数
    /// </summary>
    public int buyChallengeTimes;
	List<st.net.NetBase.robot_list> robot_list = new List<st.net.NetBase.robot_list>();
	List<st.net.NetBase.log_list> log_list = new List<st.net.NetBase.log_list>();
	/// <summary>
	/// 获取log
	/// </summary>
	public List<string> GetLogList(){
		List<string> strs = new List<string>();
		string[] strval = null;
		//===========fix list禁止用foreach，应用for
		for (int j = 0; j < log_list.Count; j++)
		{
			st.net.NetBase.log_list data = log_list[j];
			string[] sp = data.arg.Split(',');
			strval = new string[sp.Length + 1];
			strval[0] = GameCenter.duplicateMng.ItemTime((int)data.challenge_time, true);
			for (int i = 0, len = sp.Length; i < len; i++)
			{
				strval[i + 1] = sp[i];
			}
			strs.Add(ConfigMng.Instance.GetUItext((int)data.log_id, strval));
		}
		return strs;
	}
	/// <summary>
	/// 获取玩家
	/// </summary>
	public List<OtherPlayerInfo> GetPlayer(){
		List<OtherPlayerInfo> strs = new List<OtherPlayerInfo>();
		OtherPlayerData player = null;

		for (int i = 0; i < robot_list.Count; i++)
		{
			st.net.NetBase.robot_list data = robot_list[i];
			player = new OtherPlayerData();
			player.serverInstanceID = (int)data.uid;
			player.name = data.name;
			player.baseValueDic[ActorBaseTag.FightValue] = data.battle;
			player.prof = (int)data.prof;
			strs.Add(new OtherPlayerInfo(player));
		}
		return strs;
	}
	/// <summary>
	/// 获取奖励内容
	/// </summary>
	public List<EquipmentInfo> GetRankRewardItems(){
		List<EquipmentInfo> strs = new List<EquipmentInfo>();
        ArenaRankRef refData = ConfigMng.Instance.GetArenaRankRef(reward_rank > 100 ? 100 : reward_rank);
		if(refData == null){
			return strs;
		}
		for (int i = 0; i < refData.reward.Count; i++)
		{
			ItemValue data = refData.reward[i];
			strs.Add(new EquipmentInfo(data, EquipmentBelongTo.PREVIEW));
		}
		return strs;
	}
	/// <summary>
	/// 排行信息
	/// </summary>
	public string GetPlayerRank(int uid){
		for (int i = 0; i < robot_list.Count; i++)
		{
			st.net.NetBase.robot_list data = robot_list[i];
			if (data.uid == uid)
			{
				return data.rank.ToString();
			}
		}
		return string.Empty;
	}
	/// <summary>
	/// 构造
	/// </summary>
	public ArenaServerDataInfo(){
	}
	/// <summary>
	/// 构造
	/// </summary>
	public ArenaServerDataInfo(pt_pk_info_d485 info){
		rank = info.rank;
        reward_rank = info.reward_rank;
		surplus_time = info.surplus_time;
		challenge_num = info.challenge_num;
		reward_countdown = info.reward_countdown;
		state = info.state;
        buyChallengeTimes = info.buy_challenge_times;

		uint minRank = 0;
		for(int i=0;i<info.robot_list.Count;i++){
			minRank = info.robot_list[i].rank;
			int dex = 0;
			for(int j=0;j<info.robot_list.Count;j++){
				if(robot_list.Count <= j){
					robot_list.Add(info.robot_list[j]);
				}
				if(minRank > info.robot_list[j].rank){
					dex ++;
				}
			}
			robot_list[dex] = info.robot_list[i];
		}

		log_list = info.log_list;
	}
}
