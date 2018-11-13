using System.Collections;
using System.Collections.Generic;

public class pt_guild_copy_trophy_d11d : st.net.NetBase.Pt {
	public pt_guild_copy_trophy_d11d()
	{
		Id = 0xD11D;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_copy_trophy_d11d();
	}
	public List<st.net.NetBase.guild_copy_trophy_list> guild_copy_trophy_list = new List<st.net.NetBase.guild_copy_trophy_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenguild_copy_trophy_list = reader.Read_ushort();
		guild_copy_trophy_list = new List<st.net.NetBase.guild_copy_trophy_list>();
		for(int i_guild_copy_trophy_list = 0 ; i_guild_copy_trophy_list < lenguild_copy_trophy_list ; i_guild_copy_trophy_list ++)
		{
			st.net.NetBase.guild_copy_trophy_list listData = new st.net.NetBase.guild_copy_trophy_list();
			listData.fromBinary(reader);
			guild_copy_trophy_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenguild_copy_trophy_list = (ushort)guild_copy_trophy_list.Count;
		writer.write_short(lenguild_copy_trophy_list);
		for(int i_guild_copy_trophy_list = 0 ; i_guild_copy_trophy_list < lenguild_copy_trophy_list ; i_guild_copy_trophy_list ++)
		{
			st.net.NetBase.guild_copy_trophy_list listData = guild_copy_trophy_list[i_guild_copy_trophy_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
