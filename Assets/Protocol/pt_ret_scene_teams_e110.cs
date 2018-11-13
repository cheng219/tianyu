using System.Collections;
using System.Collections.Generic;

public class pt_ret_scene_teams_e110 : st.net.NetBase.Pt {
	public pt_ret_scene_teams_e110()
	{
		Id = 0xE110;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_ret_scene_teams_e110();
	}
	public List<st.net.NetBase.team_info> team_info = new List<st.net.NetBase.team_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenteam_info = reader.Read_ushort();
		team_info = new List<st.net.NetBase.team_info>();
		for(int i_team_info = 0 ; i_team_info < lenteam_info ; i_team_info ++)
		{
			st.net.NetBase.team_info listData = new st.net.NetBase.team_info();
			listData.fromBinary(reader);
			team_info.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenteam_info = (ushort)team_info.Count;
		writer.write_short(lenteam_info);
		for(int i_team_info = 0 ; i_team_info < lenteam_info ; i_team_info ++)
		{
			st.net.NetBase.team_info listData = team_info[i_team_info];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
