using System.Collections;
using System.Collections.Generic;

public class pt_members_entry_d113 : st.net.NetBase.Pt {
	public pt_members_entry_d113()
	{
		Id = 0xD113;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_members_entry_d113();
	}
	public List<st.net.NetBase.guild_members_entry_list> guild_members_entry_list = new List<st.net.NetBase.guild_members_entry_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenguild_members_entry_list = reader.Read_ushort();
		guild_members_entry_list = new List<st.net.NetBase.guild_members_entry_list>();
		for(int i_guild_members_entry_list = 0 ; i_guild_members_entry_list < lenguild_members_entry_list ; i_guild_members_entry_list ++)
		{
			st.net.NetBase.guild_members_entry_list listData = new st.net.NetBase.guild_members_entry_list();
			listData.fromBinary(reader);
			guild_members_entry_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenguild_members_entry_list = (ushort)guild_members_entry_list.Count;
		writer.write_short(lenguild_members_entry_list);
		for(int i_guild_members_entry_list = 0 ; i_guild_members_entry_list < lenguild_members_entry_list ; i_guild_members_entry_list ++)
		{
			st.net.NetBase.guild_members_entry_list listData = guild_members_entry_list[i_guild_members_entry_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
