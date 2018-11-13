using System.Collections;
using System.Collections.Generic;

public class pt_win_list_d467 : st.net.NetBase.Pt {
	public pt_win_list_d467()
	{
		Id = 0xD467;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_win_list_d467();
	}
	public int star_num;
	public int time;
	public List<st.net.NetBase.reward_list> reward_list = new List<st.net.NetBase.reward_list>();
	public int scene_type;
	public List<st.net.NetBase.team_reward_list> team_reward_list = new List<st.net.NetBase.team_reward_list>();
	public int kill_boss_num;
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		star_num = reader.Read_int();
		time = reader.Read_int();
		ushort lenreward_list = reader.Read_ushort();
		reward_list = new List<st.net.NetBase.reward_list>();
		for(int i_reward_list = 0 ; i_reward_list < lenreward_list ; i_reward_list ++)
		{
			st.net.NetBase.reward_list listData = new st.net.NetBase.reward_list();
			listData.fromBinary(reader);
			reward_list.Add(listData);
		}
		scene_type = reader.Read_int();
		ushort lenteam_reward_list = reader.Read_ushort();
		team_reward_list = new List<st.net.NetBase.team_reward_list>();
		for(int i_team_reward_list = 0 ; i_team_reward_list < lenteam_reward_list ; i_team_reward_list ++)
		{
			st.net.NetBase.team_reward_list listData = new st.net.NetBase.team_reward_list();
			listData.fromBinary(reader);
			team_reward_list.Add(listData);
		}
		kill_boss_num = reader.Read_int();
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(star_num);
		writer.write_int(time);
		ushort lenreward_list = (ushort)reward_list.Count;
		writer.write_short(lenreward_list);
		for(int i_reward_list = 0 ; i_reward_list < lenreward_list ; i_reward_list ++)
		{
			st.net.NetBase.reward_list listData = reward_list[i_reward_list];
			listData.toBinary(writer);
		}
		writer.write_int(scene_type);
		ushort lenteam_reward_list = (ushort)team_reward_list.Count;
		writer.write_short(lenteam_reward_list);
		for(int i_team_reward_list = 0 ; i_team_reward_list < lenteam_reward_list ; i_team_reward_list ++)
		{
			st.net.NetBase.team_reward_list listData = team_reward_list[i_team_reward_list];
			listData.toBinary(writer);
		}
		writer.write_int(kill_boss_num);
		return writer.data;
	}

}
