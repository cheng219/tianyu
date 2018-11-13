using System.Collections;
using System.Collections.Generic;

public class pt_guild_skill_list_d514 : st.net.NetBase.Pt {
	public pt_guild_skill_list_d514()
	{
		Id = 0xD514;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_skill_list_d514();
	}
	public List<int> guild_skills = new List<int>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenguild_skills = reader.Read_ushort();
		guild_skills = new List<int>();
		for(int i_guild_skills = 0 ; i_guild_skills < lenguild_skills ; i_guild_skills ++)
		{
			int listData = reader.Read_int();
			guild_skills.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenguild_skills = (ushort)guild_skills.Count;
		writer.write_short(lenguild_skills);
		for(int i_guild_skills = 0 ; i_guild_skills < lenguild_skills ; i_guild_skills ++)
		{
			int listData = guild_skills[i_guild_skills];
			writer.write_int(listData);
		}
		return writer.data;
	}

}
