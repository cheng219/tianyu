using System.Collections;
using System.Collections.Generic;

public class pt_all_guild_list_info_d111 : st.net.NetBase.Pt {
	public pt_all_guild_list_info_d111()
	{
		Id = 0xD111;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_all_guild_list_info_d111();
	}
	public uint guild_state;
	public List<st.net.NetBase.guild_info_list> guild_info_list = new List<st.net.NetBase.guild_info_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		guild_state = reader.Read_uint();
		ushort lenguild_info_list = reader.Read_ushort();
		guild_info_list = new List<st.net.NetBase.guild_info_list>();
		for(int i_guild_info_list = 0 ; i_guild_info_list < lenguild_info_list ; i_guild_info_list ++)
		{
			st.net.NetBase.guild_info_list listData = new st.net.NetBase.guild_info_list();
			listData.fromBinary(reader);
			guild_info_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		writer.write_int(guild_state);
		ushort lenguild_info_list = (ushort)guild_info_list.Count;
		writer.write_short(lenguild_info_list);
		for(int i_guild_info_list = 0 ; i_guild_info_list < lenguild_info_list ; i_guild_info_list ++)
		{
			st.net.NetBase.guild_info_list listData = guild_info_list[i_guild_info_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
