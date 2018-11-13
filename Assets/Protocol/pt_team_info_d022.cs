using System.Collections;
using System.Collections.Generic;

public class pt_team_info_d022 : st.net.NetBase.Pt {
	public pt_team_info_d022()
	{
		Id = 0xD022;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_team_info_d022();
	}
	public uint team_id;
	public uint leader_id;
	public uint target;
	public List<st.net.NetBase.team_member_list> team_member_list = new List<st.net.NetBase.team_member_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		team_id = reader.Read_uint();
		leader_id = reader.Read_uint();
		target = reader.Read_uint();
		ushort lenteam_member_list = reader.Read_ushort();
		team_member_list = new List<st.net.NetBase.team_member_list>();
		for(int i_team_member_list = 0 ; i_team_member_list < lenteam_member_list ; i_team_member_list ++)
		{
			st.net.NetBase.team_member_list listData = new st.net.NetBase.team_member_list();
			listData.fromBinary(reader);
			team_member_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(team_id);
		writer.write_int(leader_id);
		writer.write_int(target);
		ushort lenteam_member_list = (ushort)team_member_list.Count;
		writer.write_short(lenteam_member_list);
		for(int i_team_member_list = 0 ; i_team_member_list < lenteam_member_list ; i_team_member_list ++)
		{
			st.net.NetBase.team_member_list listData = team_member_list[i_team_member_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
