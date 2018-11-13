using System.Collections;
using System.Collections.Generic;

public class pt_guild_copy_enter_d11e : st.net.NetBase.Pt {
	public pt_guild_copy_enter_d11e()
	{
		Id = 0xD11E;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_copy_enter_d11e();
	}
	public List<st.net.NetBase.guild_copy_enter_list> guild_copy_enter_list = new List<st.net.NetBase.guild_copy_enter_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenguild_copy_enter_list = reader.Read_ushort();
		guild_copy_enter_list = new List<st.net.NetBase.guild_copy_enter_list>();
		for(int i_guild_copy_enter_list = 0 ; i_guild_copy_enter_list < lenguild_copy_enter_list ; i_guild_copy_enter_list ++)
		{
			st.net.NetBase.guild_copy_enter_list listData = new st.net.NetBase.guild_copy_enter_list();
			listData.fromBinary(reader);
			guild_copy_enter_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenguild_copy_enter_list = (ushort)guild_copy_enter_list.Count;
		writer.write_short(lenguild_copy_enter_list);
		for(int i_guild_copy_enter_list = 0 ; i_guild_copy_enter_list < lenguild_copy_enter_list ; i_guild_copy_enter_list ++)
		{
			st.net.NetBase.guild_copy_enter_list listData = guild_copy_enter_list[i_guild_copy_enter_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
