using System.Collections;
using System.Collections.Generic;

public class pt_treasure_house_info_d390 : st.net.NetBase.Pt {
	public pt_treasure_house_info_d390()
	{
		Id = 0xD390;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_treasure_house_info_d390();
	}
	public List<int> empty_bags = new List<int>();
	public List<st.net.NetBase.item_des> item_list = new List<st.net.NetBase.item_des>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenempty_bags = reader.Read_ushort();
		empty_bags = new List<int>();
		for(int i_empty_bags = 0 ; i_empty_bags < lenempty_bags ; i_empty_bags ++)
		{
			int listData = reader.Read_int();
			empty_bags.Add(listData);
		}
		ushort lenitem_list = reader.Read_ushort();
		item_list = new List<st.net.NetBase.item_des>();
		for(int i_item_list = 0 ; i_item_list < lenitem_list ; i_item_list ++)
		{
			st.net.NetBase.item_des listData = new st.net.NetBase.item_des();
			listData.fromBinary(reader);
			item_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenempty_bags = (ushort)empty_bags.Count;
		writer.write_short(lenempty_bags);
		for(int i_empty_bags = 0 ; i_empty_bags < lenempty_bags ; i_empty_bags ++)
		{
			int listData = empty_bags[i_empty_bags];
			writer.write_int(listData);
		}
		ushort lenitem_list = (ushort)item_list.Count;
		writer.write_short(lenitem_list);
		for(int i_item_list = 0 ; i_item_list < lenitem_list ; i_item_list ++)
		{
			st.net.NetBase.item_des listData = item_list[i_item_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
