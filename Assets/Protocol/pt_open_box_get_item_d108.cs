using System.Collections;
using System.Collections.Generic;

public class pt_open_box_get_item_d108 : st.net.NetBase.Pt {
	public pt_open_box_get_item_d108()
	{
		Id = 0xD108;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_open_box_get_item_d108();
	}
	public List<st.net.NetBase.item_list> item_list = new List<st.net.NetBase.item_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenitem_list = reader.Read_ushort();
		item_list = new List<st.net.NetBase.item_list>();
		for(int i_item_list = 0 ; i_item_list < lenitem_list ; i_item_list ++)
		{
			st.net.NetBase.item_list listData = new st.net.NetBase.item_list();
			listData.fromBinary(reader);
			item_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenitem_list = (ushort)item_list.Count;
		writer.write_short(lenitem_list);
		for(int i_item_list = 0 ; i_item_list < lenitem_list ; i_item_list ++)
		{
			st.net.NetBase.item_list listData = item_list[i_item_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
