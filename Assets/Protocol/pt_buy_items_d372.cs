using System.Collections;
using System.Collections.Generic;

public class pt_buy_items_d372 : st.net.NetBase.Pt {
	public pt_buy_items_d372()
	{
		Id = 0xD372;
	}
	public override st.net.NetBase.Pt createNew()
	{
		return new pt_buy_items_d372();
	}
	public List<st.net.NetBase.item_des> all_buy_items = new List<st.net.NetBase.item_des>();
	public override void fromBinary(byte[] binary)
	{
		reader = new st.net.NetBase.ByteReader(binary);
		ushort lenall_buy_items = reader.Read_ushort();
		all_buy_items = new List<st.net.NetBase.item_des>();
		for(int i_all_buy_items = 0 ; i_all_buy_items < lenall_buy_items ; i_all_buy_items ++)
		{
			st.net.NetBase.item_des listData = new st.net.NetBase.item_des();
			listData.fromBinary(reader);
			all_buy_items.Add(listData);
		}
	}

	public override byte[] toBinary()
	{
		writer = new st.net.NetBase.ByteWriter();
		ushort lenall_buy_items = (ushort)all_buy_items.Count;
		writer.write_short(lenall_buy_items);
		for(int i_all_buy_items = 0 ; i_all_buy_items < lenall_buy_items ; i_all_buy_items ++)
		{
			st.net.NetBase.item_des listData = all_buy_items[i_all_buy_items];
			listData.toBinary(writer);
		}
		return writer.data;
	}

}
