using System.Collections;
using System.Collections.Generic;

public class pt_guild_check_out_item_ask_list_d524 : st.net.NetBase.Pt {
	public pt_guild_check_out_item_ask_list_d524()
	{
		Id = 0xD524;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_check_out_item_ask_list_d524();
	}
	public List<st.net.NetBase.guild_check_out_item_ask_list> ask_list = new List<st.net.NetBase.guild_check_out_item_ask_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenask_list = reader.Read_ushort();
		ask_list = new List<st.net.NetBase.guild_check_out_item_ask_list>();
		for(int i_ask_list = 0 ; i_ask_list < lenask_list ; i_ask_list ++)
		{
			st.net.NetBase.guild_check_out_item_ask_list listData = new st.net.NetBase.guild_check_out_item_ask_list();
			listData.fromBinary(reader);
			ask_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenask_list = (ushort)ask_list.Count;
		writer.write_short(lenask_list);
		for(int i_ask_list = 0 ; i_ask_list < lenask_list ; i_ask_list ++)
		{
			st.net.NetBase.guild_check_out_item_ask_list listData = ask_list[i_ask_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
