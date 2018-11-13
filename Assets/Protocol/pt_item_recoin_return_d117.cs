using System.Collections;
using System.Collections.Generic;

public class pt_item_recoin_return_d117 : st.net.NetBase.Pt {
	public pt_item_recoin_return_d117()
	{
		Id = 0xD117;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_item_recoin_return_d117();
	}
	public List<st.net.NetBase.item_prop_list> item_prop_list = new List<st.net.NetBase.item_prop_list>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenitem_prop_list = reader.Read_ushort();
		item_prop_list = new List<st.net.NetBase.item_prop_list>();
		for(int i_item_prop_list = 0 ; i_item_prop_list < lenitem_prop_list ; i_item_prop_list ++)
		{
			st.net.NetBase.item_prop_list listData = new st.net.NetBase.item_prop_list();
			listData.fromBinary(reader);
			item_prop_list.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenitem_prop_list = (ushort)item_prop_list.Count;
		writer.write_short(lenitem_prop_list);
		for(int i_item_prop_list = 0 ; i_item_prop_list < lenitem_prop_list ; i_item_prop_list ++)
		{
			st.net.NetBase.item_prop_list listData = item_prop_list[i_item_prop_list];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
