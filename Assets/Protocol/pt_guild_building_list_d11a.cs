using System.Collections;
using System.Collections.Generic;

public class pt_guild_building_list_d11a : st.net.NetBase.Pt {
	public pt_guild_building_list_d11a()
	{
		Id = 0xD11A;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_building_list_d11a();
	}
	public List<st.net.NetBase.guild_building_list> guild_building_list = new List<st.net.NetBase.guild_building_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenguild_building_list = reader.Read_ushort();
		guild_building_list = new List<st.net.NetBase.guild_building_list>();
		for(int i_guild_building_list = 0 ; i_guild_building_list < lenguild_building_list ; i_guild_building_list ++)
		{
			st.net.NetBase.guild_building_list listData = new st.net.NetBase.guild_building_list();
			listData.fromBinary(reader);
			guild_building_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenguild_building_list = (ushort)guild_building_list.Count;
		writer.write_short(lenguild_building_list);
		for(int i_guild_building_list = 0 ; i_guild_building_list < lenguild_building_list ; i_guild_building_list ++)
		{
			st.net.NetBase.guild_building_list listData = guild_building_list[i_guild_building_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
