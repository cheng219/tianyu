using System.Collections;
using System.Collections.Generic;

public class pt_other_guild_bonfire_info_d741 : st.net.NetBase.Pt {
	public pt_other_guild_bonfire_info_d741()
	{
		Id = 0xD741;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_other_guild_bonfire_info_d741();
	}
	public List<st.net.NetBase.other_bonfire_list> other_bonfire_list = new List<st.net.NetBase.other_bonfire_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenother_bonfire_list = reader.Read_ushort();
		other_bonfire_list = new List<st.net.NetBase.other_bonfire_list>();
		for(int i_other_bonfire_list = 0 ; i_other_bonfire_list < lenother_bonfire_list ; i_other_bonfire_list ++)
		{
			st.net.NetBase.other_bonfire_list listData = new st.net.NetBase.other_bonfire_list();
			listData.fromBinary(reader);
			other_bonfire_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenother_bonfire_list = (ushort)other_bonfire_list.Count;
		writer.write_short(lenother_bonfire_list);
		for(int i_other_bonfire_list = 0 ; i_other_bonfire_list < lenother_bonfire_list ; i_other_bonfire_list ++)
		{
			st.net.NetBase.other_bonfire_list listData = other_bonfire_list[i_other_bonfire_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
