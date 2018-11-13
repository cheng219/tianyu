using System.Collections;
using System.Collections.Generic;

public class pt_guild_battle_integer_d556 : st.net.NetBase.Pt {
	public pt_guild_battle_integer_d556()
	{
		Id = 0xD556;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_battle_integer_d556();
	}
	public List<st.net.NetBase.guild_battle_integer_list> guild_battle_info = new List<st.net.NetBase.guild_battle_integer_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenguild_battle_info = reader.Read_ushort();
		guild_battle_info = new List<st.net.NetBase.guild_battle_integer_list>();
		for(int i_guild_battle_info = 0 ; i_guild_battle_info < lenguild_battle_info ; i_guild_battle_info ++)
		{
			st.net.NetBase.guild_battle_integer_list listData = new st.net.NetBase.guild_battle_integer_list();
			listData.fromBinary(reader);
			guild_battle_info.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenguild_battle_info = (ushort)guild_battle_info.Count;
		writer.write_short(lenguild_battle_info);
		for(int i_guild_battle_info = 0 ; i_guild_battle_info < lenguild_battle_info ; i_guild_battle_info ++)
		{
			st.net.NetBase.guild_battle_integer_list listData = guild_battle_info[i_guild_battle_info];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
