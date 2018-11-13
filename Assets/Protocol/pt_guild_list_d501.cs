using System.Collections;
using System.Collections.Generic;

public class pt_guild_list_d501 : st.net.NetBase.Pt {
	public pt_guild_list_d501()
	{
		Id = 0xD501;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_list_d501();
	}
	public List<st.net.NetBase.guild_list_info> all_guild_info = new List<st.net.NetBase.guild_list_info>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenall_guild_info = reader.Read_ushort();
		all_guild_info = new List<st.net.NetBase.guild_list_info>();
		for(int i_all_guild_info = 0 ; i_all_guild_info < lenall_guild_info ; i_all_guild_info ++)
		{
			st.net.NetBase.guild_list_info listData = new st.net.NetBase.guild_list_info();
			listData.fromBinary(reader);
			all_guild_info.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenall_guild_info = (ushort)all_guild_info.Count;
		writer.write_short(lenall_guild_info);
		for(int i_all_guild_info = 0 ; i_all_guild_info < lenall_guild_info ; i_all_guild_info ++)
		{
			st.net.NetBase.guild_list_info listData = all_guild_info[i_all_guild_info];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
