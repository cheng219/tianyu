using System.Collections;
using System.Collections.Generic;

public class pt_guild_member_info_d112 : st.net.NetBase.Pt {
	public pt_guild_member_info_d112()
	{
		Id = 0xD112;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_member_info_d112();
	}
	public List<st.net.NetBase.guild_member_list> guild_member_list = new List<st.net.NetBase.guild_member_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenguild_member_list = reader.Read_ushort();
		guild_member_list = new List<st.net.NetBase.guild_member_list>();
		for(int i_guild_member_list = 0 ; i_guild_member_list < lenguild_member_list ; i_guild_member_list ++)
		{
			st.net.NetBase.guild_member_list listData = new st.net.NetBase.guild_member_list();
			listData.fromBinary(reader);
			guild_member_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenguild_member_list = (ushort)guild_member_list.Count;
		writer.write_short(lenguild_member_list);
		for(int i_guild_member_list = 0 ; i_guild_member_list < lenguild_member_list ; i_guild_member_list ++)
		{
			st.net.NetBase.guild_member_list listData = guild_member_list[i_guild_member_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
