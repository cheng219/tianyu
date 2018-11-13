using System.Collections;
using System.Collections.Generic;

public class pt_guild_item_info_d379 : st.net.NetBase.Pt {
	public pt_guild_item_info_d379()
	{
		Id = 0xD379;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_guild_item_info_d379();
	}
	public List<int> emptys = new List<int>();
	public List<st.net.NetBase.item_des> guld_items = new List<st.net.NetBase.item_des>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenemptys = reader.Read_ushort();
		emptys = new List<int>();
		for(int i_emptys = 0 ; i_emptys < lenemptys ; i_emptys ++)
		{
			int listData = reader.Read_int();
			emptys.Add(listData);
		}
		ushort lenguld_items = reader.Read_ushort();
		guld_items = new List<st.net.NetBase.item_des>();
		for(int i_guld_items = 0 ; i_guld_items < lenguld_items ; i_guld_items ++)
		{
			st.net.NetBase.item_des listData = new st.net.NetBase.item_des();
			listData.fromBinary(reader);
			guld_items.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenemptys = (ushort)emptys.Count;
		writer.write_short(lenemptys);
		for(int i_emptys = 0 ; i_emptys < lenemptys ; i_emptys ++)
		{
			int listData = emptys[i_emptys];
			writer.write_int(listData);
		}
		ushort lenguld_items = (ushort)guld_items.Count;
		writer.write_short(lenguld_items);
		for(int i_guld_items = 0 ; i_guld_items < lenguld_items ; i_guld_items ++)
		{
			st.net.NetBase.item_des listData = guld_items[i_guld_items];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
